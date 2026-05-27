using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    [Header("API Configuration")]
    [SerializeField] private string apiBaseUrl       = "https://127.0.0.1:5001";
    [SerializeField] private int    idUsuario        = 1;   // usuario hardcodeado
    [SerializeField] private int    costoMiniJuego4  = 5;   // tokens necesarios para jugar

    [Header("HTTPS - DEV ONLY")]
    [Tooltip("Acepta certificados self-signed / ad-hoc. DESACTÍVALO en producción.")]
    [SerializeField] private bool aceptarCertificadosNoConfiables = true;

    [Header("Scene Names (must exist in Build Settings)")]
    [SerializeField] private string miniJuego5StartScene = "StartScene";
    [SerializeField] private string miniJuego4StartScene = "ExInGameScene";

    [Header("UI Feedback (opcional, asignar en Inspector)")]
    [SerializeField] private GameObject panelTokensInsuficientes;
    [SerializeField] private Text       textoMensaje;    // cámbialo a TMP_Text si usas TextMeshPro
    [SerializeField] private Button     botonMiniJuego4;

    // ── Botones ───────────────────────────────────────────────────────────────

    public void OpenMiniJuego5Start()
    {
        SceneManager.LoadScene(miniJuego5StartScene);
    }

    public void OpenMiniJuego4Start()
    {
        StartCoroutine(IntentarAbrirMiniJuego4());
    }

    // ── Flujo del MiniJuego4 ──────────────────────────────────────────────────

    private IEnumerator IntentarAbrirMiniJuego4()
    {
        if (botonMiniJuego4 != null) botonMiniJuego4.interactable = false;

        // 1) GET de tokens actuales
        int tokensActuales = -1;
        yield return StartCoroutine(GetTokens(result => tokensActuales = result));

        if (tokensActuales < 0)
        {
            MostrarMensaje("No se pudo consultar tu saldo. Intenta más tarde.");
            ReactivarBoton();
            yield break;
        }

        if (tokensActuales < costoMiniJuego4)
        {
            MostrarMensaje($"Tokens insuficientes. Necesitas {costoMiniJuego4}, tienes {tokensActuales}.");
            if (panelTokensInsuficientes != null) panelTokensInsuficientes.SetActive(true);
            ReactivarBoton();
            yield break;
        }

        // 2) PUT con delta negativo para descontar
        bool descuentoOk = false;
        yield return StartCoroutine(RestarTokens(costoMiniJuego4, ok => descuentoOk = ok));

        if (!descuentoOk)
        {
            MostrarMensaje("No se pudo descontar los tokens. Intenta de nuevo.");
            ReactivarBoton();
            yield break;
        }

        // 3) Cargar la escena del minijuego
        SceneManager.LoadScene(miniJuego4StartScene);
    }

    // ── Llamadas a la API ─────────────────────────────────────────────────────

    private IEnumerator GetTokens(System.Action<int> callback)
    {
        string url = $"{apiBaseUrl}/usuarios/{idUsuario}/tokens";

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            AplicarCertHandler(req);
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"GET tokens falló: {req.error} | {req.downloadHandler.text}");
                callback(-1);
                yield break;
            }

            TokensResponse resp = JsonUtility.FromJson<TokensResponse>(req.downloadHandler.text);
            callback(resp.WhirlTokens);
        }
    }

    private IEnumerator RestarTokens(int cantidad, System.Action<bool> callback)
    {
        string url      = $"{apiBaseUrl}/usuarios/{idUsuario}/tokens";
        string bodyJson = JsonUtility.ToJson(new DeltaBody { delta = -cantidad });
        byte[] bodyRaw  = Encoding.UTF8.GetBytes(bodyJson);

        using (UnityWebRequest req = new UnityWebRequest(url, "PUT"))
        {
            req.uploadHandler   = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            AplicarCertHandler(req);

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"PUT tokens falló: {req.error} | {req.downloadHandler.text}");
                callback(false);
                yield break;
            }

            TokensResponse resp = JsonUtility.FromJson<TokensResponse>(req.downloadHandler.text);
            Debug.Log($"Tokens descontados. Saldo nuevo: {resp.WhirlTokens}");
            callback(true);
        }
    }

    private void AplicarCertHandler(UnityWebRequest req)
    {
        if (aceptarCertificadosNoConfiables && apiBaseUrl.StartsWith("https"))
        {
            req.certificateHandler = new AcceptAllCertificates();
            // Importante: liberar el handler con el request
            req.disposeCertificateHandlerOnDispose = true;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void MostrarMensaje(string mensaje)
    {
        Debug.Log(mensaje);
        if (textoMensaje != null) textoMensaje.text = mensaje;
    }

    private void ReactivarBoton()
    {
        if (botonMiniJuego4 != null) botonMiniJuego4.interactable = true;
    }

    // ── DTOs para JsonUtility ─────────────────────────────────────────────────

    [System.Serializable]
    private class TokensResponse
    {
        public int WhirlTokens;
    }

    [System.Serializable]
    private class DeltaBody
    {
        public int delta;
    }

    // ── Certificate handler para certs ad-hoc / self-signed ───────────────────
    // ⚠ Acepta CUALQUIER certificado. Solo para desarrollo local.
    private class AcceptAllCertificates : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}

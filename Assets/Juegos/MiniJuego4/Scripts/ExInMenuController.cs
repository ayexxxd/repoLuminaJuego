using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

namespace DefensoresDeSoftware
{
    public class ExInMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject instructionsPanel;
        [SerializeField] private TextMeshProUGUI feedbackText;

        [Header("API")]
        public string apiBaseUrl = "https://10.14.255.45:5001";
        public int costoJuego = 5;

        public void Start()
        {
            instructionsPanel.SetActive(false);
            if (feedbackText != null) feedbackText.text = "";
        }

        public void StartToPlay()
        {
            StartCoroutine(CobrarYJugar());
        }

        private IEnumerator CobrarYJugar()
        {
            if (feedbackText != null) feedbackText.text = "";

            int idUsuario = PlayerPrefs.GetInt("userid", 4);

            // 1 — Verificar saldo actual
            using var getReq = UnityWebRequest.Get($"{apiBaseUrl}/usuarios/{idUsuario}/tokens");
            getReq.certificateHandler = new SkipCertHandler();
            yield return getReq.SendWebRequest();

            if (getReq.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"[Menu] Error al verificar tokens: {getReq.error}");
                SceneManager.LoadScene("ExInGameScene");
                yield break;
            }

            var saldoData = JsonUtility.FromJson<TokensResponse>(getReq.downloadHandler.text);
            if (saldoData.WhirlTokens < costoJuego)
            {
                if (feedbackText != null)
                    feedbackText.text = $"Necesitas {costoJuego} Whirl-Tokens para jugar.";
                yield break;
            }

            // 2 — Cobrar tokens
            byte[] bodyBytes = Encoding.UTF8.GetBytes($"{{\"delta\":{-costoJuego}}}");

            using var putReq = new UnityWebRequest($"{apiBaseUrl}/usuarios/{idUsuario}/tokens", "PUT");
            putReq.uploadHandler      = new UploadHandlerRaw(bodyBytes);
            putReq.downloadHandler    = new DownloadHandlerBuffer();
            putReq.SetRequestHeader("Content-Type", "application/json");
            putReq.certificateHandler = new SkipCertHandler();
            yield return putReq.SendWebRequest();

            if (putReq.result != UnityWebRequest.Result.Success)
                Debug.LogWarning($"[Menu] Error al cobrar tokens: {putReq.error}");
            
            Debug.Log($"[Menu] Tokens cobrados exitosamente: {costoJuego}");
            // 3 — Cargar la escena
            SceneManager.LoadScene("ExInGameScene");
        }

        // Clase auxiliar para parsear la respuesta del GET tokens
        [System.Serializable]
        private class TokensResponse
        {
            public int WhirlTokens;
        }

        private class SkipCertHandler : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData) => true;
        }

        public void ExitGame()
        {
            if (ExInGameControl.Instance != null)
                Destroy(ExInGameControl.Instance.gameObject);
            if (ExInSFXManager.Instance != null)
                Destroy(ExInSFXManager.Instance.gameObject);
            SceneManager.LoadScene("MenuScene");
        }

        public void OpenInstructions()
        {
            instructionsPanel.SetActive(true);
            StartCoroutine(ClickToContinue());
        }

        IEnumerator ClickToContinue()
        {
            yield return null;
            yield return new WaitUntil(() =>
                Keyboard.current.anyKey.wasPressedThisFrame ||
                Mouse.current.leftButton.wasPressedThisFrame);
            instructionsPanel.SetActive(false);
        }
    }
}
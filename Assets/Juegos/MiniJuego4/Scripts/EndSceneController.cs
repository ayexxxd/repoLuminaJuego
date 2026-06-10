using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Text;

namespace DefensoresDeSoftware
{
    public class EndSceneController : MonoBehaviour
    {
        [Header("Resultados")]
        public TextMeshProUGUI textoNivelAlcanzado;
        public TextMeshProUGUI textoMonedasObtenidas;
        public TextMeshProUGUI textoPuntaje;

        [Header("Paneles")]
        public GameObject panelVictoria;
        public GameObject panelDerrota;

        void Start()
        {
            int oleadas = PlayerPrefs.GetInt("OleadasCompletadas", 0);
            int monedas = PlayerPrefs.GetInt("PreguntasCorrectas", 0);
            int vidas   = PlayerPrefs.GetInt("Lives", 0);
            int puntos  = oleadas * 100 + monedas * 50;

            if (textoNivelAlcanzado  != null)
                textoNivelAlcanzado.text  = "Nivel más alto: " + oleadas;

            if (textoMonedasObtenidas != null)
                textoMonedasObtenidas.text = "Monedas obtenidas: " + monedas;

            if (textoPuntaje != null)
                textoPuntaje.text = "Puntaje: " + puntos;

            bool victoria = vidas > 0;
            if (panelVictoria != null) panelVictoria.SetActive(victoria);
            if (panelDerrota  != null) panelDerrota.SetActive(!victoria);

            // Enviar puntos al API solo si hay un GameControl activo con datos válidos
            if (ExInGameControl.Instance != null && puntos > 0)
                StartCoroutine(EnviarPuntaje(puntos));
        }

        // PUT /usuarios/<idUsuario>/puntos  — body: { "delta": <puntos> }
        private IEnumerator EnviarPuntaje(int puntos)
        {
            string url = $"{ExInGameControl.Instance.apiBaseUrl}/usuarios/{ExInGameControl.Instance.idUsuario}/puntos";

            string body = $"{{\"delta\":{puntos}}}";
            byte[] bodyBytes = Encoding.UTF8.GetBytes(body);

            using var req = new UnityWebRequest(url, "PUT");
            req.uploadHandler   = new UploadHandlerRaw(bodyBytes);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.certificateHandler = new SkipCertHandler();

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
                Debug.Log($"[Puntaje] +{puntos} puntos enviados al usuario {ExInGameControl.Instance.idUsuario}.");
            else
                Debug.LogWarning($"[Puntaje] Error al enviar puntos: {req.error}");
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

        public void StartToPlay()
        {
            if (ExInGameControl.Instance != null)
                Destroy(ExInGameControl.Instance.gameObject);
            SceneManager.LoadScene("ExInGameScene");
        }
    }
}

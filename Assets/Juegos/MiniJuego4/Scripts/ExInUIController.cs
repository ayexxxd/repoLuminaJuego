using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

namespace DefensoresDeSoftware
{
    [System.Serializable]
    public class PreguntaTrivia
    {
        public string pregunta;
        public string[] opciones = new string[4];
        public int indiceCorrecto;
    }

    public class ExInUIController : MonoBehaviour
    {
        [Header("Corazones Visuales")]
        public Image[] livesImages;

        [Header("Indicador de Extra (Opcional)")]
        public TextMeshProUGUI textoVidasExtra;

        [Header("Indicador de Nivel")]
        public TextMeshProUGUI textoNivel;

        [Header("Sistema de Trivia")]
        public GameObject panelTrivia;
        public TextMeshProUGUI textoPregunta;
        public Button[] botonesOpciones;
        public TextMeshProUGUI textoTokens;

        [Header("API Trivia")]
        [Tooltip("Ej: https://10.14.255.45:5010/juego/4/pregunta")]
        public string triviaApiUrl = "https://10.22.186.108:5001/juego/4/pregunta";

        // Fallback en caso de que la API no responda
        public PreguntaTrivia[] bancoPreguntas;

        [HideInInspector]
        public bool triviaFinalizada = false;

        [Header("Tienda de Mejoras")]
        public GameObject panelTienda;

        public Button botonCompraDano;
        public Button botonCompraVelocidad;
        public Button botonCompraVida;
        public Button botonContinuarTienda;

        // Pregunta pre-cargada en background para evitar el delay visible
        private PreguntaTrivia _preguntaPrecargada = null;

        void Start()
        {
            UpdateLives();

            if (panelTrivia != null) panelTrivia.SetActive(false);
            if (panelTienda != null) panelTienda.SetActive(false);

            UpdateTokens();
            ConfigurarBotonesTienda();

            // Pre-cargar desde el inicio para que la primera trivia sea instantánea
            if (!string.IsNullOrEmpty(triviaApiUrl))
                StartCoroutine(PrecargarPregunta());
        }

        public void UpdateLives()
        {
            int currentLives = PlayerPrefs.GetInt("Lives", 3);

            for (int i = 0; i < livesImages.Length; i++)
                livesImages[i].enabled = (i < currentLives);

            if (textoVidasExtra != null)
            {
                if (currentLives > livesImages.Length)
                {
                    textoVidasExtra.text    = "+ " + (currentLives - livesImages.Length);
                    textoVidasExtra.enabled = true;
                }
                else
                {
                    textoVidasExtra.enabled = false;
                }
            }
        }

        public void ActualizarNivel(int nivelActual)
        {
            if (textoNivel != null)
                textoNivel.text = "Nivel: " + nivelActual;
        }

        public void UpdateTokens()
        {
            if (textoTokens != null)
                textoTokens.text = PlayerPrefs.GetInt("WhirlpoolTokens", 0).ToString();

            RefrescarBotonesTienda();
        }

        // ─── TRIVIA ───────────────────────────────────────────────────────────

        public void MostrarTrivia()
        {
            triviaFinalizada = false;
            if (panelTrivia != null) panelTrivia.SetActive(true);
            Time.timeScale = 0f;

            if (_preguntaPrecargada != null)
            {
                // Pregunta lista: sin delay
                PoblarPanel(_preguntaPrecargada);
                _preguntaPrecargada = null;
            }
            else if (!string.IsNullOrEmpty(triviaApiUrl))
            {
                // No llegó a tiempo, cargar ahora (raro después de la primera vez)
                StartCoroutine(CargarPreguntaDesdeApi());
            }
            else
            {
                MostrarTriviaLocal();
            }
        }

        // Fetch en background — no bloquea, no pausa el juego
        private IEnumerator PrecargarPregunta()
        {
            _preguntaPrecargada = null;

            using var req = UnityWebRequest.Get(triviaApiUrl);
            req.SetRequestHeader("Accept", "application/json");
            req.certificateHandler = new SkipCertHandler();

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                var q = JsonUtility.FromJson<PreguntaTrivia>(req.downloadHandler.text);
                if (q != null && !string.IsNullOrEmpty(q.pregunta))
                    _preguntaPrecargada = q;
                else
                    Debug.LogWarning("[Trivia] Respuesta inválida al precargar.");
            }
            else
            {
                Debug.LogWarning($"[Trivia] Error al precargar: {req.error}");
            }
        }

        // Solo se usa si la pre-carga no llegó a tiempo
        private IEnumerator CargarPreguntaDesdeApi()
        {
            using var req = UnityWebRequest.Get(triviaApiUrl);
            req.SetRequestHeader("Accept", "application/json");
            req.certificateHandler = new SkipCertHandler();

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"[Trivia] Error API: {req.error} — usando banco local.");
                MostrarTriviaLocal();
                yield break;
            }

            var q = JsonUtility.FromJson<PreguntaTrivia>(req.downloadHandler.text);

            if (q == null || string.IsNullOrEmpty(q.pregunta))
            {
                Debug.LogWarning("[Trivia] Respuesta inválida — usando banco local.");
                MostrarTriviaLocal();
                yield break;
            }

            PoblarPanel(q);
        }

        private void MostrarTriviaLocal()
        {
            if (bancoPreguntas == null || bancoPreguntas.Length == 0)
            {
                Debug.LogWarning("[Trivia] Sin preguntas locales.");
                triviaFinalizada = true;
                if (panelTrivia != null) panelTrivia.SetActive(false);
                MostrarTienda();
                return;
            }

            PoblarPanel(bancoPreguntas[Random.Range(0, bancoPreguntas.Length)]);
        }

        private void PoblarPanel(PreguntaTrivia q)
        {
            if (textoPregunta != null)
                textoPregunta.text = q.pregunta;

            for (int i = 0; i < botonesOpciones.Length; i++)
            {
                int idx = i;
                var label = botonesOpciones[i].GetComponentInChildren<TextMeshProUGUI>();

                if (label != null && i < q.opciones.Length)
                    label.text = q.opciones[i];

                botonesOpciones[i].onClick.RemoveAllListeners();
                botonesOpciones[i].onClick.AddListener(() => RevisarRespuesta(idx == q.indiceCorrecto));
            }
        }

        // ─── RESPUESTA ────────────────────────────────────────────────────────

        void RevisarRespuesta(bool esCorrecto)
        {
            if (esCorrecto && ExInGameControl.Instance != null)
            {
                ExInGameControl.Instance.AddWhirlpoolToken();
                ExInGameControl.Instance.RegistrarRespuestaCorrecta();
            }

            if (panelTrivia != null) panelTrivia.SetActive(false);

            // Aprovechar el tiempo de la tienda para pre-cargar la siguiente pregunta
            if (!string.IsNullOrEmpty(triviaApiUrl))
                StartCoroutine(PrecargarPregunta());

            MostrarTienda();
        }

        // ─── CERT ─────────────────────────────────────────────────────────────

        private class SkipCertHandler : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData) => true;
        }

        // ─── TIENDA ───────────────────────────────────────────────────────────

        void ConfigurarBotonesTienda()
        {
            if (botonCompraDano != null)
            {
                botonCompraDano.onClick.RemoveAllListeners();
                botonCompraDano.onClick.AddListener(() => IntentarCompra(TipoMejora.Dano));
            }
            if (botonCompraVelocidad != null)
            {
                botonCompraVelocidad.onClick.RemoveAllListeners();
                botonCompraVelocidad.onClick.AddListener(() => IntentarCompra(TipoMejora.Velocidad));
            }
            if (botonCompraVida != null)
            {
                botonCompraVida.onClick.RemoveAllListeners();
                botonCompraVida.onClick.AddListener(() => IntentarCompra(TipoMejora.Vida));
            }

            if (botonContinuarTienda != null)
            {
                botonContinuarTienda.onClick.RemoveAllListeners();
                botonContinuarTienda.onClick.AddListener(CerrarTienda);
            }
        }

        void RefrescarBotonesTienda()
        {
            if (ExInGameControl.Instance == null) return;
            int tokens = PlayerPrefs.GetInt("WhirlpoolTokens", 0);
            SetBotonInteractable(botonCompraDano,      tokens >= ExInGameControl.Instance.costoDamage);
            SetBotonInteractable(botonCompraVelocidad, tokens >= ExInGameControl.Instance.costoVelocidad);
            SetBotonInteractable(botonCompraVida,      tokens >= ExInGameControl.Instance.costoVida);
        }

        void SetBotonInteractable(Button btn, bool estado)
        {
            if (btn != null) btn.interactable = estado;
        }

        void IntentarCompra(TipoMejora mejora)
        {
            if (ExInGameControl.Instance == null) return;
            ExInGameControl.Instance.ComprarMejora(mejora);
            UpdateTokens();
            RefrescarBotonesTienda();
        }

        public void MostrarTienda()
        {
            if (panelTienda != null)
            {
                panelTienda.SetActive(true);
                RefrescarBotonesTienda();
            }

            Time.timeScale = 0f;
        }

        public void CerrarTienda()
        {
            if (panelTienda != null) panelTienda.SetActive(false);
            Time.timeScale = 1f;
            triviaFinalizada = true;
        }
    }
}

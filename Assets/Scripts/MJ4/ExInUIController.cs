using UnityEngine;
using UnityEngine.UI; 
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

        // --- TRIVIA ---
        [Header("Sistema de Trivia")]
        public GameObject panelTrivia; 
        public TextMeshProUGUI textoPregunta;
        public Button[] botonesOpciones;
        public TextMeshProUGUI textoTokens; 
        public PreguntaTrivia[] bancoPreguntas;

        [HideInInspector]
        public bool triviaFinalizada = false;

        // ==========================================
        //        PANEL DE TIENDA DE MEJORAS
        // ==========================================
        [Header("Tienda de Mejoras")]
        public GameObject panelTienda;

        // Botones de compra
        public Button botonCompraDano;
        public Button botonCompraVelocidad;
        public Button botonCompraVida;
        public Button botonContinuarTienda;

        // ==========================================

        void Start()
        {
            UpdateLives();

            if (panelTrivia != null) panelTrivia.SetActive(false);
            if (panelTienda != null) panelTienda.SetActive(false);

            UpdateTokens();
            ConfigurarBotonesTienda();
        }

        // ----------------------------------------------------------------
        //  VIDAS Y NIVEL
        // ----------------------------------------------------------------

        public void UpdateLives()
        {
            int currentLives = PlayerPrefs.GetInt("Lives", 3);

            for (int i = 0; i < livesImages.Length; i++)
            {
                livesImages[i].enabled = (i < currentLives);
            }

            if (textoVidasExtra != null) 
            {
                if (currentLives > livesImages.Length)
                {
                    textoVidasExtra.text = "+ " + (currentLives - livesImages.Length);
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

        // ----------------------------------------------------------------
        //  TOKENS
        // ----------------------------------------------------------------

        public void UpdateTokens() 
        {
            if (textoTokens != null)
                textoTokens.text = PlayerPrefs.GetInt("WhirlpoolTokens", 0).ToString();

            // Refrescamos los colores de los botones según si el jugador puede comprar
            RefrescarBotonesTienda();
        }

        // ----------------------------------------------------------------
        //  TRIVIA
        // ----------------------------------------------------------------

        public void MostrarTrivia() 
        {
            if (bancoPreguntas == null || bancoPreguntas.Length == 0)
            {
                Debug.LogWarning("No hay preguntas configuradas en el Inspector.");
                triviaFinalizada = true;
                // Si no hay trivia, mostramos la tienda directamente
                MostrarTienda();
                return;
            }

            triviaFinalizada = false;
            if (panelTrivia != null) panelTrivia.SetActive(true);
            Time.timeScale = 0f; 

            PreguntaTrivia q = bancoPreguntas[Random.Range(0, bancoPreguntas.Length)];
            if (textoPregunta != null) textoPregunta.text = q.pregunta;

            for (int i = 0; i < botonesOpciones.Length; i++) 
            {
                int index = i;
                TextMeshProUGUI textoBoton = botonesOpciones[i].GetComponentInChildren<TextMeshProUGUI>();
                if (textoBoton != null) textoBoton.text = q.opciones[i];
                botonesOpciones[i].onClick.RemoveAllListeners();
                botonesOpciones[i].onClick.AddListener(() => RevisarRespuesta(index == q.indiceCorrecto));
            }
        }

        void RevisarRespuesta(bool esCorrecto) 
        {
            if (esCorrecto && ExInGameControl.Instance != null)
            {
                ExInGameControl.Instance.AddWhirlpoolToken();
            }
            
            if (panelTrivia != null) panelTrivia.SetActive(false);

            // Después de la trivia → abrimos la Tienda antes de continuar
            MostrarTienda();
        }

        // ----------------------------------------------------------------
        //  TIENDA DE MEJORAS
        // ----------------------------------------------------------------

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
            // 4to botón: Continuar (siempre interactable)
            if (botonContinuarTienda != null)
            {
                botonContinuarTienda.onClick.RemoveAllListeners();
                botonContinuarTienda.onClick.AddListener(CerrarTienda);
            }

            ActualizarTextosCostos();
        }

        void ActualizarTextosCostos() { } // textos manejados manualmente en la escena

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

        IEnumerator MostrarFeedback(string mensaje) { yield break; } // eliminado

        public void MostrarTienda()
        {
            if (panelTienda != null)
            {
                panelTienda.SetActive(true);
                RefrescarBotonesTienda();
                ActualizarTextosCostos();
            }
            // El juego queda pausado (timeScale = 0 viene de la Trivia, o lo pausamos aquí)
            Time.timeScale = 0f;
        }

        /// <summary>
        /// Llamado por el botón "Continuar" del panel de Tienda.
        /// </summary>
        public void CerrarTienda()
        {
            if (panelTienda != null) panelTienda.SetActive(false);
            Time.timeScale = 1f;
            triviaFinalizada = true; // Ahora sí le avisamos al Spawner que puede seguir
        }

        // ----------------------------------------------------------------
        //  SALTAR OLEADA
        // ----------------------------------------------------------------

        void ConfigurarBotonSaltarOleada() { } // eliminado, ya no se usa
    }
}
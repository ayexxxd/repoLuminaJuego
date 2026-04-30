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

        [Header("Sistema de Trivia")]
        public GameObject panelTrivia; 
        public TextMeshProUGUI textoPregunta;
        public Button[] botonesOpciones;
        public TextMeshProUGUI textoTokens; 
        public PreguntaTrivia[] bancoPreguntas;

        [HideInInspector]
        public bool triviaFinalizada = false;

        [Header("Tienda de Mejoras")]
        public GameObject panelTienda;

        public Button botonCompraDano;
        public Button botonCompraVelocidad;
        public Button botonCompraVida;
        public Button botonContinuarTienda;

        void Start()
        {
            UpdateLives();

            if (panelTrivia != null) panelTrivia.SetActive(false);
            if (panelTienda != null) panelTienda.SetActive(false);

            UpdateTokens();
            ConfigurarBotonesTienda();
        }

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

        public void UpdateTokens() 
        {
            if (textoTokens != null)
                textoTokens.text = PlayerPrefs.GetInt("WhirlpoolTokens", 0).ToString();

            RefrescarBotonesTienda();
        }

        public void MostrarTrivia() 
        {
            if (bancoPreguntas == null || bancoPreguntas.Length == 0)
            {
                Debug.LogWarning("No hay preguntas configuradas en el Inspector.");
                triviaFinalizada = true;

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

            MostrarTienda();
        }

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

        void ConfigurarBotonSaltarOleada() { }
    }
}
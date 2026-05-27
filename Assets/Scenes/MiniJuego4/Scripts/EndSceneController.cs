using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace DefensoresDeSoftware
{
    public class EndSceneController : MonoBehaviour
    {
        [Header("Resultados")]
        public TextMeshProUGUI textoNivelAlcanzado;
        public TextMeshProUGUI textoMonedasObtenidas;

        [Header("Paneles")]
        public GameObject panelVictoria;
        public GameObject panelDerrota;

        void Start()
        {
            int oleadas = PlayerPrefs.GetInt("OleadasCompletadas", 0);
            int monedas = PlayerPrefs.GetInt("PreguntasCorrectas", 0);
            int vidas   = PlayerPrefs.GetInt("Lives", 0);

            if (textoNivelAlcanzado != null)
                textoNivelAlcanzado.text = "Nivel más alto: " + oleadas;

            if (textoMonedasObtenidas != null)
                textoMonedasObtenidas.text = "Monedas obtenidas: " + monedas;

            bool victoria = vidas > 0;
            if (panelVictoria != null) panelVictoria.SetActive(victoria);
            if (panelDerrota  != null) panelDerrota.SetActive(!victoria);
        }
        public void ExitGame()
        {
            SceneManager.LoadScene("MenuScene");
            //UnityEditor.EditorApplication.isPlaying = false;
            //Application.Quit();//funcion del boton para salir en app
        }
        public void StartToPlay()
        {//funcion del boton de jugar
            SceneManager.LoadScene("ExInGameScene");
        }
    }
}

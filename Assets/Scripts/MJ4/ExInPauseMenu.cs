using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefensoresDeSoftware
{
    public class ExInPauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;
        // isPaused ya no es tan necesaria si usas botones distintos para pausar y despausar, 
        // pero la podemos dejar para mantener el control.
        private bool isPaused = false; 

        private void Start()
        {
            pausePanel.SetActive(false);
        }

        // ¡Adiós a la función Update que escuchaba la tecla P!

        public void Pause()
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f; 
            isPaused = true;
        }

        public void Resume()
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f; 
            isPaused = false;
        }

        public void Leave()
        {
            Time.timeScale = 1f; 
            SceneManager.LoadScene("ExInInicio"); 
        }
    }
}
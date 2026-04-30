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
            // 1. Descongelamos el motor físico
            Time.timeScale = 1f; 

            // 2. Reiniciamos las vidas en el registro usando el valor por defecto de tu GameControl
            if (ExInGameControl.Instance != null)
            {
                // 3. Destruimos el GameControl para que al volver a jugar nazca uno nuevo y limpio
                Destroy(ExInGameControl.Instance.gameObject);
                ExInGameControl.Instance = null; 
            }

            // 4. Cargamos el menú principal
            SceneManager.LoadScene("ExInInicio"); 
        }
    }
}
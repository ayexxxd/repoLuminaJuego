using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefensoresDeSoftware
{
    public class ExInPauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;

        private bool isPaused = false; 

        private void Start()
        {
            pausePanel.SetActive(false);
        }

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

            PlayerPrefs.DeleteKey("WhirlpoolTokens");
            PlayerPrefs.Save();

            if (ExInGameControl.Instance != null)
                Destroy(ExInGameControl.Instance.gameObject);
            if (ExInSFXManager.Instance != null)
                Destroy(ExInSFXManager.Instance.gameObject);
            SceneManager.LoadScene("ExInInicio"); 
        }
    }
}
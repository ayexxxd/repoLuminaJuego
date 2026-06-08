using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private bool paused = false;

    private void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }

    public void Pause()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
        Time.timeScale = 0f;
        paused = true;
    }

    public void Resume()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        Time.timeScale = 1f;
        paused = false;
    }
}
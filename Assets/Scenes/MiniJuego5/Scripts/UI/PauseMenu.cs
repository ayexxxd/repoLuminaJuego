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

    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("StartScene");
        }
    }

    private void LateUpdate()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (paused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        EnsurePausePanelExists();
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
        Time.timeScale = 0f;
        paused = true;
    }

    public void Resume()
    {
        EnsurePausePanelExists();
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        Time.timeScale = 1f;
        paused = false;
    }

    private void EnsurePausePanelExists()
    {
        if (pausePanel == null)
        {
            pausePanel = GameObject.Find("PausePanel");
            if (pausePanel == null)
            {
                pausePanel = FindAnyObjectByType<Canvas>()?.gameObject.transform.Find("PausePanel")?.gameObject;
            }
        }
    }
}
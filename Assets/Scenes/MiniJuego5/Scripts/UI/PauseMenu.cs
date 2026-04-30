using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    SpriteRenderer player;
    private bool isPaused = false;

    private void Start()
    {
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (isPaused)
            Resume();
        else
            Pause();
        }
    }
    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        //hide player in pause
        if(player != null){
            player.gameObject.SetActive(false);
        }
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        //reveal player when resuming
        if(player != null){
            player.gameObject.SetActive(true);
        }
    }

    public void Leave()
    {
        SceneManager.LoadScene("StartScene");
    }
}

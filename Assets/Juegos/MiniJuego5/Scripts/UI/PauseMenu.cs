using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private AudioClip clickSFX;

    private void Start()
    {
        if (clickSFX != null){
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(clickSFX);
        }
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        if (clickSFX != null){
             AudioSource audioSource = GetComponent<AudioSource>();
             audioSource.PlayOneShot(clickSFX);
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }

    public void Pause()
    {
        if (clickSFX != null){
             AudioSource audioSource = GetComponent<AudioSource>();
             audioSource.PlayOneShot(clickSFX);
        }
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        if (clickSFX != null){
             AudioSource audioSource = GetComponent<AudioSource>();
             audioSource.PlayOneShot(clickSFX);
        }
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        Time.timeScale = 1f;
    }
}
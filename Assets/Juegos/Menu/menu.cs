using UnityEngine;
using UnityEngine.SceneManagement;


public class menu : MonoBehaviour
{
    [SerializeField] private AudioClip clickSFX;
    public void OpenMiniJuego5Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        clickSFX = Resources.Load<AudioClip>("ClickSFX");
        SceneManager.LoadScene("StartScene");
    }
    public void OpenMiniJuego4Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        clickSFX = Resources.Load<AudioClip>("ClickSFX");
        SceneManager.LoadScene("ExInGameScene");
    }

    public void OpenMiniJuego1Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        clickSFX = Resources.Load<AudioClip>("ClickSFX");
        SceneManager.LoadScene("MenuMJ1Escena");
    }

    public void OpenMiniJuego2Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        clickSFX = Resources.Load<AudioClip>("ClickSFX");
        SceneManager.LoadScene("EMI");
    }

    public void OpenMiniJuego3Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        clickSFX = Resources.Load<AudioClip>("ClickSFX");
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void Start()
    {
        clickSFX = Resources.Load<AudioClip>("ClickSFX");
        AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            DontDestroyOnLoad(gameObject);
    }

    public void ExitGame()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        clickSFX = Resources.Load<AudioClip>("ClickSFX");
        audioSource.PlayOneShot(clickSFX);
        PlayerPrefs.DeleteKey("userid");
        Application.Quit();
    }
}

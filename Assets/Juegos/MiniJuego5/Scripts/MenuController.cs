using UnityEngine;
using UnityEngine.SceneManagement;
//namespace TopDown.Shooting{//namespace to organize code and avoid naming conflicts

public class MenuController : MonoBehaviour
{
    [SerializeField] private AudioClip clickSFX;
    [SerializeField] private GameObject instructionsPanel;

    public void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(clickSFX);
        instructionsPanel.SetActive(false);
    }
    public void StartToPlay()
    {//funcion del boton de jugar
        SceneManager.LoadScene("ShooterScene");
    }
    public void ExitGame()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(clickSFX);
        SceneManager.LoadScene("MenuScene");
        //UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();//funcion del boton para salir en app
    }
    public void OpenInventory()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(clickSFX);
        instructionsPanel.SetActive(instructionsPanel.activeSelf);//funcion del boton de instrucciones  
    }
}
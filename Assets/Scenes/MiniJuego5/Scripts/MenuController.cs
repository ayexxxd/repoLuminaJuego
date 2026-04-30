using UnityEngine;
using UnityEngine.SceneManagement;
//namespace TopDown.Shooting{//namespace to organize code and avoid naming conflicts

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject instructionsPanel;

    public void Start()
    {
        instructionsPanel.SetActive(false);
    }
    public void StartToPlay()
    {//funcion del boton de jugar
        SceneManager.LoadScene("ShooterScene");
    }
    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();//funcion del boton para salir en app
    }
    public void OpenInstructions()
    {
        instructionsPanel.SetActive(!instructionsPanel.activeSelf);//funcion del boton de instrucciones  
    }
}
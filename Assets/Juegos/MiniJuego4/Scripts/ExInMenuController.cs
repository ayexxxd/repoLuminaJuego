using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
//namespace TopDown.Shooting{//namespace to organize code and avoid naming conflicts

public class ExInMenuController : MonoBehaviour
{
    [SerializeField] private GameObject instructionsPanel;

    public void Start()
    {
        instructionsPanel.SetActive(false);
    }
    public void StartToPlay()
    {//funcion del boton de jugar
        SceneManager.LoadScene("ExInGameScene");
    }
    public void ExitGame()
    {
        SceneManager.LoadScene("MenuScene");
        //UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();//funcion del boton para salir en app
    }
    public void OpenInstructions()
    {
        instructionsPanel.SetActive(true);//funcion del boton de instrucciones
        StartCoroutine(ClickToContinue());  
    }
    IEnumerator ClickToContinue()
    {
        yield return null; 

        // Pausamos la ejecución de esta función HASTA que el jugador presione 
        // cualquier tecla o el botón izquierdo del mouse
        yield return new WaitUntil(() => Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame);

        // Una vez que hace clic o presiona una tecla, apagamos el panel
        instructionsPanel.SetActive(false);
    }
}

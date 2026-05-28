using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{

    // Button function: opens MiniJuego5 start scene
    public void OpenMiniJuego5Start()
    {
        SceneManager.LoadScene("StartScene");
    }

    // Button function: opens MiniJuego4 start scene
    public void OpenMiniJuego4Start()
    {
        SceneManager.LoadScene("ExInGameScene");
    }

    public void OpenMiniJuego1Start()
    {
        SceneManager.LoadScene("MenuMJ1Escena");
    }

    public void OpenMiniJuego2Start()
    {
        SceneManager.LoadScene("EMI");
    }

    public void OpenMiniJuego3Start()
    {
        SceneManager.LoadScene("SceneCar");
    }
}

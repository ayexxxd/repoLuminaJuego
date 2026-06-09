using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public void OpenMiniJuego5Start()
    {
        SceneManager.LoadScene("StartScene");
    }
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
        SceneManager.LoadScene("MenuPrincipal");
    }
}

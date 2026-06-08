using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMJ1Controller : MonoBehaviour
{
    public void IrAJuego()
    {
        SceneManager.LoadScene("EscenadeJuego");
    }

    public void IrAMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void Salir()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}

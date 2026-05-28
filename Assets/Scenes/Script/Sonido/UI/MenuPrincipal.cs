using UnityEngine;
using UnityEngine.SceneManagement;

// Controla toda la lógica del menú principal
// Este script va en un GameObject vacío llamado "MenuManager"
public class MenuPrincipal : MonoBehaviour
{
    [Header("Nombres exactos de las escenas")]
    // Deben coincidir EXACTAMENTE con los nombres de los archivos de escena
    public string nombreEscenaJuego = "Juego";

    void Start()
    {
        // Nos aseguramos de que el juego corre a velocidad normal
        // (puede quedar en 0 si hubo una pausa antes)
        Time.timeScale = 1f;

        // Mostramos en consola los tokens acumulados del jugador
        int tokens = PlayerPrefs.GetInt("TokensAcumulados", 0);
        Debug.Log("Tokens acumulados del jugador: " + tokens);
    }

    // ---- Se conecta al botón JUGAR desde el Inspector ----
    public void BotonJugar()
    {
        Debug.Log("Iniciando juego...");
        SceneManager.LoadScene("SceneCarro"); // Ponemos el nombre exacto aquí
    }

    public void BotonMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    // ---- Se conecta al botón SALIR desde el Inspector ----
    public void BotonSalir()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
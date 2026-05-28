using UnityEngine;
using UnityEngine.SceneManagement;
public class Pausaa : MonoBehaviour
{

    [Header("Asigna el panel de pausa aquí")]
    public GameObject panelPausa;

    // Variable para saber si el juego está pausado o no
    private bool juegoPausado = false;

    void Update()
    {
        // Detecta si presionamos la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Pausar()
    {
        juegoPausado = true;
        Time.timeScale = 0f; // Esto congela TODAS las físicas y movimientos del juego
        panelPausa.SetActive(true); // Muestra el panel
    }

    public void Reanudar()
    {
        juegoPausado = false;
        Time.timeScale = 1f; // Esto hace que el tiempo vuelva a la normalidad
        panelPausa.SetActive(false); // Oculta el panel
    }

    public void SalirJuego()
    {
        SceneManager.LoadScene("MenuPrincipal"); // Cambia "MenuPrincipal" por el nombre de tu escena de menú
    }
}

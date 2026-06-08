using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;   // ← nuevo

public class Pausaa : MonoBehaviour
{
    [Header("Asigna el panel de pausa aquí")]
    public GameObject panelPausa;

    private bool juegoPausado = false;

    void Update()
    {
        // Keyboard.current reemplaza a Input.GetKeyDown
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (juegoPausado)
                Reanudar();
            else
                Pausar();
        }
    }

    public void Pausar()
    {
        juegoPausado = true;
        Time.timeScale = 0f;
        panelPausa.SetActive(true);
    }

    public void Reanudar()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
        panelPausa.SetActive(false);
    }

    public void SalirJuego()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}
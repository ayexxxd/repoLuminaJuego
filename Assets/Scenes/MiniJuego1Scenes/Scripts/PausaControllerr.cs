using UnityEngine;
using UnityEngine.InputSystem;

public class PausaControllerr : MonoBehaviour
{
    public GameObject panelPausa; 
    
    private bool juegoPausado = false;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (juegoPausado)
            {
                RegresarAlJuego();
            }
            else
            {
                ActivarPausa();
            }
        }
    }

    public void ActivarPausa()
    {
        juegoPausado = true;
        panelPausa.SetActive(true); 
        Time.timeScale = 0f;       
    }


    public void RegresarAlJuego()
    {
        juegoPausado = false;
        panelPausa.SetActive(false); 
        Time.timeScale = 1f;         
    }


    public void SalirDelJuego()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}

using UnityEngine;

public class PausaControllerr : MonoBehaviour
{
    // Arrastra tu Panel de Pausa aquí desde el Inspector
    public GameObject panelPausa; 
    
    private bool juegoPausado = false;

    void Update()
    {
        // El jugador también puede pausar/despausar presionando la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
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

    // Se activa al presionar el botón de Pausa en la pantalla
    public void ActivarPausa()
    {
        juegoPausado = true;
        panelPausa.SetActive(true); // Muestra el menú flotante
        Time.timeScale = 0f;        // Congela el juego por completo
    }

    // BOTÓN 1: Regresar al juego (Reanudar)
    public void RegresarAlJuego()
    {
        juegoPausado = false;
        panelPausa.SetActive(false); // Esconde el menú
        Time.timeScale = 1f;         // El juego vuelve a la normalidad
    }

    // BOTÓN 2: Salir (Al menú principal o cerrar el juego)
    public void SalirDelJuego()
    {
        // ¡Importante! Descongelamos el tiempo antes de irnos, 
        // si no, el menú principal podría quedarse congelado.
        Time.timeScale = 1f; 

        // OPCIÓN A: Si tienes una escena de Menú Principal, cárgala aquí (Descomenta la línea de abajo)
        // SceneManager.LoadScene("NombreDeTuEscenaMenu");

        // OPCIÓN B: Salir por completo del juego (Editor y Juego final)
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

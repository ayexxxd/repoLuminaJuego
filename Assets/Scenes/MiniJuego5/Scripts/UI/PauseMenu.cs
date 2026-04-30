using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;//referncia al panel
    //SpriteRenderer player;//referencia al sprite del jugador para ocultarlo en pausa
    private bool paused = false;

    private void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (paused)
                Resume();
            else
            Pause();
        }

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("StartScene");//loadea menu
        }
    }
    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;//pausar el tiempo
        paused = true;
    }

    public void Resume()
    {//quitar panel de pausa
        pausePanel.SetActive(false);
        Time.timeScale = 1f;//resumir el tiempo
        paused = false;
    }

    public void Leave()
    {//funcion del boton de salir al menu
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("StartScene");
        }
    }
}

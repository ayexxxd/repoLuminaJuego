using UnityEngine;
using UnityEngine.Events;

public class VidasManager : MonoBehaviour
{
    [Header("Configuración")]
    public int vidasIniciales = 3;
    public float tiempoInvencibilidad = 2f;

    [Header("Estado actual")]
    public int vidasActuales;

    private bool esInvencible = false;

    private CarrerasGameManager gameManager;
    private UIManager uiManager;

    public UnityEvent onSinVidas;
    public UnityEvent<int> onVidaCambiada;

    void Start()
    {
        vidasActuales = vidasIniciales;

        gameManager = FindObjectOfType<CarrerasGameManager>();
        uiManager = FindObjectOfType<UIManager>();

        if (gameManager == null)
        {
            Debug.LogError("VidasManager: NO se encontró el GameManager en la escena.");
        }
        else
        {
            Debug.Log("VidasManager: GameManager encontrado correctamente.");
        }

        ActualizarUIVidas();
    }

    public void QuitarVida()
    {
        if (esInvencible)
        {
            Debug.Log("Daño ignorado — jugador invencible.");
            return;
        }


        if (gameManager != null &&
            gameManager.estadoActual != CarrerasGameManager.EstadoJuego.Jugando)
        {
            Debug.Log("Daño ignorado — juego ya terminó.");
            return;
        }

        vidasActuales--;
        vidasActuales = Mathf.Max(vidasActuales, 0);

        Debug.Log("¡Vida perdida! Vidas restantes: " + vidasActuales);

        onVidaCambiada?.Invoke(vidasActuales);


        ActualizarUIVidas();


        if (vidasActuales <= 0)
        {
            Debug.Log("¡SIN VIDAS! Llamando al GameManager...");


            onSinVidas?.Invoke();


            if (gameManager != null)
            {
                gameManager.JugadorPerdioSinVidas();
            }
            else
            {
                Debug.LogError("VidasManager: GameManager es null.");
            }

            return;
        }


        StartCoroutine(CorrutinaInvencibilidad());
    }


    public void AgregarVida()
    {
        if (vidasActuales < vidasIniciales)
        {
            vidasActuales++;
            Debug.Log("¡Vida extra! Vidas: " + vidasActuales);
            onVidaCambiada?.Invoke(vidasActuales);
            ActualizarUIVidas();
        }
        else
        {
            Debug.Log("Ya tienes el máximo de vidas.");
        }
    }

    System.Collections.IEnumerator CorrutinaInvencibilidad()
    {
        esInvencible = true;
        Debug.Log("Invencibilidad activada por " + tiempoInvencibilidad + "s");

        SpriteRenderer sr = null;
        MovimientoNave nave = FindObjectOfType<MovimientoNave>();
        if (nave != null)
            sr = nave.GetComponent<SpriteRenderer>();

        float transcurrido = 0f;
        float intervalo = 0.15f;

        while (transcurrido < tiempoInvencibilidad)
        {
            if (sr != null) sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(intervalo);
            transcurrido += intervalo;
        }


        if (sr != null) sr.enabled = true;

        esInvencible = false;
        Debug.Log("Invencibilidad terminada.");
    }


    void ActualizarUIVidas()
    {

        if (uiManager != null)
            uiManager.ActualizarVidas(vidasActuales);


        if (VidasUI.instancia != null)
            VidasUI.instancia.ActualizarVidas(vidasActuales);
    }


    public bool EsInvencible()
    {
        return esInvencible;
    }
}
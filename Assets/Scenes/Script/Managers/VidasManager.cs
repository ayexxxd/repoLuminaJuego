using UnityEngine;
using UnityEngine.Events;

// VidasManager controla las vidas del jugador
// Cuando llega a 0 llama directamente al GameManager
// Este script va en el GameObject "VidasManager" en la escena Juego
public class VidasManager : MonoBehaviour
{
    [Header("Configuración")]
    public int vidasIniciales = 3;
    public float tiempoInvencibilidad = 2f;

    [Header("Estado actual")]
    public int vidasActuales;

    // ---- Variables privadas ----
    private bool esInvencible = false;

    // Referencias directas — más confiables que eventos
    private GameManager gameManager;
    private UIManager uiManager;

    // Evento opcional por si otros scripts quieren escuchar
    public UnityEvent onSinVidas;
    public UnityEvent<int> onVidaCambiada;

    void Start()
    {
        // Inicializamos las vidas
        vidasActuales = vidasIniciales;

        // Buscamos referencias directamente
        gameManager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();

        if (gameManager == null)
        {
            Debug.LogError("VidasManager: NO se encontró el GameManager en la escena.");
        }
        else
        {
            Debug.Log("VidasManager: GameManager encontrado correctamente.");
        }

        // Actualizamos la UI con las vidas iniciales
        ActualizarUIVidas();
    }

    // ---- Quita una vida al jugador ----
    public void QuitarVida()
    {
        // Si es invencible ignoramos el daño
        if (esInvencible)
        {
            Debug.Log("Daño ignorado — jugador invencible.");
            return;
        }

        // Si el juego ya terminó no hacemos nada
        if (gameManager != null &&
            gameManager.estadoActual != GameManager.EstadoJuego.Jugando)
        {
            Debug.Log("Daño ignorado — juego ya terminó.");
            return;
        }

        // Quitamos una vida
        vidasActuales--;
        vidasActuales = Mathf.Max(vidasActuales, 0);

        Debug.Log("¡Vida perdida! Vidas restantes: " + vidasActuales);

        // Avisamos a quien escuche el evento
        onVidaCambiada?.Invoke(vidasActuales);

        // Actualizamos la UI
        ActualizarUIVidas();

        // Verificamos si se quedó sin vidas
        if (vidasActuales <= 0)
        {
            Debug.Log("¡SIN VIDAS! Llamando al GameManager...");

            // Disparamos el evento opcional
            onSinVidas?.Invoke();

            // Llamamos DIRECTAMENTE al GameManager
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

        // Si aún tiene vidas, activamos invencibilidad
        StartCoroutine(CorrutinaInvencibilidad());
    }

    // ---- Agrega una vida ----
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

    // ---- Corrutina de invencibilidad con parpadeo ----
    System.Collections.IEnumerator CorrutinaInvencibilidad()
    {
        esInvencible = true;
        Debug.Log("Invencibilidad activada por " + tiempoInvencibilidad + "s");

        // Buscamos el SpriteRenderer de la nave para el parpadeo
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

        // Aseguramos que la nave quede visible
        if (sr != null) sr.enabled = true;

        esInvencible = false;
        Debug.Log("Invencibilidad terminada.");
    }

    // ---- Actualiza la UI de vidas ----
    void ActualizarUIVidas()
    {
        // Actualizamos el texto si existe
        if (uiManager != null)
            uiManager.ActualizarVidas(vidasActuales);

        // Actualizamos los sprites de corazón si existe el VidasUI
        if (VidasUI.instancia != null)
            VidasUI.instancia.ActualizarVidas(vidasActuales);
    }

    // ---- Devuelve si el jugador es invencible ----
    public bool EsInvencible()
    {
        return esInvencible;
    }
}
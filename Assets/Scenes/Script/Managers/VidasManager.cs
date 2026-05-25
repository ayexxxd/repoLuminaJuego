using UnityEngine;
using UnityEngine.Events;

// VidasManager controla las vidas del jugador
// Sabe cuántas vidas tiene, cuándo pierde una y cuándo llega a cero
// Vive en un GameObject vacío en la escena
public class VidasManager : MonoBehaviour
{
    [Header("Configuración")]
    // Cuántas vidas empieza el jugador
    public int vidasIniciales = 3;

    // Segundos de invencibilidad después de recibir daño
    // Durante este tiempo el jugador no puede recibir más daño
    public float tiempoInvencibilidad = 2f;

    [Header("Estado actual (solo lectura)")]
    public int vidasActuales;

    // ---- Variables privadas ----

    // Controla si el jugador puede recibir daño ahora mismo
    private bool esInvencible = false;

    // Referencias a otros managers
    private UIManager uiManager;
    private GameManager gameManager;

    // Evento que se dispara cuando el jugador pierde todas las vidas
    public UnityEvent onSinVidas;

    // Evento que se dispara cada vez que cambia la cantidad de vidas
    // El int es la cantidad actual de vidas
    public UnityEvent<int> onVidaCambiada;

    void Start()
    {
        // Inicializamos las vidas
        vidasActuales = vidasIniciales;

        // Buscamos los managers que necesitamos
        uiManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();

        // Actualizamos la UI con las vidas iniciales
        ActualizarUIVidas();
    }

    // ---- Quita una vida al jugador ----
    // Llamado por los obstáculos cuando colisionan con la nave
    public void QuitarVida()
    {
        // Si el jugador es invencible, ignoramos el daño
        if (esInvencible)
        {
            Debug.Log("Daño ignorado - jugador invencible.");
            return;
        }

        // Si el juego ya terminó no hacemos nada
        if (gameManager != null && gameManager.estadoActual != GameManager.EstadoJuego.Jugando)
        {
            return;
        }

        // Quitamos una vida
        vidasActuales--;
        Debug.Log("¡Vida perdida! Vidas restantes: " + vidasActuales);

        // Avisamos que cambió la cantidad de vidas
        onVidaCambiada?.Invoke(vidasActuales);

        // Actualizamos la UI
        ActualizarUIVidas();

        // Verificamos si se quedó sin vidas
        if (vidasActuales <= 0)
        {
            vidasActuales = 0;
            Debug.Log("¡Sin vidas! El jugador perdió.");
            onSinVidas?.Invoke();
            return;
        }

        // Si aún tiene vidas, activamos la invencibilidad temporal
        StartCoroutine(CorrutinaInvencibilidad());
    }

    // ---- Da una vida extra al jugador ----
    // Llamado por los power-ups de vida extra
    public void AgregarVida()
    {
        // No superamos el máximo de vidas iniciales
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

    // ---- Corrutina que maneja el tiempo de invencibilidad ----
    System.Collections.IEnumerator CorrutinaInvencibilidad()
    {
        // Activamos la invencibilidad
        esInvencible = true;
        Debug.Log("Invencibilidad activada por " + tiempoInvencibilidad + " segundos.");

        // Efecto visual: hacemos parpadear la nave
        // Buscamos el SpriteRenderer de la nave para el parpadeo
        MovimientoNave nave = FindObjectOfType<MovimientoNave>();
        SpriteRenderer spriteNave = null;

        if (nave != null)
        {
            spriteNave = nave.GetComponent<SpriteRenderer>();
        }

        // Tiempo transcurrido desde que empezó la invencibilidad
        float tiempoTranscurrido = 0f;

        // Intervalo de parpadeo en segundos
        float intervaloParpado = 0.15f;

        // Hacemos parpadear la nave mientras dura la invencibilidad
        while (tiempoTranscurrido < tiempoInvencibilidad)
        {
            // Alternamos entre visible e invisible
            if (spriteNave != null)
            {
                spriteNave.enabled = !spriteNave.enabled;
            }

            // Esperamos el intervalo de parpadeo
            yield return new WaitForSeconds(intervaloParpado);
            tiempoTranscurrido += intervaloParpado;
        }

        // Nos aseguramos de que la nave quede visible al terminar
        if (spriteNave != null)
        {
            spriteNave.enabled = true;
        }

        // Desactivamos la invencibilidad
        esInvencible = false;
        Debug.Log("Invencibilidad terminada.");
    }

    // ---- Actualiza el texto de vidas en la UI ----
    void ActualizarUIVidas()
    {
        if (uiManager != null)
        {
            uiManager.ActualizarVidas(vidasActuales);
        }
    }

    // ---- Devuelve si el jugador es invencible ahora mismo ----
    // Útil para que otros scripts verifiquen esto
    public bool EsInvencible()
    {
        return esInvencible;
    }
}
using UnityEngine;
using UnityEngine.Events;

// TimerManager controla el contador regresivo del juego
// Vive en un GameObject vacío en la escena
public class TimerManager : MonoBehaviour
{
    [Header("Configuración del Timer")]
    // Tiempo total de la carrera en segundos
    public float tiempoTotal = 60f;

    [Header("Estado actual (solo lectura)")]
    // Tiempo que queda actualmente — lo hacemos público para que
    // otros scripts puedan leerlo (por ejemplo para guardar el mejor tiempo)
    public float tiempoRestante;

    // ---- Variables privadas ----

    // Controla si el timer está corriendo o pausado
    private bool timerActivo = false;

    // Para no ejecutar la lógica de derrota más de una vez
    private bool juegoTerminado = false;

    // Referencia al UIManager para actualizar la pantalla
    private UIManager uiManager;

    // Evento que se dispara cuando el tiempo se acaba
    // Otros scripts lo escuchan para reaccionar (GameManager, por ejemplo)
    public UnityEvent onTiempoAgotado;

    // Evento que se dispara cuando el jugador gana antes de que acabe el tiempo
    // Lo usamos para detener el timer
    public UnityEvent onJuegoCompletado;

    void Start()
    {
        // Inicializamos el tiempo restante con el tiempo total configurado
        tiempoRestante = tiempoTotal;

        // Buscamos el UIManager para actualizar la pantalla
        uiManager = Object.FindAnyObjectByType<UIManager>();

        // Actualizamos la UI con el tiempo inicial
        if (uiManager != null)
        {
            uiManager.ActualizarTimer(tiempoRestante);
        }

        // Iniciamos el timer automáticamente al empezar la escena
        IniciarTimer();
    }

    void Update()
    {
        // Solo contamos si el timer está activo y el juego no terminó
        if (!timerActivo || juegoTerminado) return;

        // Reducimos el tiempo restante frame por frame
        // Time.deltaTime es el tiempo del último frame en segundos
        tiempoRestante -= Time.deltaTime;

        // Actualizamos la UI con el nuevo tiempo
        // Hacemos esto en cada frame para que el número sea fluido
        if (uiManager != null)
        {
            uiManager.ActualizarTimer(tiempoRestante);
        }

        // Verificamos si el tiempo se agotó
        if (tiempoRestante <= 0)
        {
            // Nos aseguramos de que no baje de cero visualmente
            tiempoRestante = 0;

            // Detenemos el timer
            timerActivo = false;
            juegoTerminado = true;

            Debug.Log("¡Tiempo agotado! El jugador perdió.");

            // Disparamos el evento para que el GameManager reaccione
            onTiempoAgotado?.Invoke();
        }
    }

    // ---- Inicia el contador ----
    // Llamado automáticamente en Start, o manualmente si necesitas
    // iniciar el timer en un momento específico
    public void IniciarTimer()
    {
        timerActivo = true;
        Debug.Log("Timer iniciado: " + tiempoTotal + " segundos.");
    }

    // ---- Pausa el contador ----
    // Útil para pantallas de pausa o cutscenes
    public void PausarTimer()
    {
        timerActivo = false;
        Debug.Log("Timer pausado en: " + tiempoRestante + " segundos.");
    }

    // ---- Reanuda el contador ----
    public void ReanudarTimer()
    {
        timerActivo = true;
        Debug.Log("Timer reanudado.");
    }

    // ---- Detiene el timer cuando el jugador gana ----
    // Llamado por el LapManager cuando se completan todas las vueltas
    public void DetenerTimer()
    {
        timerActivo = false;
        juegoTerminado = true;
        Debug.Log("Timer detenido. Tiempo final: " + tiempoRestante + " segundos.");
    }

    // ---- Agrega tiempo extra al contador ----
    // Lo usaremos en la etapa de power-ups para el bonus de tiempo
    public void AgregarTiempo(float segundosExtra)
    {
        tiempoRestante += segundosExtra;

        // Nos aseguramos de no superar el tiempo total original
        tiempoRestante = Mathf.Min(tiempoRestante, tiempoTotal);

        Debug.Log("Tiempo extra agregado: +" + segundosExtra + "s. Ahora: " + tiempoRestante);

        // Actualizamos la UI inmediatamente
        if (uiManager != null)
        {
            uiManager.ActualizarTimer(tiempoRestante);
        }
    }

    // ---- Devuelve el tiempo transcurrido (no el restante) ----
    // Útil para guardar el "mejor tiempo" en la API
    public float ObtenerTiempoTranscurrido()
    {
        return tiempoTotal - tiempoRestante;
    }
}
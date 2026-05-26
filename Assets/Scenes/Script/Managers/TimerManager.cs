using UnityEngine;
using UnityEngine.Events;

// TimerManager controla el contador regresivo del juego
// Cuando llega a 0 llama directamente al GameManager
// Este script va en el GameObject "TimerManager" en la escena Juego
public class TimerManager : MonoBehaviour
{
    [Header("Configuración")]
    public float tiempoTotal = 60f;

    [Header("Estado actual")]
    public float tiempoRestante;

    // ---- Variables privadas ----
    private bool timerActivo = false;
    private bool yaTermino = false;

    // Referencia directa al GameManager — más confiable que eventos
    private CarrerasGameManager gameManager;

    // Referencia al UIManager para actualizar la pantalla
    private UIManager uiManager;

    // Evento que otros scripts pueden escuchar si quieren
    public UnityEvent onTiempoAgotado;

    void Start()
    {
        // Inicializamos el tiempo
        tiempoRestante = tiempoTotal;
        yaTermino = false;

        // Buscamos el GameManager directamente en la escena
        gameManager = FindObjectOfType<CarrerasGameManager>();

        if (gameManager == null)
        {
            Debug.LogError("TimerManager: NO se encontró el GameManager en la escena.");
        }
        else
        {
            Debug.Log("TimerManager: GameManager encontrado correctamente.");
        }

        // Buscamos el UIManager
        uiManager = FindObjectOfType<UIManager>();

        // Actualizamos la UI con el tiempo inicial
        ActualizarUI();

        // Iniciamos el timer
        timerActivo = true;
        Debug.Log("Timer iniciado: " + tiempoTotal + " segundos.");
    }

    void Update()
    {
        // Si el timer no está activo o ya terminó, no hacemos nada
        if (!timerActivo || yaTermino) return;

        // Reducimos el tiempo cada frame
        tiempoRestante -= Time.deltaTime;

        // Actualizamos la UI
        ActualizarUI();

        // Verificamos si llegó a 0
        if (tiempoRestante <= 0f)
        {
            tiempoRestante = 0f;
            timerActivo = false;
            yaTermino = true;

            Debug.Log("¡TIEMPO AGOTADO! Llamando al GameManager...");

            // Disparamos el evento por si alguien lo escucha
            onTiempoAgotado?.Invoke();

            // Llamamos DIRECTAMENTE al GameManager — esto no puede fallar
            if (gameManager != null)
            {
                gameManager.JugadorPerdioTiempo();
            }
            else
            {
                Debug.LogError("TimerManager: GameManager es null. No se puede procesar derrota.");
            }
        }
    }

    // ---- Actualiza el texto del timer en la UI ----
    void ActualizarUI()
    {
        if (uiManager != null)
        {
            uiManager.ActualizarTimer(tiempoRestante);
        }
    }

    // ---- Detiene el timer (llamado cuando el jugador gana) ----
    public void DetenerTimer()
    {
        timerActivo = false;
        yaTermino = true;
        Debug.Log("Timer detenido. Tiempo restante: " + tiempoRestante);
    }

    // ---- Pausa el timer ----
    public void PausarTimer()
    {
        timerActivo = false;
    }

    // ---- Reanuda el timer ----
    public void ReanudarTimer()
    {
        if (!yaTermino)
            timerActivo = true;
    }

    // ---- Agrega tiempo extra (power-up) ----
    public void AgregarTiempo(float segundos)
    {
        tiempoRestante += segundos;
        tiempoRestante = Mathf.Min(tiempoRestante, tiempoTotal);
        ActualizarUI();
        Debug.Log("Tiempo extra: +" + segundos + "s. Ahora: " + tiempoRestante);
    }

    // ---- Devuelve el tiempo transcurrido ----
    public float ObtenerTiempoTranscurrido()
    {
        return tiempoTotal - tiempoRestante;
    }
}
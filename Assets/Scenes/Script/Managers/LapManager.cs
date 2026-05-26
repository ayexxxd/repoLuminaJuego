using UnityEngine;
using UnityEngine.Events;

// LapManager controla todo el sistema de vueltas
// Vive en un GameObject vacío llamado "LapManager" en la escena
public class LapManager : MonoBehaviour
{
    [Header("Configuración")]
    public int vueltasTotales = 2;
    public int totalCheckpoints = 2;

    [Header("Estado actual (solo lectura)")]
    public int vueltaActual = 0;
    public int checkpointsCruzados = 0;
    public int ultimoCheckpoint = 0;
    public bool puedeCruzarMeta = false;

    // Eventos opcionales
    public UnityEvent<int> onVueltaCompletada;
    public UnityEvent onJugadorGano;

    // Referencias
    private UIManager uiManager;
    private CarrerasGameManager gameManager;
    private TimerManager timerManager;

    void Start()
    {
        // Buscamos referencias
        uiManager    = FindObjectOfType<UIManager>();
        gameManager  = FindObjectOfType<CarrerasGameManager>();
        timerManager = FindObjectOfType<TimerManager>();

        // Verificamos referencias críticas
        if (gameManager == null)
            Debug.LogError("LapManager: No se encontró el GameManager.");

        if (uiManager == null)
            Debug.LogWarning("LapManager: No se encontró el UIManager.");

        // Estado inicial
        vueltaActual        = 1;
        checkpointsCruzados = 0;
        ultimoCheckpoint    = 0;
        puedeCruzarMeta     = false;

        // Actualizamos la UI con la vuelta inicial
        ActualizarUIVueltas();

        Debug.Log("LapManager iniciado. Vueltas requeridas: " + vueltasTotales +
                " | Checkpoints requeridos: " + totalCheckpoints);
    }

    // ---- Llamado por cada Checkpoint cuando la nave lo cruza ----
    public void CheckpointCruzado(int numero)
    {
        Debug.Log("LapManager: CheckpointCruzado(" + numero + ") recibido. " +
                "Último checkpoint: " + ultimoCheckpoint);

        // Verificamos que sea el siguiente en orden
        if (numero != ultimoCheckpoint + 1)
        {
            Debug.Log("LapManager: Checkpoint " + numero + " ignorado. " +
                    "Se esperaba el " + (ultimoCheckpoint + 1));
            return;
        }

        // Registramos el checkpoint
        ultimoCheckpoint = numero;
        checkpointsCruzados++;

        Debug.Log("LapManager: Checkpoint " + numero + " registrado. " +
                "Cruzados: " + checkpointsCruzados + "/" + totalCheckpoints);

        // Si ya cruzó todos los checkpoints, puede cruzar la meta
        if (checkpointsCruzados >= totalCheckpoints)
        {
            puedeCruzarMeta = true;
            Debug.Log("LapManager: Todos los checkpoints cruzados. " +
                    "¡Busca la meta!");

            // Avisamos al jugador con un mensaje en pantalla
            if (uiManager != null)
                uiManager.MostrarMensajeTemporal("¡Busca la meta!", 2f);
        }
    }

    // ---- Llamado por LineaMeta cuando la nave cruza la meta ----
    public void MetaCruzada()
    {
        Debug.Log("LapManager: MetaCruzada() recibido. " +
                "puedeCruzarMeta: " + puedeCruzarMeta);

        // Solo cuenta si cruzó todos los checkpoints
        if (!puedeCruzarMeta)
        {
            Debug.Log("LapManager: Meta ignorada — faltan checkpoints.");

            if (uiManager != null)
                uiManager.MostrarMensajeTemporal("", 2f);
            return;
        }

        // Completó una vuelta válida
        Debug.Log(" LapManager: ¡Vuelta " + vueltaActual + " completada!");

        // Damos puntos por vuelta
        if (PuntosManager.instancia != null)
            PuntosManager.instancia.AgregarPuntosPorVuelta();

        // Disparamos el evento
        onVueltaCompletada?.Invoke(vueltaActual);

        // Verificamos si ya ganó
        if (vueltaActual >= vueltasTotales)
        {
            Debug.Log("🏆 LapManager: ¡El jugador completó todas las vueltas!");
            onJugadorGano?.Invoke();

            // Llamamos directamente al GameManager
            if (gameManager != null)
                gameManager.JugadorGano();

            // Detenemos el timer
            if (timerManager != null)
                timerManager.DetenerTimer();

            return;
        }

        // Preparamos la siguiente vuelta
        vueltaActual++;
        checkpointsCruzados = 0;
        ultimoCheckpoint    = 0;
        puedeCruzarMeta     = false;

        // Actualizamos la UI
        ActualizarUIVueltas();

        // Mensaje de vuelta completada
        if (uiManager != null)
            uiManager.MostrarMensajeTemporal("¡Vuelta " + (vueltaActual - 1) +
                                            " completada!", 2f);

        Debug.Log("LapManager: Vuelta " + vueltaActual + " iniciada.");
    }

    // ---- Actualiza el texto de vueltas en la UI ----
    void ActualizarUIVueltas()
    {
        if (uiManager != null)
            uiManager.ActualizarVueltas(vueltaActual);

        Debug.Log("LapManager: UI actualizada → Vuelta " +
                vueltaActual + "/" + vueltasTotales);
    }
}
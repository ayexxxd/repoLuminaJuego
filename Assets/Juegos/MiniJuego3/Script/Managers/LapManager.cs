using UnityEngine;
using UnityEngine.Events;

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

    public UnityEvent<int> onVueltaCompletada;
    public UnityEvent onJugadorGano;


    private UIManager uiManager;
    private CarrerasGameManager gameManager;
    private TimerManager timerManager;

    void Start()
    {

        uiManager    = FindObjectOfType<UIManager>();
        gameManager  = FindObjectOfType<CarrerasGameManager>();
        timerManager = FindObjectOfType<TimerManager>();

        if (gameManager == null)
            Debug.LogError("LapManager: No se encontró el GameManager.");

        if (uiManager == null)
            Debug.LogWarning("LapManager: No se encontró el UIManager.");

        vueltaActual        = 0;
        checkpointsCruzados = 0;
        ultimoCheckpoint    = 0;
        puedeCruzarMeta     = false;

        ActualizarUIVueltas();

        Debug.Log("LapManager iniciado. Vueltas requeridas: " + vueltasTotales +
                " | Checkpoints requeridos: " + totalCheckpoints);
    }

    public void CheckpointCruzado(int numero)
    {
        Debug.Log("LapManager: CheckpointCruzado(" + numero + ") recibido. " +
                "Último checkpoint: " + ultimoCheckpoint);

        if (numero != ultimoCheckpoint + 1)
        {
            Debug.Log("LapManager: Checkpoint " + numero + " ignorado. " +
                    "Se esperaba el " + (ultimoCheckpoint + 1));
            return;
        }

        
        ultimoCheckpoint = numero;
        checkpointsCruzados++;

        Debug.Log("LapManager: Checkpoint " + numero + " registrado. " +
                "Cruzados: " + checkpointsCruzados + "/" + totalCheckpoints);

        
        if (checkpointsCruzados >= totalCheckpoints)
        {
            puedeCruzarMeta = true;
            Debug.Log("LapManager: Todos los checkpoints cruzados. " +
                    "¡Busca la meta!");

        
            if (uiManager != null)
                uiManager.MostrarMensajeTemporal("¡Busca la meta!", 2f);
        }
    }

    
    public void MetaCruzada()
    {
        Debug.Log("LapManager: MetaCruzada() recibido. " +
                "puedeCruzarMeta: " + puedeCruzarMeta);

        if (!puedeCruzarMeta)
        {
            Debug.Log("LapManager: Meta ignorada — faltan checkpoints.");

            if (uiManager != null)
                uiManager.MostrarMensajeTemporal(" ", 2f);
            return;
        }

        Debug.Log(" LapManager: ¡Vuelta completada!");

        if (PuntosManager.instancia != null)
            PuntosManager.instancia.AgregarPuntosPorVuelta();

        onVueltaCompletada?.Invoke(vueltaActual);

        if (vueltaActual >= vueltasTotales)
        {
            Debug.Log(" LapManager: ¡El jugador completó todas las vueltas!");
            onJugadorGano?.Invoke();

            if (gameManager != null)
                gameManager.JugadorGano();

            if (timerManager != null)
                timerManager.DetenerTimer();

            return;
        }

        vueltaActual++;
        checkpointsCruzados = 0;
        ultimoCheckpoint    = 0;
        puedeCruzarMeta     = false;


        ActualizarUIVueltas();

        if (uiManager != null)
            uiManager.MostrarMensajeTemporal("¡Vuelta completada!", 2f);

        Debug.Log("LapManager: Vuelta " + vueltaActual + " iniciada.");
    }

    void ActualizarUIVueltas()
    {
        if (uiManager != null)
            uiManager.ActualizarVueltas(vueltaActual);

        Debug.Log("LapManager: UI actualizada → Vuelta " +
                vueltaActual + "/" + vueltasTotales);
    }
}
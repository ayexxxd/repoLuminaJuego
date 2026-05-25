using UnityEngine;
using UnityEngine.Events;

// LapManager controla todo el sistema de vueltas del juego
// Vive en un GameObject vacío en la escena
public class LapManager : MonoBehaviour
{
    [Header("Configuración de Vueltas")]

    // Cuántas vueltas necesita completar el jugador para ganar
    public int vueltasTotales = 3;

    // Cuántos checkpoints tiene la pista
    public int totalCheckpoints = 3;

    [Header("Estado Actual (solo lectura)")]

    // Empieza en 0 para que la primera vuelta sea 1
    public int vueltaActual = 0;

    // ---- Variables privadas ----

    // Último checkpoint cruzado
    private int ultimoCheckpointCruzado = 0;

    // Cuántos checkpoints lleva en la vuelta actual
    private int checkpointsCruzadosEnVuelta = 0;

    // Solo permite cruzar meta si pasó todos los checkpoints
    private bool puedeCruzarMeta = false;
    

    // Evita contar varias veces la meta rápidamente
    private bool metaBloqueada = false;

    // Evento cuando se completa una vuelta
    public UnityEvent<int> onVueltaCompletada;

    // Evento cuando el jugador gana
    public UnityEvent onJugadorGano;

    void Start()
    {
        puedeCruzarMeta = false;

        Debug.Log("LapManager iniciado.");
    }

    // ----------------------------------------------------
    // CHECKPOINTS
    // ----------------------------------------------------
    public void CheckpointCruzado(int numeroCheckpoint)
    {
        // Verificamos que pase los checkpoints en orden
        if (numeroCheckpoint == ultimoCheckpointCruzado + 1)
        {
            ultimoCheckpointCruzado = numeroCheckpoint;

            checkpointsCruzadosEnVuelta++;

            Debug.Log("Checkpoint " + numeroCheckpoint + " cruzado.");

            // Si ya cruzó todos los checkpoints
            if (checkpointsCruzadosEnVuelta >= totalCheckpoints)
            {
                puedeCruzarMeta = true;

                Debug.Log("¡Todos los checkpoints cruzados! Ve a la meta.");
            }
        }
        else
        {
            Debug.Log(
                "Checkpoint incorrecto. Debes pasar el checkpoint "
                + (ultimoCheckpointCruzado + 1)
            );
        }
    }

    // ----------------------------------------------------
    // META
    // ----------------------------------------------------
    public void MetaCruzada()
    {
        // Si la meta está bloqueada, ignoramos
        if (metaBloqueada)
        {
            return;
        }

        // Verificamos que haya pasado todos los checkpoints
        if (!puedeCruzarMeta)
        {
            Debug.Log("Debes pasar todos los checkpoints primero.");
            return;
        }

        // Bloqueamos temporalmente la meta
        metaBloqueada = true;

        // Desbloqueamos después de 1 segundo
        Invoke(nameof(DesbloquearMeta), 1f);

        // Aumentamos la vuelta
        vueltaActual++;

        Debug.Log("¡Vuelta " + vueltaActual + " completada!");

        // Damos puntos por vuelta
        if (PuntosManager.instancia != null)
        {
            PuntosManager.instancia.AgregarPuntosPorVuelta();
        }

        // Actualizamos la UI
        onVueltaCompletada?.Invoke(vueltaActual);

        // Verificamos si ganó
        if (vueltaActual >= vueltasTotales)
        {
            Debug.Log("¡EL JUGADOR GANÓ!");

            // Detenemos el timer
            TimerManager timerManager =
                Object.FindAnyObjectByType<TimerManager>();

            if (timerManager != null)
            {
                timerManager.DetenerTimer();
            }

            // Disparamos evento de victoria
            onJugadorGano?.Invoke();

            return;
        }

        // Reiniciamos para la siguiente vuelta
        ultimoCheckpointCruzado = 0;

        checkpointsCruzadosEnVuelta = 0;

        puedeCruzarMeta = false;
    }

    // ----------------------------------------------------
    // DESBLOQUEAR META
    // ----------------------------------------------------
    void DesbloquearMeta()
    {
        metaBloqueada = false;
    }
}
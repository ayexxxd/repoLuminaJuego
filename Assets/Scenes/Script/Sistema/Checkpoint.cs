using UnityEngine;

// Script que va en cada objeto Checkpoint de la pista
// Detecta cuando la nave lo cruza y avisa al LapManager
public class Checkpoint : MonoBehaviour
{
    [Header("Configuración")]
    // Número de orden — asígnalo manualmente en el Inspector de cada checkpoint
    // Checkpoint1 = 1, Checkpoint2 = 2, Checkpoint3 = 3
    public int numeroCheckpoint;

    void Start()
    {
        // Verificamos configuración al iniciar
        Collider2D col = GetComponent<Collider2D>();

        if (col == null)
        {
            Debug.LogError(gameObject.name + ": No tiene Collider2D.");
            return;
        }

        if (!col.isTrigger)
        {
            Debug.LogError(gameObject.name + ": El Collider2D NO tiene isTrigger activado. " +
                        "Actívalo en el Inspector.");
        }

        if (numeroCheckpoint == 0)
        {
            Debug.LogWarning(gameObject.name + ": El Numero Checkpoint es 0. " +
                        "Asigna 1, 2 o 3 en el Inspector.");
        }

        Debug.Log(gameObject.name + ": Checkpoint " + numeroCheckpoint + " listo.");
    }

    // ---- Se llama cuando cualquier objeto con Rigidbody2D entra al trigger ----
    void OnTriggerEnter2D(Collider2D otro)
    {
        // Log para diagnosticar — muestra CUALQUIER objeto que entre al trigger
        Debug.Log(gameObject.name + ": algo entró al trigger → " +
                otro.gameObject.name + " (tag: " + otro.tag + ")");

        // Verificamos que sea el jugador por su tag
        if (!otro.CompareTag("Jugador"))
        {
            Debug.Log(gameObject.name + ": ignorado porque no es el jugador.");
            return;
        }

        Debug.Log(" Checkpoint " + numeroCheckpoint + " cruzado por el jugador.");

        // Buscamos el LapManager en la escena
        LapManager lapManager = FindObjectOfType<LapManager>();

        if (lapManager == null)
        {
            Debug.LogError(gameObject.name + ": No se encontró el LapManager en la escena.");
            return;
        }

        // Avisamos al LapManager
        lapManager.CheckpointCruzado(numeroCheckpoint);
    }
}
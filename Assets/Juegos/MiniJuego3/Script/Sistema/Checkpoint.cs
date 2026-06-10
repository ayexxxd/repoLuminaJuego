using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Configuración")]
    public int numeroCheckpoint;

    void Start()
    {
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

    void OnTriggerEnter2D(Collider2D otro)
    {
        Debug.Log(gameObject.name + ": algo entró al trigger → " +
                otro.gameObject.name + " (tag: " + otro.tag + ")");

        if (!otro.CompareTag("Jugador"))
        {
            Debug.Log(gameObject.name + ": ignorado porque no es el jugador.");
            return;
        }

        Debug.Log(" Checkpoint " + numeroCheckpoint + " cruzado por el jugador.");

        
        Rigidbody2D rb = otro.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float dot = Vector2.Dot(rb.linearVelocity.normalized, transform.up);
            if (dot < -0.5f)
            {
                Debug.Log("Checkpoint " + numeroCheckpoint + ": jugador va en reversa, ignorado.");
                return;
            }
        }

        LapManager lapManager = FindObjectOfType<LapManager>();

        if (lapManager == null)
        {
            Debug.LogError(gameObject.name + ": No se encontró el LapManager en la escena.");
            return;
        }

    
        lapManager.CheckpointCruzado(numeroCheckpoint);
    }
}
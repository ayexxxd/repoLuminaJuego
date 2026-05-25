using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Configuración")]
    public int numeroCheckpoint;

    // Esta variable es estática: todos los checkpoints comparten el mismo contador
    // Empieza en 0 y sube conforme pasamos los checkpoints en orden
    public static int checkpointActual = 0; 

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Jugador"))
        {
            // Solo activamos si es el siguiente checkpoint en la secuencia
            if (numeroCheckpoint == checkpointActual + 1)
            {
                checkpointActual = numeroCheckpoint;
                Debug.Log("¡Checkpoint " + numeroCheckpoint + " alcanzado!");
                
                // Aquí podrías añadir lógica para ganar puntos o verificar vuelta completa
            }
        }
    }
}
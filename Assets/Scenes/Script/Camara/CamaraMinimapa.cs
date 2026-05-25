using UnityEngine;

// Controla la cámara del minimapa
// Puede seguir al jugador o quedarse fija mostrando toda la pista
public class CamaraMinimapa : MonoBehaviour
{
    [Header("Configuración")]
    // Si es true, la cámara sigue al jugador
    // Si es false, la cámara está fija mostrando toda la pista
    public bool seguirJugador = false;

    // Referencia al jugador — solo se usa si seguirJugador = true
    public Transform jugador;

    // Altura fija de la cámara (posición Z en 2D)
    private float alturaZ;

    void Start()
    {
        // Guardamos la Z original
        alturaZ = transform.position.z;

        // Si no asignaron el jugador manualmente, lo buscamos
        if (jugador == null)
        {
            GameObject naveObj = GameObject.FindWithTag("Jugador");
            if (naveObj != null)
            {
                jugador = naveObj.transform;
            }
        }
    }

    void LateUpdate()
    {
        // Solo seguimos al jugador si está configurado así
        if (!seguirJugador || jugador == null) return;

        // Seguimos la posición X e Y del jugador pero mantenemos Z fija
        transform.position = new Vector3(
            jugador.position.x,
            jugador.position.y,
            alturaZ
        );
    }
}
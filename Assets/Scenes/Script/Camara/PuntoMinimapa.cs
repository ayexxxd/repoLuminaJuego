using UnityEngine;

public class PuntoMinimapa : MonoBehaviour
{
    // Nave real
    public Transform jugador;

    // Qué tan pequeño será el movimiento
    public float escalaMovimiento = 0.02f;

    // Posición inicial del punto
    private Vector3 posicionInicial;

    // Posición inicial del jugador
    private Vector3 jugadorInicial;

    void Start()
    {
        posicionInicial = transform.localPosition;

        if (jugador != null)
        {
            jugadorInicial = jugador.position;
        }
    }

    void Update()
    {
        if (jugador == null) return;

        // Diferencia de movimiento
        Vector3 diferencia = jugador.position - jugadorInicial;

        // Movimiento reducido
        transform.localPosition = posicionInicial + new Vector3(
            diferencia.x * escalaMovimiento,
            diferencia.y * escalaMovimiento,
            0
        );
    }
}
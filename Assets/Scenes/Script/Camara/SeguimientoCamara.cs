using UnityEngine;

// Este script hace que la cámara siga a la nave de forma suave
// Debe estar en el GameObject de la Main Camera
public class SeguimientoCamara : MonoBehaviour
{
    [Header("Objetivo a seguir")]
    // Arrastra aquí el GameObject de la nave desde el Hierarchy
    public Transform objetivo;

    [Header("Configuración de seguimiento")]
    // Qué tan rápido la cámara alcanza a la nave
    // Valores bajos = más lento y suave / Valores altos = más rápido y directo
    // Recomendado: entre 2 y 6
    public float velocidadSeguimiento = 4f;

    // Distancia en Z que mantiene la cámara
    // En 2D la cámara debe estar en Z negativo para ver la escena
    // NO cambies esto a menos que la cámara deje de ver la escena
    private float offsetZ;

    void Awake()
    {
        // Guardamos la posición Z original de la cámara al iniciar
        // Así no la perdemos cuando movemos X e Y
        offsetZ = transform.position.z;
    }

    // Usamos LateUpdate en lugar de Update
    // LateUpdate() se ejecuta DESPUÉS de que todos los demás objetos ya se movieron
    // Esto evita que la cámara "tiemble" porque se mueve antes que la nave
    void LateUpdate()
    {
        // Si no hay objetivo asignado, no hacemos nada
        // Esto evita errores si olvidaste arrastrar la nave
        if (objetivo == null)
        {
            Debug.LogWarning("SeguimientoCamara: No hay objetivo asignado.");
            return;
        }

        // Creamos la posición a la que queremos llegar
        // Tomamos X e Y de la nave, pero mantenemos nuestra Z original
        Vector3 posicionObjetivo = new Vector3(
            objetivo.position.x,
            objetivo.position.y,
            offsetZ
        );

        // Vector3.Lerp interpola suavemente entre la posición actual y la posición objetivo
        // Time.deltaTime * velocidadSeguimiento controla qué tan rápido se mueve
        transform.position = Vector3.Lerp(
            transform.position,
            posicionObjetivo,
            Time.deltaTime * velocidadSeguimiento
        );
    }
}
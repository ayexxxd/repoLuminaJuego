using UnityEngine;

// CameraShake hace que la cámara vibre cuando ocurre un impacto
// Va en el mismo GameObject de la Main Camera
public class CameraShake : MonoBehaviour
{
    [Header("Configuración del shake")]
    // Qué tan fuerte tiembla la cámara
    // Valores recomendados: 0.1 a 0.4
    public float intensidad = 0.2f;

    // Cuántos segundos dura el temblor
    public float duracion = 0.3f;

    // Singleton para acceso desde cualquier script
    public static CameraShake instancia;

    // ---- Variables privadas ----

    // Posición original de la cámara antes del shake
    // La guardamos para volver exactamente al mismo lugar
    private Vector3 posicionOriginal;

    // Si el shake está activo ahora mismo
    private bool shakeActivo = false;

    // Tiempo que lleva temblando
    private float tiempoTranscurrido = 0f;

    void Awake()
    {
        if (instancia == null) instancia = this;
        else Destroy(this);
    }

    void LateUpdate()
    {
        // Solo ejecutamos la lógica si el shake está activo
        if (!shakeActivo) return;

        // Verificamos si ya pasó el tiempo de duración
        if (tiempoTranscurrido < duracion)
        {
            // Generamos una posición aleatoria pequeña alrededor de la original
            // Random.insideUnitCircle da un punto aleatorio dentro de un círculo de radio 1
            // Lo multiplicamos por la intensidad para controlar qué tan fuerte tiembla
            Vector2 desplazamiento = Random.insideUnitCircle * intensidad;

            // Aplicamos el desplazamiento a la posición original
            // Mantenemos la Z original para que la cámara no se aleje de la escena
            transform.position = new Vector3(
                posicionOriginal.x + desplazamiento.x,
                posicionOriginal.y + desplazamiento.y,
                posicionOriginal.z
            );

            // Contamos el tiempo transcurrido
            tiempoTranscurrido += Time.deltaTime;
        }
        else
        {
            // El shake terminó — restauramos la posición exacta
            // El script de seguimiento de cámara tomará el control de nuevo
            transform.position = posicionOriginal;
            shakeActivo = false;
            tiempoTranscurrido = 0f;

            Debug.Log("CameraShake: temblor terminado.");
        }
    }

    // ---- Inicia el temblor de cámara ----
    // Llamado desde otros scripts cuando ocurre un impacto
    public void Shake()
    {
        // Guardamos la posición actual como base del shake
        // Si el seguimiento de cámara ya la movió, tomamos esa posición
        posicionOriginal = transform.position;
        tiempoTranscurrido = 0f;
        shakeActivo = true;

        Debug.Log("CameraShake: iniciando temblor. Intensidad: " + intensidad +
                  " | Duración: " + duracion + "s");
    }

    // ---- Versión con parámetros personalizados ----
    // Útil para choques más fuertes o más suaves según el tipo de impacto
    public void Shake(float intensidadPersonalizada, float duracionPersonalizada)
    {
        posicionOriginal      = transform.position;
        tiempoTranscurrido    = 0f;
        shakeActivo           = true;

        // Usamos los valores personalizados temporalmente
        float intensidadOriginal = intensidad;
        float duracionOriginal   = duracion;

        intensidad = intensidadPersonalizada;
        duracion   = duracionPersonalizada;

        Debug.Log("CameraShake: temblor personalizado. Intensidad: " +
                  intensidadPersonalizada + " | Duración: " + duracionPersonalizada);
    }
}
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Configuración del shake")]
    public float intensidad = 0.2f;

    public float duracion = 0.3f;

    public static CameraShake instancia;

    private Vector3 posicionOriginal;

    private bool shakeActivo = false;

    private float tiempoTranscurrido = 0f;

    void Awake()
    {
        if (instancia == null) instancia = this;
        else Destroy(this);
    }

    void LateUpdate()
    {
        if (!shakeActivo) return;

        if (tiempoTranscurrido < duracion)
        {
            Vector2 desplazamiento = Random.insideUnitCircle * intensidad;

            transform.position = new Vector3(
                posicionOriginal.x + desplazamiento.x,
                posicionOriginal.y + desplazamiento.y,
                posicionOriginal.z
            );

            tiempoTranscurrido += Time.deltaTime;
        }
        else
        {
            transform.position = posicionOriginal;
            shakeActivo = false;
            tiempoTranscurrido = 0f;

            Debug.Log("CameraShake: temblor terminado.");
        }
    }

    public void Shake()
    {
        posicionOriginal = transform.position;
        tiempoTranscurrido = 0f;
        shakeActivo = true;

        Debug.Log("CameraShake: iniciando temblor. Intensidad: " + intensidad +
                  " | Duración: " + duracion + "s");
    }

    public void Shake(float intensidadPersonalizada, float duracionPersonalizada)
    {
        posicionOriginal      = transform.position;
        tiempoTranscurrido    = 0f;
        shakeActivo           = true;

        float intensidadOriginal = intensidad;
        float duracionOriginal   = duracion;

        intensidad = intensidadPersonalizada;
        duracion   = duracionPersonalizada;

        Debug.Log("CameraShake: temblor personalizado. Intensidad: " +
                  intensidadPersonalizada + " | Duración: " + duracionPersonalizada);
    }
}
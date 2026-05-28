using UnityEngine;

public class SeguimientoCamara : MonoBehaviour
{
    [Header("Objetivo a seguir")]
    public Transform objetivo;

    [Header("Configuración de seguimiento")]
    public float velocidadSeguimiento = 4f;

    private float offsetZ;

    void Awake()
    {
        offsetZ = transform.position.z;
    }

    void LateUpdate()
    {
        if (objetivo == null)
        {
            Debug.LogWarning("SeguimientoCamara: No hay objetivo asignado.");
            return;
        }

        Vector3 posicionObjetivo = new Vector3(
            objetivo.position.x,
            objetivo.position.y,
            offsetZ
        );

        transform.position = Vector3.Lerp(
            transform.position,
            posicionObjetivo,
            Time.deltaTime * velocidadSeguimiento
        );
    }
}
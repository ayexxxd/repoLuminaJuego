using UnityEngine;

public class ObstaculoMovimiento : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;

    public float velocidad = 2f;

    private Transform objetivoActual;

    void Start()
    {
        objetivoActual = puntoB;
    }

    void Update()
    {
        MoverObstaculo();
    }

    void MoverObstaculo()
    {
        transform.position = Vector3.MoveTowards(transform.position, objetivoActual.position, velocidad * Time.deltaTime);

        if (Vector3.Distance(transform.position, objetivoActual.position) < 0.1f)
        {
            if (objetivoActual == puntoA)
            {
                objetivoActual = puntoB;
            }
            else
            {
                objetivoActual = puntoA;
            }
        }
    }
}
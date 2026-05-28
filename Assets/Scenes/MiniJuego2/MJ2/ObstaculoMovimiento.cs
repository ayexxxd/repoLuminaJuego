using UnityEngine;

public class ObstaculoMovimiento : MonoBehaviour
{

    // puntos invisibles en la escena
    public Transform puntoA;
    public Transform puntoB;
// ////////////////


    public float velocidad = 2f;


// guarda hacia que punto se va a empezar a mover la lavadora si al A o al B
    private Transform objetivoActual;
// /////////////////




    void Start()
    {
        objetivoActual = puntoB;
    }



// esto hace que se ejecute todo el tiempo mientras el juego está corriendo
    void Update()
    {
        MoverObstaculo();
    }



    void MoverObstaculo()
    {
        // con esto se mueve la lavadora poco a poco hacia el punto objetivo
        transform.position = Vector3.MoveTowards(transform.position, objetivoActual.position, velocidad * Time.deltaTime);
        //Vector3.MoveTowards sirve para desplazar un objeto desde su posición actual hacia otra posición.
        // Time.deltaTime ayuda a que el movimiento sea estable y no dependa tanto de la velocidad de la computadora.


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
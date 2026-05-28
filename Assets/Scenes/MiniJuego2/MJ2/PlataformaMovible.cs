using UnityEngine;

public class PlataformaMovible : MonoBehaviour
{

// este es para representar el lugar en donde quiero que termine la plataforma
// fue lo que puse como Empty que no se ve en el juego pero que guarda posicion final y rotacion final de la plataforma 
    public Transform puntoFinal;



    public float velocidadMovimiento = 2f;
    public float velocidadRotacion = 100f;



// esta variable funciona como interruptor 
// o sea que solo se moverá cuando la palanca llame al meto de ActivarMovimiento
    private bool debeMoverse = false;



    void Update()
    {
        if (debeMoverse)
        {
            MoverYRotarPlataforma();
        }
    }




    public void ActivarMovimiento()
    {
        debeMoverse = true;
    }




    void MoverYRotarPlataforma()
    {
        transform.position = Vector3.MoveTowards( // este es el que mueve la plataforma poco a poco hacia la posicion del punto final
            transform.position,
            puntoFinal.position,
            velocidadMovimiento * Time.deltaTime
        );

        transform.rotation = Quaternion.RotateTowards( // este es el que rota la plataforma poco a poco hacia la rotación del punto Final
            transform.rotation,
            puntoFinal.rotation,
            velocidadRotacion * Time.deltaTime
        );

// este es el que checa si la plataforma ya llegó a la posicion y rotacion exacta 
        if (Vector3.Distance(transform.position, puntoFinal.position) < 0.05f &&
            Quaternion.Angle(transform.rotation, puntoFinal.rotation) < 0.5f)
        {
            transform.position = puntoFinal.position;
            transform.rotation = puntoFinal.rotation;
            debeMoverse = false;
        }
        // cuando lo hace el script hace
        // 1: Ajusta la posición exacta al punto final
        // 2: Ajusta la rotación exacta al punto final
        // 3: Detiene el movimiento 
    }
}


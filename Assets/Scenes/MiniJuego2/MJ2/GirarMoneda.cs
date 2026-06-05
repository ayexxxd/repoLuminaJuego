using UnityEngine;

public class GirarMoneda : MonoBehaviour
{
    public float velocidadGiro = 4f;
    public float anchoMinimo = 0.1f;


// sirve para guardar el tamaño original de la moneda
    private Vector3 escalaOriginal;

    void Start()
    {
        escalaOriginal = transform.localScale; // sepa cuál era el tamaño normal de la moneda antes de empeza
    }

    void Update()
    {
        float nuevoAncho = Mathf.Abs(Mathf.Sin(Time.time * velocidadGiro));
// Mathf.Sin genera un valor que sube y baja constantemente. Con eso hacemos que el ancho de la moneda cambie todo el tiempo.
        if (nuevoAncho < anchoMinimo)
        {
            nuevoAncho = anchoMinimo;
        } // evita que desaparezca la moneda al momento de estar girando debido a que al momento de girar puede llegar a no verse

        transform.localScale = new Vector3(nuevoAncho * escalaOriginal.x, escalaOriginal.y, escalaOriginal.z);
    } // Aquí se cambia únicamente el ancho de la moneda, o sea el eje X
    // hace que gire sin mover su posicion ni afectar la logica de recoleccion

}
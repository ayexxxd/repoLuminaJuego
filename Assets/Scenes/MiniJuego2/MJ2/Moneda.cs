using UnityEngine;

public class Moneda : MonoBehaviour
{
    private ControlNivel controlNivel;


// aqui es en donde busca el script ControlNivel
// porque es el script que maneja el contador y la secuencia de las monedas
    void Start()
    {
        controlNivel = FindFirstObjectByType<ControlNivel>();
    }

    void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player")) // se asegura que el que tocó la moneda sea el jugador y no un objeto 
    {
        controlNivel.RecolectarMoneda(); // se le llama al método de RecolectarMoneda del ControlNivel

        if (AudioManager.instancia != null)
        {
            AudioManager.instancia.ReproducirMoneda();
        }

        gameObject.SetActive(false); // despues de recolectar la moneda, esta se desactiva para que ya no aparezca
    }
}
}
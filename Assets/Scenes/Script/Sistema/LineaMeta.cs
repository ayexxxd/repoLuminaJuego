using UnityEngine;

public class LineaMeta : MonoBehaviour
{
    [Header("Configuración de Vueltas")]
    public int vueltasTotales = 3; // Cuántas vueltas necesita el jugador para ganar
    private int vueltasActuales = 0;

    void OnTriggerEnter2D(Collider2D otro)
    {
        // Solo reaccionamos si el objeto que entra tiene el tag "Jugador"
        if (otro.CompareTag("Jugador"))
        {
            MetaCruzada();
        }
    }

    public void MetaCruzada()
    {
        vueltasActuales++;
        Debug.Log("¡Meta cruzada! Vuelta actual: " + vueltasActuales);

        if (vueltasActuales >= vueltasTotales)
        {
            Debug.Log("¡Felicidades, has ganado el juego!");
            // Aquí puedes poner el código para cargar la pantalla de victoria
        }
    }
}
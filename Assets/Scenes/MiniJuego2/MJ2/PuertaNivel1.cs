using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PuertaNivel : MonoBehaviour
{


    // esto permite usar el mismo script para las 3 puertas ya que solo se cambia el numero de nivel desde el inspector
    public int numeroNivel = 1;
    public string escenaPregunta = "ENP";
// /////////////////


    public Sprite puertaAbierta;
    public float tiempoEspera = 0.7f;



// referencias internas o sea de que vienen de otros scripts y que van a servir en este 
    private ControlNivel controlNivel;
    private SpriteRenderer spriteRendererPuerta; // cambiar visualmente puerta cerrada a abierta 
    private bool puertaYaSeAbrio = false; // evita que la puerta se active si el jugador se pone sobre de ella 
// /////////////////



    void Start()
    {
        controlNivel = FindFirstObjectByType<ControlNivel>(); // la puerta busca ControlNivel
        spriteRendererPuerta = GetComponent<SpriteRenderer>(); // agarra su propio SpriteRenderer
    }

    void OnTriggerEnter2D(Collider2D collision) // para detectar al jugador, o sea para detectar cuando la tocan 
    {
        if (collision.CompareTag("Player") && puertaYaSeAbrio == false)
        {
            if (controlNivel.YaRecolectoTodas()) // para saber si el jugador ya recolectó todas las monedas y pueda activarse 
            {
                puertaYaSeAbrio = true;
                
                // aqui la puerta guarda esos dos datos en DatosJuego
                DatosJuego.nivelActual = numeroNivel;
                DatosJuego.tokensNivelPendiente = controlNivel.totalMonedas;

                if (puertaAbierta != null)
                {
                    spriteRendererPuerta.sprite = puertaAbierta;
                }
                
                if (AudioManager.instancia != null)
                {
                    AudioManager.instancia.ReproducirPuerta();
                }

                StartCoroutine(AbrirPuertaYCambiarEscena());
            }
        }
    }

    IEnumerator AbrirPuertaYCambiarEscena()
    {
        yield return new WaitForSeconds(tiempoEspera); // esto es para que cuando se abra la ouerta se espere un ratito y pues aprovechando para que suene el sonido de efecto

        SceneManager.LoadScene(escenaPregunta);
    }
}
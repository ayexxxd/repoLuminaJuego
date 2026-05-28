using UnityEngine;
using System.Collections;

public class Palanca : MonoBehaviour
{

    // Sirve para conectarse desde Unity
    public PlataformaMovible plataforma;
// ////////////////

    public Sprite palancaActivada;

    public float tiempoEspera = 0.5f;

    private bool yaActivada = false; // esta variable evita que la palanca se active varias veces

    private ControlNivel controlNivel; // para conectar con ControlNivel

    private SpriteRenderer spriteRendererPalanca;



    void Start()
    {
        controlNivel = FindFirstObjectByType<ControlNivel>(); // para que la palanca pueda consultar el estado del nivel 

        spriteRendererPalanca = GetComponent<SpriteRenderer>();
    }




    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && yaActivada == false)
        {
            if (controlNivel.FaltaUltimaMoneda()) // esta es para que la palanca solo se active si falta la ultima moneda 
            {
                yaActivada = true;

                StartCoroutine(ActivarPalanca());
            }
        }
    }




    IEnumerator ActivarPalanca()
    {
        yield return new WaitForSeconds(tiempoEspera);

        if (palancaActivada != null)
        {
            spriteRendererPalanca.sprite = palancaActivada; // cambia el sprite de la palanca a pues la palanca ya activada 
        }

        if (plataforma != null)
        {
            plataforma.ActivarMovimiento(); // metodo que pertenece al script de PlataformaMovible
        }
    }
}
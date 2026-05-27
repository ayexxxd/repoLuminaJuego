using UnityEngine;
using System.Collections;

public class Palanca : MonoBehaviour
{
    public PlataformaMovible plataforma;

    public Sprite palancaActivada;

    public float tiempoEspera = 0.5f;

    private bool yaActivada = false;

    private ControlNivel controlNivel;

    private SpriteRenderer spriteRendererPalanca;

    void Start()
    {
        controlNivel = FindFirstObjectByType<ControlNivel>();

        spriteRendererPalanca = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && yaActivada == false)
        {
            if (controlNivel.FaltaUltimaMoneda())
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
            spriteRendererPalanca.sprite = palancaActivada;
        }

        if (plataforma != null)
        {
            plataforma.ActivarMovimiento();
        }
    }
}
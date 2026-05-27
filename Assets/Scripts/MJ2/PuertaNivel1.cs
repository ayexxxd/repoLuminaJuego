using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PuertaNivel : MonoBehaviour
{
    public int numeroNivel = 1;
    public string escenaPregunta = "ENP";

    public Sprite puertaAbierta;
    public float tiempoEspera = 0.7f;

    private ControlNivel controlNivel;
    private SpriteRenderer spriteRendererPuerta;
    private bool puertaYaSeAbrio = false;

    void Start()
    {
        controlNivel = FindFirstObjectByType<ControlNivel>();
        spriteRendererPuerta = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && puertaYaSeAbrio == false)
        {
            if (controlNivel.YaRecolectoTodas())
            {
                puertaYaSeAbrio = true;

                DatosJuego.nivelActual = numeroNivel;
                DatosJuego.tokensNivelPendiente = controlNivel.totalMonedas;

                if (puertaAbierta != null)
                {
                    spriteRendererPuerta.sprite = puertaAbierta;
                }

                StartCoroutine(AbrirPuertaYCambiarEscena());
            }
        }
    }

    IEnumerator AbrirPuertaYCambiarEscena()
    {
        yield return new WaitForSeconds(tiempoEspera);

        SceneManager.LoadScene(escenaPregunta);
    }
}
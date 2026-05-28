using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LavadoraPeligrosa : MonoBehaviour
{
    private bool yaTocoJugador = false;
    // se usa esta variable booleana para evitar que el reinicio se active varias veces al mismo tiempo

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && yaTocoJugador == false)
        {
            yaTocoJugador = true;

            StartCoroutine(ReiniciarConSonido());
        }
    }

    IEnumerator ReiniciarConSonido()
    {
        if (AudioManager.instancia != null)
        {
            AudioManager.instancia.ReproducirLavadoraRoja();
        }

        yield return new WaitForSeconds(0.7f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
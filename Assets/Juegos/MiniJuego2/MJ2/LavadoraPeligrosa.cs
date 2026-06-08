using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LavadoraPeligrosa : MonoBehaviour
{
    private bool yaTocoJugador = false;

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
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControlPregunta : MonoBehaviour
{
    public PreguntaApi preguntaApi;

    public void ResponderCorrecto()
    {
        StartCoroutine(RevisarRespuesta("Correcto"));
    }

    public void ResponderIncorrecto()
    {
        StartCoroutine(RevisarRespuesta("Incorrecto"));
    }

    IEnumerator RevisarRespuesta(string respuestaJugador)
    {
        if (preguntaApi.RespuestaEsCorrecta(respuestaJugador))
        {
            if (AudioManager.instancia != null)
            {
                AudioManager.instancia.ReproducirCorrecto();
            }

            DatosJuego.tokensPartida += DatosJuego.tokensNivelPendiente;
            DatosJuego.tokensNivelPendiente = 0;

            yield return new WaitForSeconds(0.5f);

            AvanzarNivel();
        }
        else
        {
            if (AudioManager.instancia != null)
            {
                AudioManager.instancia.ReproducirIncorrecto();
            }

            DatosJuego.tokensNivelPendiente = 0;

            yield return new WaitForSeconds(1f);

            RepetirNivel();
        }
    }

    void AvanzarNivel()
    {
        if (DatosJuego.nivelActual == 1)
        {
            SceneManager.LoadScene("EN2");
        }
        else if (DatosJuego.nivelActual == 2)
        {
            SceneManager.LoadScene("EN3");
        }
        else if (DatosJuego.nivelActual == 3)
        {
            SceneManager.LoadScene("EFW");
        }
    }

    void RepetirNivel()
    {
        if (DatosJuego.nivelActual == 1)
        {
            SceneManager.LoadScene("EN1");
        }
        else if (DatosJuego.nivelActual == 2)
        {
            SceneManager.LoadScene("EN2");
        }
        else if (DatosJuego.nivelActual == 3)
        {
            SceneManager.LoadScene("EN3");
        }
    }

    public void CambiarPregunta()
    {
        SceneManager.LoadScene("ENP");
    }

    public void RegresarMisionEscape()
    {
        SceneManager.LoadScene("EMI");
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPregunta : MonoBehaviour
{
    public PreguntaApi preguntaApi;

    public void ResponderCorrecto()
    {
        RevisarRespuesta("Correcto");
    }

    public void ResponderIncorrecto()
    {
        RevisarRespuesta("Incorrecto");
    }

    void RevisarRespuesta(string respuestaJugador)
    {
        if (preguntaApi.RespuestaEsCorrecta(respuestaJugador))
        {
            DatosJuego.tokensPartida += DatosJuego.tokensNivelPendiente;
            DatosJuego.tokensNivelPendiente = 0;

            AvanzarNivel();
        }
        else
        {
            DatosJuego.tokensNivelPendiente = 0;

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
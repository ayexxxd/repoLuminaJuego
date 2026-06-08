using UnityEngine;
using UnityEngine.SceneManagement;
using Ximena.Sonido;
public class CarrerasGameManager : MonoBehaviour
{
    public enum EstadoJuego { Jugando, Victoria, Derrota }
    public EstadoJuego estadoActual = EstadoJuego.Jugando;

    public static CarrerasGameManager instancia;

    void Awake()
    {
        if (instancia == null) instancia = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (Ximena.Sonido.SFXManager.instancia != null)
        {
            Ximena.Sonido.SFXManager.instancia.PistaMusica();
        }
        else
        {
            Debug.LogWarning("CarrerasGameManager: SFXManager.instancia es null en Start. Asegúrate de que el SFXManager esté presente en la escena.");
        }
    }


    public void JugadorGano()
    {
        if (estadoActual != EstadoJuego.Jugando) return;
        estadoActual = EstadoJuego.Victoria;

        Debug.Log("¡VICTORIA!");


        MovimientoNave nave = FindObjectOfType<MovimientoNave>();
        if (nave != null) nave.enabled = false;


        TimerManager timerManager = FindObjectOfType<TimerManager>();
        float tiempoTranscurrido = 0f;
        float tiempoRestante = 0f;

        if (timerManager != null)
        {
            tiempoTranscurrido = timerManager.ObtenerTiempoTranscurrido();
            tiempoRestante     = timerManager.tiempoRestante;
            timerManager.DetenerTimer();
        }

    
        int puntosTotales = 0;
        int tokens = 0;

        if (PuntosManager.instancia != null)
        {
            PuntosManager.instancia.AgregarPuntosPorVictoria(tiempoRestante);
            tokens        = PuntosManager.instancia.CalcularTokens();
            puntosTotales = PuntosManager.instancia.ObtenerPuntos();
        }

        if (SFXManager.instancia != null)
            SFXManager.instancia.Victoria();

        GuardarMejorTiempo(tiempoTranscurrido);

        PlayerPrefs.SetInt("PuntosFinales",  puntosTotales);
        PlayerPrefs.SetInt("TokensGanados",  tokens);
        PlayerPrefs.SetFloat("TiempoFinal",  tiempoTranscurrido);
        PlayerPrefs.Save();

        Debug.Log("Puntos: " + puntosTotales + " | Tokens: " + tokens +
                " | Tiempo: " + tiempoTranscurrido);
        
        int tiempoEntero = Mathf.RoundToInt(tiempoTranscurrido);

        if (ConectorAPI.instancia != null)
        {
            ConectorAPI.instancia.GuardarTiempo(tiempoEntero, (exito) =>
            {
                if (exito)
                    Debug.Log(" Tiempo enviado a la API: " + tiempoEntero + "s");
                else
                    Debug.LogWarning(" No se pudo guardar en la API.");
            });
        }

        Invoke("CargarVictoria", 2f);
        
    }


    public void JugadorPerdioTiempo()
    {
        Debug.Log("GameManager: JugadorPerdioTiempo() recibido. Estado: " + estadoActual);

        if (estadoActual != EstadoJuego.Jugando) return;

        PlayerPrefs.SetInt("RazonDerrota", 1);
        ProcesarDerrota();
    }

    public void JugadorPerdioSinVidas()
    {
        Debug.Log("GameManager: JugadorPerdioSinVidas() recibido. Estado: " + estadoActual);

        if (estadoActual != EstadoJuego.Jugando) return;

        PlayerPrefs.SetInt("RazonDerrota", 0);
        ProcesarDerrota();
    }

    void ProcesarDerrota()
    {
        estadoActual = EstadoJuego.Derrota;
        Debug.Log("GameManager: Procesando derrota...");

        MovimientoNave nave = FindObjectOfType<MovimientoNave>();
        if (nave != null) nave.enabled = false;

        if (PuntosManager.instancia != null)
        {
            PlayerPrefs.SetInt("PuntosFinales",
                            PuntosManager.instancia.ObtenerPuntos());
        }

        if (SFXManager.instancia != null)
            SFXManager.instancia.GameOver();

        PlayerPrefs.Save();

        Debug.Log("Cargando escena Derrota en 2 segundos...");
        Invoke("CargarDerrota", 2f);
    }

    
    void GuardarMejorTiempo(float tiempoActual)
    {
        float mejorTiempo = PlayerPrefs.GetFloat("MejorTiempo", 0f);

        if (mejorTiempo <= 0f || tiempoActual < mejorTiempo)
        {
            PlayerPrefs.SetFloat("MejorTiempo", tiempoActual);
            PlayerPrefs.Save();
            Debug.Log("¡Nuevo mejor tiempo! " + tiempoActual + "s");
        }
        else
        {
            Debug.Log("Mejor tiempo anterior: " + mejorTiempo +
                    "s | Tiempo actual: " + tiempoActual + "s");
        }
    }


    void CargarVictoria()
    {
        SceneManager.LoadScene("Victoria");
    }

    void CargarDerrota()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Derrota");
    }
}
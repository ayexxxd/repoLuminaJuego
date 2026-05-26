using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum EstadoJuego { Jugando, Victoria, Derrota }
    public EstadoJuego estadoActual = EstadoJuego.Jugando;

    public static GameManager instancia;

    void Awake()
    {
        if (instancia == null) instancia = this;
        else Destroy(gameObject);
    }

    // ============================================================
    // VICTORIA
    // ============================================================

    public void JugadorGano()
    {
        if (estadoActual != EstadoJuego.Jugando) return;
        estadoActual = EstadoJuego.Victoria;

        Debug.Log("¡VICTORIA!");

        // Detenemos la nave
        MovimientoNave nave = FindObjectOfType<MovimientoNave>();
        if (nave != null) nave.enabled = false;

        // Obtenemos el tiempo transcurrido desde el TimerManager
        TimerManager timerManager = FindObjectOfType<TimerManager>();
        float tiempoTranscurrido = 0f;
        float tiempoRestante = 0f;

        if (timerManager != null)
        {
            tiempoTranscurrido = timerManager.ObtenerTiempoTranscurrido();
            tiempoRestante     = timerManager.tiempoRestante;
            timerManager.DetenerTimer();
        }

        // Calculamos puntos y tokens
        int puntosTotales = 0;
        int tokens = 0;

        if (PuntosManager.instancia != null)
        {
            PuntosManager.instancia.AgregarPuntosPorVictoria(tiempoRestante);
            tokens        = PuntosManager.instancia.CalcularTokens();
            puntosTotales = PuntosManager.instancia.ObtenerPuntos();
        }

        // Comparamos y guardamos el mejor tiempo
        GuardarMejorTiempo(tiempoTranscurrido);

        // Guardamos todo en PlayerPrefs para la pantalla de victoria
        PlayerPrefs.SetInt("PuntosFinales",  puntosTotales);
        PlayerPrefs.SetInt("TokensGanados",  tokens);
        PlayerPrefs.SetFloat("TiempoFinal",  tiempoTranscurrido);
        PlayerPrefs.Save();

        Debug.Log("Puntos: " + puntosTotales + " | Tokens: " + tokens +
                " | Tiempo: " + tiempoTranscurrido);

        // Cargamos la pantalla de victoria después de 2 segundos
        Invoke("CargarVictoria", 2f);
    }

    // ============================================================
    // DERROTA — por tiempo agotado
    // ============================================================

    // ---- Llamado cuando se acaba el tiempo ----
    public void JugadorPerdioTiempo()
    {
        Debug.Log("GameManager: JugadorPerdioTiempo() recibido. Estado: " + estadoActual);

        if (estadoActual != EstadoJuego.Jugando) return;

        PlayerPrefs.SetInt("RazonDerrota", 1);
        ProcesarDerrota();
    }

    // ---- Llamado cuando se acaban las vidas ----
    public void JugadorPerdioSinVidas()
    {
        Debug.Log("GameManager: JugadorPerdioSinVidas() recibido. Estado: " + estadoActual);

        if (estadoActual != EstadoJuego.Jugando) return;

        PlayerPrefs.SetInt("RazonDerrota", 0);
        ProcesarDerrota();
    }

    // ---- Lógica común de derrota ----
    void ProcesarDerrota()
    {
        estadoActual = EstadoJuego.Derrota;
        Debug.Log("GameManager: Procesando derrota...");

        // Detenemos la nave
        MovimientoNave nave = FindObjectOfType<MovimientoNave>();
        if (nave != null) nave.enabled = false;

        // Guardamos los puntos actuales
        if (PuntosManager.instancia != null)
        {
            PlayerPrefs.SetInt("PuntosFinales",
                            PuntosManager.instancia.ObtenerPuntos());
        }

        PlayerPrefs.Save();

        Debug.Log("Cargando escena Derrota en 2 segundos...");
        Invoke("CargarDerrota", 2f);
    }

    
    // ============================================================
    // MEJOR TIEMPO
    // ============================================================

    // ---- Compara el tiempo actual con el mejor guardado ----
    // Si es mejor (más rápido), lo guarda
    void GuardarMejorTiempo(float tiempoActual)
    {
        float mejorTiempo = PlayerPrefs.GetFloat("MejorTiempo", 0f);

        // Si no hay tiempo guardado (0) o el nuevo es más rápido → guardamos
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

    // ============================================================
    // MÉTODOS DE CARGA DE ESCENAS
    // ============================================================

    void CargarVictoria()
    {
        SceneManager.LoadScene("Victoria");
    }

    void CargarDerrota()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Derrota");
    }
}
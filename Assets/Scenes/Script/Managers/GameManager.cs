using UnityEngine;
using UnityEngine.SceneManagement;

// GameManager controla el estado general del juego
// Sabe si el jugador está jugando, ganó o perdió
// Vive en un GameObject vacío en la escena
public class GameManager : MonoBehaviour
{
    // Estado posibles del juego
    public enum EstadoJuego
    {
        Jugando,
        Victoria,
        Derrota
    }

    // Estado actual — empieza en Jugando
    public EstadoJuego estadoActual = EstadoJuego.Jugando;

    // Para que solo exista un GameManager en toda la escena
    public static GameManager instancia;

    void Awake()
    {
        // Patrón Singleton: nos aseguramos de que solo haya uno
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ---- Llamado cuando el tiempo se agota ----
    // Lo conectaremos al evento onTiempoAgotado del TimerManager
    public void JugadorPerdio()
    {
        // Solo reaccionamos si el juego aún está en curso
        if (estadoActual != EstadoJuego.Jugando) return;

        estadoActual = EstadoJuego.Derrota;
        Debug.Log("GAME OVER - El jugador perdió.");

        // Desactivamos el movimiento de la nave
        MovimientoNave nave = Object.FindAnyObjectByType<MovimientoNave>();
        if (nave != null)
        {
            nave.enabled = false;
        }

        // Por ahora solo mostramos en consola
        // En la Etapa 13 cargaremos la pantalla de derrota real
        Debug.Log("Cargando pantalla de derrota...");

        // Esperamos 2 segundos y recargamos la escena por ahora
        Invoke("RecargarEscena", 2f);
    }

    // ---- Llamado cuando el jugador completa todas las vueltas ----
    // Lo conectaremos al evento onJugadorGano del LapManager
    public void JugadorGano()
    {
        if (estadoActual != EstadoJuego.Jugando) return;

        estadoActual = EstadoJuego.Victoria;
        Debug.Log("¡VICTORIA!");

        // Detenemos la nave
        MovimientoNave nave = FindObjectOfType<MovimientoNave>();
        if (nave != null) nave.enabled = false;

        // Obtenemos el tiempo restante para el bonus
         // Obtenemos el tiempo restante para el bonus
        TimerManager timerManager = FindObjectOfType<TimerManager>();
        float tiempoRestante = 0f;

        if (timerManager != null)
        {
            tiempoRestante = timerManager.tiempoRestante;
        }
        

        // Buscamos el UIManager
        UIManager uiManager = FindObjectOfType<UIManager>();
        
        // Agregamos puntos por victoria y por tiempo restante
        if (PuntosManager.instancia != null)
        {
            PuntosManager.instancia.AgregarPuntosPorVictoria(tiempoRestante);

            // Calculamos los tokens generados
            int tokens = PuntosManager.instancia.CalcularTokens();
            int puntosTotales = PuntosManager.instancia.ObtenerPuntos();

            Debug.Log("=== RESUMEN DE PARTIDA ===");
            Debug.Log("Puntos totales: " + puntosTotales);
            Debug.Log("Tokens ganados: " + tokens);

            // Guardamos los tokens para enviarlos a la API
            // Esto lo usaremos en la Etapa 14
            PlayerPrefs.SetInt("TokensGanados", tokens);
            PlayerPrefs.SetInt("PuntosFinales", puntosTotales);
            PlayerPrefs.SetFloat("TiempoFinal", timerManager != null ?
                                timerManager.ObtenerTiempoTranscurrido() : 0f);
            PlayerPrefs.Save();
        }
        

        // Mostramos el resumen en UI
        uiManager?.MostrarMensajeTemporal("¡VICTORIA! 🏆", 3f);

        Invoke("RecargarEscena", 3f);
    }

    void RecargarEscena()
    {
        // Recarga la escena actual — útil para pruebas
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
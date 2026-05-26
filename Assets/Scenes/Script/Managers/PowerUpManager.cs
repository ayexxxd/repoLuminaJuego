using UnityEngine;

// PowerUpManager centraliza todos los efectos de power-ups
// Decide qué efecto aplicar y lo ejecuta
// Vive en un GameObject vacío en la escena
public class PowerUpManager : MonoBehaviour
{
    [Header("Configuración de efectos")]

    // ---- Boost de velocidad ----
    // Cuánto multiplica la velocidad durante el boost
    public float multiplicadorBoost = 2f;
    // Cuántos segundos dura el boost
    public float duracionBoost = 5f;

    // ---- Tiempo extra ----
    // Cuántos segundos agrega al timer
    public float tiempoExtra = 10f;

    // ---- Probabilidades de cada efecto aleatorio (deben sumar 100) ----
    [Range(0, 100)]
    public int probabilidadVelocidad = 35;
    [Range(0, 100)]
    public int probabilidadTiempoExtra = 25;
    [Range(0, 100)]
    public int probabilidadVidaExtra = 25;
    [Range(0, 100)]
    public int probabilidadQuitarVida = 15;

    // ---- Referencias a otros managers ----
    private VidasManager vidasManager;
    private TimerManager timerManager;

    // Referencia a la nave para aplicar efectos de velocidad
    private MovimientoNave nave;

    // Singleton para acceso fácil desde otros scripts
    public static PowerUpManager instancia;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); 
            Debug.Log("PowerUpManager: Inicializado y protegido.");
        }
        else if (instancia != this)
        {
            Destroy(gameObject);
            Debug.Log("PowerUpManager: Duplicado eliminado.");
        }
    }

    void Start()
    {
        // Buscamos todas las referencias que necesitamos
        vidasManager = FindObjectOfType<VidasManager>();
        timerManager = FindObjectOfType<TimerManager>();
        nave = FindObjectOfType<MovimientoNave>();

        // Verificamos que las probabilidades sumen 100
        int suma = probabilidadVelocidad + probabilidadTiempoExtra +
                probabilidadVidaExtra + probabilidadQuitarVida;

        if (suma != 100)
        {
            Debug.LogWarning("PowerUpManager: Las probabilidades suman " + suma +
                        " en lugar de 100. Ajústalas en el Inspector.");
        }
    }

    // ============================================================
    // EFECTOS DEL BOOST (zona fija en la pista)
    // ============================================================

    // ---- Aplica el efecto de boost de velocidad ----
    // Llamado por las zonas boost de la pista
    public void AplicarBoost()
    {
        if (nave == null) return;

        nave.AplicarEfectoVelocidad(multiplicadorBoost, duracionBoost);

        // Avisamos al UIManager
        UIManager uiManager = FindObjectOfType<UIManager>();
        uiManager?.MostrarMensajeTemporal("¡IMPULSO!", 1.5f);

        
        Debug.Log("Boost aplicado: x" + multiplicadorBoost + " por " + duracionBoost + "s");
        // Damos puntos por recoger la estrella
        PuntosManager.instancia?.AgregarPuntosPorEstrella();
        
    
    }

    // ============================================================
    // EFECTOS DE LA ESTRELLA (aleatorio)
    // ============================================================

    // ---- Decide y aplica un efecto aleatorio ----
    // Llamado por las estrellas cuando la nave las recoge
    public void AplicarEfectoAleatorio()
    {
        // Generamos un número aleatorio entre 0 y 99
        int aleatorio = Random.Range(0, 100);

        // Determinamos qué efecto toca según las probabilidades
        // Funciona como rangos acumulados:
        // 0-34    = velocidad (35%)
        // 35-59   = tiempo extra (25%)
        // 60-84   = vida extra (25%)
        // 85-99   = quitar vida (15%)

        if (aleatorio < probabilidadVelocidad)
        {
            AplicarEfectoVelocidadEstrella();
        }
        else if (aleatorio < probabilidadVelocidad + probabilidadTiempoExtra)
        {
            AplicarEfectoTiempoExtra();
        }
        else if (aleatorio < probabilidadVelocidad + probabilidadTiempoExtra + probabilidadVidaExtra)
        {
            AplicarEfectoVidaExtra();
        }
        else
        {
            AplicarEfectoQuitarVida();
        }
        PuntosManager.instancia?.AgregarPuntosPorEstrella();
    }

    // ---- Efecto: Velocidad temporal ----
    void AplicarEfectoVelocidadEstrella()
    {
        if (nave == null) return;

        nave.AplicarEfectoVelocidad(multiplicadorBoost, duracionBoost);

        UIManager uiManager = FindObjectOfType<UIManager>();
        uiManager?.MostrarMensajeTemporal(" ¡Velocidad!", 2f);

        Debug.Log("Estrella: efecto VELOCIDAD aplicado.");
        // Damos puntos por recoger la estrella
        PuntosManager.instancia?.AgregarPuntosPorEstrella();
    }

    // ---- Efecto: Tiempo extra ----
    void AplicarEfectoTiempoExtra()
    {
        if (timerManager == null) return;

        timerManager.AgregarTiempo(tiempoExtra);

        UIManager uiManager = FindObjectOfType<UIManager>();
        uiManager?.MostrarMensajeTemporal(" +" + tiempoExtra + "s", 2f);

        Debug.Log("Estrella: efecto TIEMPO EXTRA aplicado. +" + tiempoExtra + "s");
        PuntosManager.instancia?.AgregarPuntosPorEstrella();
    }

    // ---- Efecto: Vida extra ----
    void AplicarEfectoVidaExtra()
    {
        if (vidasManager == null) return;

        vidasManager.AgregarVida();

        UIManager uiManager = FindObjectOfType<UIManager>();
        uiManager?.MostrarMensajeTemporal(" ¡Vida extra!", 2f);

        Debug.Log("Estrella: efecto VIDA EXTRA aplicado.");
        PuntosManager.instancia?.AgregarPuntosPorEstrella();
    }

    // ---- Efecto: Quitar vida ----
    void AplicarEfectoQuitarVida()
    {
        if (vidasManager == null) return;

        vidasManager.QuitarVida();

        UIManager uiManager = FindObjectOfType<UIManager>();
        uiManager?.MostrarMensajeTemporal(" ¡Mala suerte!", 2f);

        Debug.Log("Estrella: efecto QUITAR VIDA aplicado.");
        PuntosManager.instancia?.AgregarPuntosPorEstrella();
    }
}
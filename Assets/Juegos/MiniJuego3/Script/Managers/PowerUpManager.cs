using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [Header("Configuración de efectos")]



    public float multiplicadorBoost = 2f;

    public float duracionBoost = 5f;



    public float tiempoExtra = 10f;


    [Range(0, 100)]
    public int probabilidadVelocidad = 35;
    [Range(0, 100)]
    public int probabilidadTiempoExtra = 25;
    [Range(0, 100)]
    public int probabilidadVidaExtra = 25;
    [Range(0, 100)]
    public int probabilidadQuitarVida = 15;


    private VidasManager vidasManager;
    private TimerManager timerManager;


    private MovimientoNave nave;


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


        int suma = probabilidadVelocidad + probabilidadTiempoExtra +
                probabilidadVidaExtra + probabilidadQuitarVida;

        if (suma != 100)
        {
            Debug.LogWarning("PowerUpManager: Las probabilidades suman " + suma +
                        " en lugar de 100. Ajústalas en el Inspector.");
        }
    }

    public void AplicarBoost()
    {
        if (nave == null) return;

        nave.AplicarEfectoVelocidad(multiplicadorBoost, duracionBoost);


        UIManager uiManager = FindObjectOfType<UIManager>();
        uiManager?.MostrarMensajeTemporal("¡IMPULSO!", 1.5f);

        
        Debug.Log("Boost aplicado: x" + multiplicadorBoost + " por " + duracionBoost + "s");

        PuntosManager.instancia?.AgregarPuntosPorEstrella();
        
    
    }

    public void AplicarEfectoAleatorio()
    {
        int aleatorio = Random.Range(0, 100);


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

    void AplicarEfectoVelocidadEstrella()
    {
        if (nave == null) return;

        nave.AplicarEfectoVelocidad(multiplicadorBoost, duracionBoost);

        UIManager uiManager = FindObjectOfType<UIManager>();
        uiManager?.MostrarMensajeTemporal(" ¡Velocidad!", 2f);

        Debug.Log("Estrella: efecto VELOCIDAD aplicado.");

        PuntosManager.instancia?.AgregarPuntosPorEstrella();
    }


    void AplicarEfectoTiempoExtra()
    {
        if (timerManager == null) return;

        timerManager.AgregarTiempo(tiempoExtra);

        UIManager uiManager = FindObjectOfType<UIManager>();
        uiManager?.MostrarMensajeTemporal(" +" + tiempoExtra + "s", 2f);

        Debug.Log("Estrella: efecto TIEMPO EXTRA aplicado. +" + tiempoExtra + "s");
        PuntosManager.instancia?.AgregarPuntosPorEstrella();
    }

    void AplicarEfectoVidaExtra()
    {
        if (vidasManager == null) return;

        vidasManager.AgregarVida();

        UIManager uiManager = FindObjectOfType<UIManager>();
        uiManager?.MostrarMensajeTemporal(" ¡Vida extra!", 2f);

        Debug.Log("Estrella: efecto VIDA EXTRA aplicado.");
        PuntosManager.instancia?.AgregarPuntosPorEstrella();
    }
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
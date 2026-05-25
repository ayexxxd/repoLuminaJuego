// GameManager.cs
// Controla movimientos, puntos y estado general del juego.

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ── Singleton ─────────────────────────────────────────
    public static GameManager instancia;

    // ── Movimientos ───────────────────────────────────────
    [Header("Movimientos")]
    public int movimientosIniciales = 20;
    private int movimientosRestantes;

    // ── Puntos ────────────────────────────────────────────
    [Header("Puntos")]
    private int puntosActuales   = 0;  // Puntos del nivel actual
    private int puntosTotales    = 0;  // Acumulado de todos los niveles
    [HideInInspector] public int puntosGastados = 0; // Gastados en tienda este nivel


    // ── UI ────────────────────────────────────────────────
    [Header("UI")]
    public TextMeshProUGUI textoMovimientos;
    public TextMeshProUGUI textoPuntos;
    public TextMeshProUGUI textoNivel;   // ← NUEVO — arrastra TextoNivel aquí
    public TextMeshProUGUI textoMeta;  

    [Header("Niveles")]
    public int nivelActual = 1;
    public int nivelMaximo = 3;

    // Puntos necesarios por nivel
    private int[] metasPorNivel = { 1500, 3000, 6000 };

    // Puntos necesarios para el nivel actual
    private int puntosNecesarios;

    // ─────────────────────────────────────────────────────
    void Awake()
    {
        if (instancia == null) instancia = this;
    }

    void Start()
    {
        // Lee si venimos de ResultadoScene
        // GetInt devuelve 0 si la clave no existe nunca
        int nivelGuardado = PlayerPrefs.GetInt("NivelActual", 0);

        if (nivelGuardado > 0)
        {
            // Venimos de ResultadoScene, usamos el nivel guardado
            nivelActual = nivelGuardado;

            // Borramos la clave para que la próxima vez empiece limpio
            PlayerPrefs.DeleteKey("NivelActual");
            PlayerPrefs.Save();

            Debug.Log("Nivel cargado desde PlayerPrefs: " + nivelActual);
        }
        else
        {
            // Inicio limpio desde el editor o primer arranque
            nivelActual = 1;
            Debug.Log("Inicio limpio — nivel 1");
        }

        movimientosRestantes = movimientosIniciales;
        InicializarNivel();
        ActualizarUI();
    }

    // ── Movimientos ───────────────────────────────────────

    // Resta 1 movimiento tras un intercambio válido
    public void UsarMovimiento()
    {
        movimientosRestantes--;
        ActualizarUI();

        if (movimientosRestantes <= 0)
            AlQuedarSinMovimientos();
    }

    // Agrega movimientos (preguntas, power-ups)
    public void AgregarMovimientos(int cantidad)
    {
        movimientosRestantes += cantidad;
        ActualizarUI();
    }

    void AlQuedarSinMovimientos()
    {
        VerificarFinDeNivel();
    }

    // ── Puntos ────────────────────────────────────────────

    public void AgregarPuntos(int cantidad)
    {
        puntosActuales += cantidad;
        puntosTotales  += cantidad; // Acumulado global
        ActualizarUI();

        // 🔥 VITAL: Si no tienes esto, el juego nunca cambiará de escena al ganar en tiempo real
        if (puntosActuales >= puntosNecesarios)
        {
            VerificarFinDeNivel();
        }
    }

    // Descuenta puntos para la tienda. Devuelve false si no alcanza.
    public bool GastarPuntos(int cantidad)
    {
        if (puntosActuales < cantidad) return false;
        puntosActuales  -= cantidad;
        puntosGastados  += cantidad;
        ActualizarUI();
        return true;
    }

    public int ObtenerPuntos() => puntosActuales;

    // ── UI ────────────────────────────────────────────────
    void ActualizarUI()
    {
        if (textoMovimientos != null)
            textoMovimientos.text = "Movimientos: " + movimientosRestantes;

        if (textoPuntos != null)
            textoPuntos.text = "Puntos: " + puntosActuales;

        // ── NUEVO ──────────────────────────────────────────
        if (textoNivel != null)
            textoNivel.text = "Nivel: " + nivelActual;

        if (textoMeta != null)
            textoMeta.text = "Meta: " + puntosActuales + " / " + puntosNecesarios;
        // ───────────────────────────────────────────────────
    }

    // ── Inicializa configuración del nivel actual ─────────
    public void InicializarNivel()
    {
        int indice = Mathf.Clamp(nivelActual - 1, 0, metasPorNivel.Length - 1);
        puntosNecesarios = metasPorNivel[indice];

        movimientosRestantes = movimientosIniciales;

        // Los puntos del nivel se reinician
        // Los puntos totales NO se reinician, se acumulan
        puntosActuales = 0;
        puntosGastados = 0;

        // Activa monstruos según nivel
        if (Board.instancia != null)
        {
            Board.instancia.monstruoRojoActivo  = nivelActual >= 2;
            Board.instancia.monstruoVerdeActivo = nivelActual >= 3;

            if (nivelActual == 1) Board.instancia.tiposActivosEnNivel = 4;
            if (nivelActual == 2) Board.instancia.tiposActivosEnNivel = 5;
            if (nivelActual == 3) Board.instancia.tiposActivosEnNivel = 6;
        }

        ActualizarUI();
        Debug.Log($"Nivel {nivelActual} — Meta: {puntosNecesarios}");
        
    }

    // ── Verifica si ganó o perdió al quedarse sin movimientos
    public void VerificarFinDeNivel()
    {
        bool gano = puntosActuales >= puntosNecesarios;

        Debug.Log($"Fin nivel {nivelActual} — " +
                $"Puntos: {puntosActuales}/{puntosNecesarios} — " +
                $"Ganó: {gano}");

        // Guarda datos para ResultadoScene
        PlayerPrefs.SetInt("Gano",           gano ? 1 : 0);
        PlayerPrefs.SetInt("PuntosNivel",    puntosActuales);   // puntos de ESTE nivel
        PlayerPrefs.SetInt("PuntosTotales",  puntosTotales);    // acumulado global
        PlayerPrefs.SetInt("PuntosGastados", puntosGastados);
        PlayerPrefs.SetInt("NivelActual",    nivelActual);
        PlayerPrefs.SetInt("EsUltimoNivel",  nivelActual >= nivelMaximo ? 1 : 0);
        PlayerPrefs.Save();

        SceneManager.LoadScene("FinalEscena");
    }

    // ── Avanza al siguiente nivel ─────────────────────────
    public void SiguienteNivel()
    {
        if (nivelActual >= nivelMaximo)
        {
            Debug.Log("¡Completaste todos los niveles!");
            return;
        }

        nivelActual++;
        InicializarNivel();
        Board.instancia.ReiniciarTablero();
    }

    // ── Reintenta el nivel actual ─────────────────────────
    public void ReintentarNivel()
    {
        InicializarNivel();
        Board.instancia.ReiniciarTablero();
    }

    // Getter para que otros scripts lean puntosNecesarios   
    public int ObtenerPuntosNivel()   => puntosActuales;
    public int ObtenerPuntosTotales() => puntosTotales;
    public int ObtenerPuntosNecesarios() => puntosNecesarios;

    
}
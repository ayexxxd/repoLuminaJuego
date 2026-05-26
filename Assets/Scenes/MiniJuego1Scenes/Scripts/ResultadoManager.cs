// ResultadoManager.cs
// Lee PlayerPrefs y muestra el panel correcto en ResultadoScene.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultadoManager : MonoBehaviour
{
    // ── Paneles principales ───────────────────────────────
    [Header("Paneles Resultado")]
    public GameObject panelGanaste;
    public GameObject panelPerdiste;

    // ── Panel de tienda ───────────────────────────────────
    [Header("Panel Tienda")]
    public GameObject      panelTienda;
    public TextMeshProUGUI textoPuntosEnTienda;  // puntos disponibles
    public TextMeshProUGUI textoMensajeCompra;   // "No tienes suficientes puntos"

    // ── Textos panel Ganaste ──────────────────────────────
    [Header("Textos Ganaste")]
    public TextMeshProUGUI textoResumenGanaste;

    // ── Textos panel Perdiste ─────────────────────────────
    [Header("Textos Perdiste")]
    public TextMeshProUGUI textoResumenPerdiste;

    // ── Textos panel final nivel 3 ────────────────────────
    [Header("Textos Nivel 3 Final")]
    public GameObject      panelFinal;          // solo aparece en nivel 3
    public TextMeshProUGUI textoResumenFinal;

    // ── Costos de la tienda ───────────────────────────────
    [Header("Costos Tienda")]
    public int costoMartillo    = 1000;
    public int costoShuffle     = 800;
    public int costoMovimientos = 500;

    // ── Datos leídos de PlayerPrefs ───────────────────────
    private int gano            = 0;
    private int puntosNivel     = 0;   // puntos de este nivel
    private int puntosTotales   = 0;   // acumulado global
    private int puntosGastados  = 0;
    private int nivelActual     = 1;
    private int nivelMaximo     = 3;
    private int esUltimoNivel   = 0;

    // ── Puntos disponibles para gastar en tienda ──────────
    private int puntosDisponibles = 0;

    // ─────────────────────────────────────────────────────
    void Start()
    {
        // Oculta todo al inicio
        panelGanaste.SetActive(false);
        panelPerdiste.SetActive(false);
        panelTienda.SetActive(false);

        if (panelFinal != null)
            panelFinal.SetActive(false);

        LeerDatos();
        MostrarPanelCorrecto();
    }

    void LeerDatos()
    {
        gano           = PlayerPrefs.GetInt("Gano",          0);
        puntosNivel    = PlayerPrefs.GetInt("PuntosNivel",   0);
        puntosTotales  = PlayerPrefs.GetInt("PuntosTotales", 0);
        puntosGastados = PlayerPrefs.GetInt("PuntosGastados",0);
        nivelActual    = PlayerPrefs.GetInt("NivelActual",   1);
        esUltimoNivel  = PlayerPrefs.GetInt("EsUltimoNivel", 0);

        // Los puntos disponibles para la tienda son los de este nivel
        // menos lo que ya gastó durante el juego
        puntosDisponibles = Mathf.Max(0, puntosNivel - puntosGastados);

        Debug.Log($"Resultado — Ganó:{gano} " +
                $"PuntosNivel:{puntosNivel} " +
                $"Disponibles:{puntosDisponibles}");
    }

    // ── Rellena los textos del panel de victoria ──────────
    void MostrarPanelCorrecto()
    {
        // Oculta el botón de tienda si es nivel 1
        OcultarTiendaSiNivel1();

        if (gano == 1)
        {
            panelGanaste.SetActive(true);
            MostrarResumenGanaste();
        }
        else
        {
            panelPerdiste.SetActive(true);
            MostrarResumenPerdiste();
        }
    }

    void OcultarTiendaSiNivel1()
    {
        if (nivelActual > 1) return;

        // Busca y oculta los botones de abrir tienda
        GameObject TiendaGBtn  = GameObject.Find("TiendaGBtn");
        GameObject TiendaPBtn = GameObject.Find("TiendaPBtn");

        if (TiendaGBtn  != null) TiendaGBtn.SetActive(false);
        if (TiendaPBtn != null) TiendaPBtn.SetActive(false);

        Debug.Log("Nivel 1 — botones de tienda ocultos");
    }

    void MostrarResumenGanaste()
    {
        if (textoResumenGanaste == null) return;

        textoResumenGanaste.text =
            "¡Ganaste el nivel " + nivelActual + "!\n" +
            "Puntos obtenidos: " + puntosNivel + "\n" +
            "Disponibles para tienda: " + puntosDisponibles;
    }

    void MostrarResumenPerdiste()
    {
        if (textoResumenPerdiste == null) return;

        textoResumenPerdiste.text =
            "Puntos obtenidos: " + puntosNivel + "\n" +
            "Disponibles para tienda: " + puntosDisponibles;
    }

    // ─────────────────────────────────────────────────────
    // TIENDA
    // ─────────────────────────────────────────────────────

    // ── Botón "Abrir Tienda" (en panel Ganaste o Perdiste)
    public void AlAbrirTienda()
    {
        // Oculta el panel actual sin importar el nivel
        panelGanaste.SetActive(false);
        panelPerdiste.SetActive(false);

        ActualizarTextoPuntosEnTienda();

        if (textoMensajeCompra != null)
            textoMensajeCompra.text = "";

        // Siempre abre la tienda — el botón Continuar decide qué sigue
        panelTienda.SetActive(true);
    }
    // Si es nivel 1, va directo sin pasar por tienda
    void ProcesarSinTienda()
    {
        if (gano == 1)
            ProcesarVictoria();
        else
            ProcesarDerrota();
    }

    // ── Actualiza el texto de puntos dentro de la tienda ──
    void ActualizarTextoPuntosEnTienda()
    {
        if (textoPuntosEnTienda != null)
            textoPuntosEnTienda.text = "Puntos disponibles: " + puntosDisponibles;
    }

    // ── Comprar Martillo ──────────────────────────────────
    public void ComprarMartillo()
    {
        if (!IntentarGastar(costoMartillo)) return;

        InventarioManager.instancia.AgregarMartillo();
        Debug.Log("Compró Martillo");
    }

    // ── Comprar Shuffle ───────────────────────────────────
    public void ComprarShuffle()
    {
        if (!IntentarGastar(costoShuffle)) return;

        InventarioManager.instancia.AgregarShuffle();
        Debug.Log("Compró Shuffle");
    }

    // ── Comprar +3 Movimientos ────────────────────────────
    public void ComprarMovimientos()
    {
        if (!IntentarGastar(costoMovimientos)) return;

        InventarioManager.instancia.AgregarMovExtras();
        Debug.Log("Compró +3 Movimientos");
    }

    // ── Descuenta puntos — devuelve false si no alcanza ───
    bool IntentarGastar(int costo)
    {
        if (puntosDisponibles < costo)
        {
            if (textoMensajeCompra != null)
                textoMensajeCompra.text = "No tienes suficientes puntos";

            Debug.Log($"Puntos insuficientes — necesita:{costo} tiene:{puntosDisponibles}");
            return false;
        }

        puntosDisponibles -= costo;
        ActualizarTextoPuntosEnTienda();

        if (textoMensajeCompra != null)
            textoMensajeCompra.text = "¡Comprado!";

        return true;
    }

    // ─────────────────────────────────────────────────────
    // BOTONES DE NAVEGACIÓN
    // ─────────────────────────────────────────────────────

    // ── Botón "Continuar" dentro de la tienda ─────────────
    public void AlCerrarTienda()
    {
        panelTienda.SetActive(false);

        if (gano == 1)
            ProcesarVictoria();
        else
            ProcesarDerrota();
    }

    // ── Si ganó: avanza o muestra panel final ─────────────
    void ProcesarVictoria()
    {
        if (esUltimoNivel == 1)
        {
            MostrarPanelFinal();
        }
        else
        {
            AlPresionarSiguienteNivel();
        }
    }

    // ── Si perdió: vuelve a reintentar ────────────────────
    void ProcesarDerrota()
    {
        AlPresionarReintentar();
    }

    // ── Panel final solo al completar nivel 3 ─────────────
    void MostrarPanelFinal()
    {
        if (panelFinal == null) return;

        int puntosRestantes = Mathf.Max(0, puntosTotales);
        int tokens          = puntosRestantes / 100;

        if (textoResumenFinal != null)
            textoResumenFinal.text =
                "¡Completaste todos los niveles!\n\n" +
                "Puntos totales: "   + puntosTotales  + "\n" +
                "Tokens ganados: "   + tokens         + " (cada 100 pts = 1 token)";

        // Guarda tokens
        int tokensActuales = PlayerPrefs.GetInt("Tokens", 0);
        PlayerPrefs.SetInt("Tokens", tokensActuales + tokens);
        PlayerPrefs.Save();

        panelFinal.SetActive(true);
        Debug.Log($"Juego completado — Tokens ganados: {tokens}");
    }

    // ── Botón "Siguiente Nivel" ───────────────────────────
    public void AlPresionarSiguienteNivel()
    {
        int siguienteNivel = nivelActual >= nivelMaximo ? 1 : nivelActual + 1;
        PlayerPrefs.SetInt("NivelActual", siguienteNivel);
        PlayerPrefs.Save();

        Debug.Log("Cargando nivel: " + siguienteNivel);
        SceneManager.LoadScene("EscenadeJuego");
    }

    // ── Botón "Reintentar" ────────────────────────────────
    public void AlPresionarReintentar()
    {
        PlayerPrefs.SetInt("NivelActual", nivelActual);
        PlayerPrefs.Save();

        Debug.Log("Reintentando nivel: " + nivelActual);
        SceneManager.LoadScene("EscenadeJuego");
    }

    // ── Botón "Jugar de nuevo" en panel final ─────────────
    public void AlPresionarJugarDeNuevo()
    {
        PlayerPrefs.SetInt("NivelActual", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("EscenadeJuego");
    }

}
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultadoManager : MonoBehaviour
{
    [Header("Paneles Resultado")]
    public GameObject panelGanaste;
    public GameObject panelPerdiste;

    [Header("Panel Tienda")]
    public GameObject panelTienda;
    public TextMeshProUGUI textoPuntosEnTienda;
    public TextMeshProUGUI textoMensajeCompra;

    [Header("Textos Ganaste")]
    public TextMeshProUGUI textoResumenGanaste;

    [Header("Textos Perdiste")]
    public TextMeshProUGUI textoResumenPerdiste;

    [Header("Textos Nivel 3 Final")]
    public GameObject panelFinal;
    public TextMeshProUGUI textoResumenFinal;

    [Header("Costos Tienda")]
    public int costoMartillo = 1000;
    public int costoShuffle = 800;
    public int costoMovimientos = 500;

    private int gano = 0;
    private int puntosNivel = 0;
    private int puntosTotales = 0;
    private int puntosGastados = 0;
    private int nivelActual = 1;
    private int nivelMaximo = 3;
    private int esUltimoNivel = 0;

    private int puntosDisponibles = 0;

    void Start()
    {
        panelGanaste.SetActive(false);
        panelPerdiste.SetActive(false);
        panelTienda.SetActive(false);

        if (panelFinal != null)
            panelFinal.SetActive(false);

        LeerDatos();
        MostrarPanelCorrecto();
    }
    //F3
    void LeerDatos()
    {
        gano           = PlayerPrefs.GetInt("Gano",          0);
        puntosNivel    = PlayerPrefs.GetInt("PuntosNivel",   0);
        puntosTotales  = PlayerPrefs.GetInt("PuntosTotales", 0);
        puntosGastados = PlayerPrefs.GetInt("PuntosGastados",0);
        nivelActual    = PlayerPrefs.GetInt("NivelActual",   1);
        esUltimoNivel  = PlayerPrefs.GetInt("EsUltimoNivel", 0);

        puntosDisponibles = Mathf.Max(0, puntosNivel - puntosGastados);

    }
    void MostrarPanelCorrecto()
    {
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

        GameObject TiendaGBtn  = GameObject.Find("TiendaGBtn");
        GameObject TiendaPBtn = GameObject.Find("TiendaPBtn");

        if (TiendaGBtn  != null) TiendaGBtn.SetActive(false);
        if (TiendaPBtn != null) TiendaPBtn.SetActive(false);

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

    public void AlAbrirTienda()
    {
        panelGanaste.SetActive(false);
        panelPerdiste.SetActive(false);

        ActualizarTextoPuntosEnTienda();

        if (textoMensajeCompra != null)
            textoMensajeCompra.text = "";

        panelTienda.SetActive(true);
    }
    void ProcesarSinTienda()
    {
        if (gano == 1)
            ProcesarVictoria();
        else
            ProcesarDerrota();
    }

    void ActualizarTextoPuntosEnTienda()
    {
        if (textoPuntosEnTienda != null)
            textoPuntosEnTienda.text = "Puntos disponibles: " + puntosDisponibles;
    }

    public void ComprarMartillo()
    {
        if (!IntentarGastar(costoMartillo)) return;

        InventarioManager.instancia.AgregarMartillo();
    }

    public void ComprarShuffle()
    {
        if (!IntentarGastar(costoShuffle)) return;

        InventarioManager.instancia.AgregarShuffle();
    }

    public void ComprarMovimientos()
    {
        if (!IntentarGastar(costoMovimientos)) return;

        InventarioManager.instancia.AgregarMovExtras();
    }

    bool IntentarGastar(int costo)
    {
        if (puntosDisponibles < costo)
        {
            if (textoMensajeCompra != null)
                textoMensajeCompra.text = "No tienes suficientes puntos";

            return false;
        }

        puntosDisponibles -= costo;
        ActualizarTextoPuntosEnTienda();

        if (textoMensajeCompra != null)
            textoMensajeCompra.text = "¡Comprado!";

        return true;
    }


    public void AlCerrarTienda()
    {
        panelTienda.SetActive(false);

        if (gano == 1)
            ProcesarVictoria();
        else
            ProcesarDerrota();
    }

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

    void ProcesarDerrota()
    {
        AlPresionarReintentar();
    }
    //F3
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

        int tokensActuales = PlayerPrefs.GetInt("Tokens", 0);
        PlayerPrefs.SetInt("Tokens", tokensActuales + tokens);
        PlayerPrefs.Save();

        panelFinal.SetActive(true);
    }

    public void AlPresionarSiguienteNivel()
    {
        int siguienteNivel = nivelActual >= nivelMaximo ? 1 : nivelActual + 1;
        PlayerPrefs.SetInt("NivelActual", siguienteNivel);
        PlayerPrefs.Save();

        SceneManager.LoadScene("EscenadeJuego");
    }
    //F3
    public void AlPresionarReintentar()
    {
        PlayerPrefs.SetInt("NivelActual", nivelActual);
        PlayerPrefs.Save();

        SceneManager.LoadScene("EscenadeJuego");
    }

    public void AlPresionarJugarDeNuevo()
    {
        PlayerPrefs.SetInt("NivelActual", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("EscenadeJuego");
    }

}
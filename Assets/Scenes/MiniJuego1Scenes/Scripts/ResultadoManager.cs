// ResultadoManager.cs
// Lee PlayerPrefs y muestra el panel correcto en ResultadoScene.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultadoManager : MonoBehaviour
{
    // ── Paneles ───────────────────────────────────────────
    [Header("Paneles")]
    public GameObject panelGanaste;
    public GameObject panelPerdiste;

    // ── Textos de PanelGanaste ────────────────────────────
    [Header("Textos — Ganaste")]
    public TextMeshProUGUI textoResultado;
    public TextMeshProUGUI textoMonedasGanadas;  // ← NUEVO

    // Muestra puntos y nivel completado

    // ── Textos de PanelPerdiste ───────────────────────────
    [Header("Textos — Perdiste")]
    public TextMeshProUGUI textoPuntosFinales;
    public TextMeshProUGUI textoMonedasPerdiste;

    [Header("Monedas — Total")]
    public TextMeshProUGUI textoTotalMonedas; 
    // ── Datos leídos de PlayerPrefs ───────────────────────
    private int gano;           // 1 = ganó, 0 = perdió
    private int puntosFinales;
    private int nivelActual;
    private int nivelMaximo = 3;

    private int puntosGastados   = 0;
    private int puntosRestantes  = 0;
    private int monedasGanadas   = 0;
    private int totalMonedas     = 0;


    // ─────────────────────────────────────────────────────
    void Start()
    {
        LeerDatos();
        MostrarPanelCorrecto();
    }

    // ── Lee los datos guardados desde GameScene ───────────
    void LeerDatos()
    {
        gano            = PlayerPrefs.GetInt("Gano",            0);
        puntosFinales   = PlayerPrefs.GetInt("PuntosFinales",   0);
        nivelActual     = PlayerPrefs.GetInt("NivelActual",     1);

        // ── NUEVOS ────────────────────────────────────────
        puntosGastados  = PlayerPrefs.GetInt("PuntosGastados",  0);
        puntosRestantes = PlayerPrefs.GetInt("PuntosRestantes", 0);
        monedasGanadas  = PlayerPrefs.GetInt("MonedasGanadas",  0);

        // Lee el total actual desde MonedasManager si existe,
        // si no lo lee directo de PlayerPrefs
        if (MonedasManager.instancia != null)
            totalMonedas = MonedasManager.instancia.ObtenerMonedas();
        else
            totalMonedas = PlayerPrefs.GetInt("Monedas", 0);

        Debug.Log($"Resultado — Ganó:{gano} Puntos:{puntosFinales} " +
                $"Monedas ganadas:{monedasGanadas} Total:{totalMonedas}");

    }

    // ── Activa el panel correcto según el resultado ───────
    void MostrarPanelCorrecto()
    {
        if (gano == 1)
        {
            panelPerdiste.SetActive(false);
            panelGanaste.SetActive(true);
            MostrarDatosVictoria();
        }
        else
        {
            panelGanaste.SetActive(false);
            panelPerdiste.SetActive(true);
            MostrarDatosDerrota();
        }
    }

    // ── Rellena los textos del panel de victoria ──────────
    void MostrarDatosVictoria()
    {
        if (textoResultado != null)
        {
            if (nivelActual >= nivelMaximo)
            {
                textoResultado.text =
                    "¡Completaste todos los niveles!\n" +
                    "Puntos finales: " + puntosFinales;
            }
            else
            {
                textoResultado.text =
                    "Nivel completado: "  + nivelActual     + "\n" +
                    "Puntos obtenidos: "  + puntosFinales   + "\n" +
                    "Puntos gastados: "   + puntosGastados  + "\n" +
                    "Puntos sobrantes: "  + puntosRestantes;
            }
        }

        // ── Monedas ganadas este nivel ────────────────────
        if (textoMonedasGanadas != null)
            textoMonedasGanadas.text = "+" + monedasGanadas + " monedas";

        // ── Total de monedas acumuladas ───────────────────
        if (textoTotalMonedas != null)
            textoTotalMonedas.text = "Total monedas: " + totalMonedas;
    }

    // ── Rellena los textos del panel de derrota ───────────
    void MostrarDatosDerrota()
    {
        if (textoPuntosFinales == null) return;

        textoPuntosFinales.text =
            "Puntos obtenidos: " + puntosFinales;
    }

    // ─────────────────────────────────────────────────────
    // BOTONES
    // ─────────────────────────────────────────────────────

    // ── Botón "Siguiente Nivel" ───────────────────────────
    public void AlPresionarSiguienteNivel()
    {
        if (nivelActual >= nivelMaximo)
        {
            // Ya completó todo, reinicia desde nivel 1
            PlayerPrefs.SetInt("NivelActual", 1);
        }
        else
        {
            // Avanza al siguiente nivel
            PlayerPrefs.SetInt("NivelActual", nivelActual + 1);
        }

        PlayerPrefs.Save();
        SceneManager.LoadScene("EscenadeJuego");
    }

    // ── Botón "Reintentar" ────────────────────────────────
    public void AlPresionarReintentar()
    {
        // Mantiene el mismo nivel, solo regresa a jugar
        PlayerPrefs.SetInt("NivelActual", nivelActual);
        PlayerPrefs.Save();
        SceneManager.LoadScene("EscenadeJuego");
    }
}
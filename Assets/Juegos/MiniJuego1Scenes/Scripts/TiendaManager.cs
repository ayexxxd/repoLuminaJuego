using UnityEngine;
using TMPro;

public class TiendaManager : MonoBehaviour
{
    public static TiendaManager instancia;

    [Header("Panel de Tienda")]
    public GameObject panelTienda;
    public TextMeshProUGUI textoPuntosEnTienda;

    [Header("Costos (puntos)")]
    public int costoMartillo = 300;
    public int costoShuffle = 500;
    public int costoMovimientos = 800;

    public bool martilloActivo = false;

    public int martillosGuardados = 0;
    public int shufflesGuardados = 0;
    public int movExtrasGuardados = 0;

    void Awake()
    {
        if (instancia == null) instancia = this;
    }

    public void AbrirTienda()
    {
        ActualizarPuntosEnPanel();
        panelTienda.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CerrarTienda()
    {
        panelTienda.SetActive(false);
        Time.timeScale = 1f;
    }

    void ActualizarPuntosEnPanel()
    {
        if (textoPuntosEnTienda != null)
            textoPuntosEnTienda.text = "Puntos: " + GameManager.instancia.ObtenerPuntos();
    }

    public void ComprarMartillo()
    {
        if (!GameManager.instancia.GastarPuntos(costoMartillo))
        {
            return;
        }
        martillosGuardados++;
        martilloActivo = true;
        CerrarTienda();
    }

    public void ComprarShuffle()
    {
        if (!GameManager.instancia.GastarPuntos(costoShuffle))
        {
            return;
        }
        shufflesGuardados++;
        CerrarTienda();
        Board.instancia.MezclarTablero();
    }

    public void ComprarMovimientos()
    {
        if (!GameManager.instancia.GastarPuntos(costoMovimientos))
        {
            return;
        }
        movExtrasGuardados++;
        GameManager.instancia.AgregarMovimientos(3);
        CerrarTienda();
    }

    public void UsarMartilloEn(Pieza pieza)
    {
        if (!martilloActivo) return;
        martilloActivo = false;
        martillosGuardados = Mathf.Max(0, martillosGuardados - 1);
        Board.instancia.DestruirPiezaEn(pieza.col, pieza.fil);
    }
}
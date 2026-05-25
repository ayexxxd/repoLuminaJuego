// TiendaManager.cs
// Tienda de power-ups. Se compra con puntos.

using UnityEngine;
using TMPro;

public class TiendaManager : MonoBehaviour
{
    // ── Singleton ─────────────────────────────────────────
    public static TiendaManager instancia;

    // ── UI ────────────────────────────────────────────────
    [Header("Panel de Tienda")]
    public GameObject      panelTienda;
    public TextMeshProUGUI textoPuntosEnTienda;

    // ── Costos ────────────────────────────────────────────
    [Header("Costos (puntos)")]
    public int costoMartillo    = 300;
    public int costoShuffle     = 500;
    public int costoMovimientos = 800;

    // ── Estado del martillo ───────────────────────────────
    // Cuando está true, el próximo clic en una pieza la destruye
    public bool martilloActivo = false;

    // ── Inventario ────────────────────────────────────────
    [HideInInspector] public int martillosGuardados   = 0;
    [HideInInspector] public int shufflesGuardados    = 0;
    [HideInInspector] public int movExtrasGuardados   = 0;

    // ─────────────────────────────────────────────────────
    void Awake()
    {
        if (instancia == null) instancia = this;
    }

    // ── Abrir / Cerrar ────────────────────────────────────
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

    // ── Comprar Martillo ──────────────────────────────────
    public void ComprarMartillo()
    {
        if (!GameManager.instancia.GastarPuntos(costoMartillo))
        {
            Debug.Log("Puntos insuficientes — Martillo");
            return;
        }
        martillosGuardados++;
        martilloActivo = true;
        CerrarTienda();
        Debug.Log("Martillo activo. Toca una pieza.");
    }

    // ── Comprar Shuffle ───────────────────────────────────
    public void ComprarShuffle()
    {
        if (!GameManager.instancia.GastarPuntos(costoShuffle))
        {
            Debug.Log("Puntos insuficientes — Shuffle");
            return;
        }
        shufflesGuardados++;
        CerrarTienda();
        Board.instancia.MezclarTablero();
    }

    // ── Comprar +3 Movimientos ────────────────────────────
    public void ComprarMovimientos()
    {
        if (!GameManager.instancia.GastarPuntos(costoMovimientos))
        {
            Debug.Log("Puntos insuficientes — Movimientos");
            return;
        }
        movExtrasGuardados++;
        GameManager.instancia.AgregarMovimientos(3);
        CerrarTienda();
    }

    // ── Usar martillo sobre una pieza ─────────────────────
    // Llamado desde Board.cs cuando martilloActivo == true
    public void UsarMartilloEn(Pieza pieza)
    {
        if (!martilloActivo) return;
        martilloActivo = false;
        martillosGuardados = Mathf.Max(0, martillosGuardados - 1);
        Board.instancia.DestruirPiezaEn(pieza.col, pieza.fil);
        Debug.Log($"Martillo en [{pieza.col}, {pieza.fil}]");
    }
}
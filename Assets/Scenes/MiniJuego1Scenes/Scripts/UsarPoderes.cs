// UsadorPowerUps.cs
// Controla el uso de power-ups dentro de GameScene.
// Ponlo en un GameObject vacío llamado "UsadorPowerUps".

using UnityEngine;
using TMPro;

public class UsarPoderes : MonoBehaviour
{
    public static UsarPoderes instancia;

    // ── Texto de aviso (opcional) ─────────────────────────
    [Header("UI")]
    public TextMeshProUGUI textoAviso;

    [Header("Cursor Martillo")]
    public Texture2D cursorMartillo;
    // Arrastra aquí un TMP para mostrar mensajes como
    // "No tienes martillos" — puede ser temporal en pantalla

    // ── Estado del martillo ───────────────────────────────
    private bool modoMartilloActivo = false;
    public  bool ModoMartilloActivo => modoMartilloActivo;

    // ─────────────────────────────────────────────────────
    void Awake()
    {
        if (instancia == null) instancia = this;
    }

    // ─────────────────────────────────────────────────────
    // BOTÓN MARTILLO
    // ─────────────────────────────────────────────────────
    public void AlPresionarMartillo()
    {
        if (InventarioManager.instancia.martillos <= 0)
        {
            MostrarAviso("No tienes martillos");
            return;
        }

        modoMartilloActivo = true;
        MostrarAviso("Toca una runa para destruirla");

        // Cambia el cursor al sprite del martillo
        if (cursorMartillo != null)
            Cursor.SetCursor(cursorMartillo, Vector2.zero, CursorMode.Auto);

        Debug.Log("Modo martillo activado");
    }

    // Llamado desde Board.cs cuando el jugador hace clic en una pieza
    // y modoMartilloActivo == true
    public void UsarMartilloEnPieza(Pieza pieza)
    {
        if (!modoMartilloActivo) return;

        modoMartilloActivo = false;

        // Restaura cursor normal
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        bool exito = InventarioManager.instancia.UsarMartillo();
        if (!exito) return;

        Board.instancia.DestruirPiezaEn(pieza.col, pieza.fil);
        MostrarAviso("¡Runa destruida!");
        Debug.Log($"Martillo usado en [{pieza.col},{pieza.fil}]");
    }

    // ─────────────────────────────────────────────────────
    // BOTÓN SHUFFLE
    // ─────────────────────────────────────────────────────
    public void AlPresionarShuffle()
    {
        if (InventarioManager.instancia.shuffles <= 0)
        {
            MostrarAviso("No tienes shuffles");
            return;
        }

        bool exito = InventarioManager.instancia.UsarShuffle();
        if (!exito) return;

        Board.instancia.MezclarTablero();
        MostrarAviso("¡Tablero mezclado!");
        Debug.Log("Shuffle usado");
    }

    // ─────────────────────────────────────────────────────
    // BOTÓN +3 MOVIMIENTOS
    // ─────────────────────────────────────────────────────
    public void AlPresionarMovExtra()
    {
        if (InventarioManager.instancia.movExtras <= 0)
        {
            MostrarAviso("No tienes movimientos extra");
            return;
        }

        bool exito = InventarioManager.instancia.UsarMovExtra();
        if (!exito) return;

        JuegoManager.instancia.AgregarMovimientos(3);
        MostrarAviso("+3 movimientos agregados");
        Debug.Log("+3 movimientos usados");
    }

    // ─────────────────────────────────────────────────────
    // AVISO EN PANTALLA
    // ─────────────────────────────────────────────────────
    void MostrarAviso(string mensaje)
    {
        if (textoAviso == null) return;
        textoAviso.text = mensaje;

        // Borra el aviso después de 2 segundos
        CancelInvoke(nameof(LimpiarAviso));
        Invoke(nameof(LimpiarAviso), 2f);
    }

    void LimpiarAviso()
    {
        if (textoAviso != null)
            textoAviso.text = "";
    }
}
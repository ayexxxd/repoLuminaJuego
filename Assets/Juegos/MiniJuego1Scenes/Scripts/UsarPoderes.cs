using UnityEngine;
using TMPro;

public class UsarPoderes : MonoBehaviour
{
    public static UsarPoderes instancia;

    [Header("UI")]
    public TextMeshProUGUI textoAviso;

    [Header("Cursor Martillo")]
    public Texture2D cursorMartillo;

    private bool modoMartilloActivo = false;
    public  bool ModoMartilloActivo => modoMartilloActivo;

    void Awake()
    {
        if (instancia == null) instancia = this;
    }
    //f5
    public void AlPresionarMartillo()
    {
        if (InventarioManager.instancia.martillos <= 0)
        {
            MostrarAviso("No tienes martillos");
            return;
        }

        modoMartilloActivo = true;
        MostrarAviso("Toca una runa para destruirla");

        if (cursorMartillo != null)
            Cursor.SetCursor(cursorMartillo, Vector2.zero, CursorMode.Auto);

    }
    //f5
    public void UsarMartilloEnPieza(Pieza pieza)
    {
        if (!modoMartilloActivo) return;

        modoMartilloActivo = false;

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        bool exito = InventarioManager.instancia.UsarMartillo();
        if (!exito) return;

        Board.instancia.DestruirPiezaEn(pieza.col, pieza.fil);
        MostrarAviso("¡Runa destruida!");
    }


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
    }

    public void AlPresionarMovExtra()
    {
        if (InventarioManager.instancia.movExtras <= 0)
        {
            MostrarAviso("No tienes movimientos extra");
            return;
        }

        bool exito = InventarioManager.instancia.UsarMovExtra();
        if (!exito) return;

        GameManager.instancia.AgregarMovimientos(3);
        MostrarAviso("+3 movimientos agregados");
    }

    void MostrarAviso(string mensaje)
    {
        if (textoAviso == null) return;
        textoAviso.text = mensaje;

        CancelInvoke(nameof(LimpiarAviso));
        Invoke(nameof(LimpiarAviso), 2f);
    }

    void LimpiarAviso()
    {
        if (textoAviso != null)
            textoAviso.text = "";
    }
}
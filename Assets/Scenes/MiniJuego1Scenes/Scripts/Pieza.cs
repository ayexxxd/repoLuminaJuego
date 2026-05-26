// Pieza.cs
// Datos de cada pieza y detección de clic.

using UnityEngine;

public class Pieza : MonoBehaviour
{
    // ── Datos de la pieza ─────────────────────────────────
    public int tipoPieza;   // 0=rojo, 1=azul, 2=amarillo, 3=morado
    public int col;
    public int fil;

    // ── Referencia al tablero ─────────────────────────────
    private Board tableroRef;

    // ── Especiales (se usan más adelante) ─────────────────
    [HideInInspector] public bool esEspecial   = false;
    [HideInInspector] public int  tipoEspecial = 0;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    // tipoEspecial: 1 = Monstruo Rojo (bomba 3x3)
    //               2 = Monstruo Verde (elimina color)

    // ── Inicialización ────────────────────────────────────
    public void Iniciar(int tipo, int columna, int fila, Board tablero)
    {
        tipoPieza      = tipo;
        col            = columna;
        fil            = fila;
        tableroRef     = tablero;
        spriteRenderer = GetComponent<SpriteRenderer>(); // ← NUEVA
    }

    // ── Detección de clic ─────────────────────────────────
    void OnMouseDown()
    {
        if (tableroRef != null)
            tableroRef.AlHacerClicEnPieza(this);
    }
}
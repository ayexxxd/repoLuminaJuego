using UnityEngine;

public class Pieza : MonoBehaviour
{
    public int tipoPieza;
    public int col;
    public int fil;

    private Board tableroRef;

    public bool esEspecial   = false;
    public int  tipoEspecial = 0;
    public SpriteRenderer spriteRenderer;

    public void Iniciar(int tipo, int columna, int fila, Board tablero)
    {
        tipoPieza = tipo;
        col = columna;
        fil = fila;
        tableroRef = tablero;
        spriteRenderer = GetComponent<SpriteRenderer>(); // ← NUEVA
    }


    void OnMouseDown()
    {
        if (tableroRef != null)
            tableroRef.AlHacerClicEnPieza(this);
    }
}
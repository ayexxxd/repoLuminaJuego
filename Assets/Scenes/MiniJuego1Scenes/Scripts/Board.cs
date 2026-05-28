using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour
{
    public static Board instancia;

    [Header("Configuración del Tablero")]
    public int columnas = 7;
    public int filas    = 8;
    public float tamañoCelda = 1f;

    [Header("Prefabs y Sprites")]
    public GameObject prefabPieza;    
    public Sprite[]   spritePiezas;    
    public Sprite     spriteMonstruoRojo;  
    public Sprite     spriteMonstruoVerde;

    private Pieza[,] tablero;         
    private Pieza piezaSeleccionada;  
    private bool estaProcesando = false; 

    [HideInInspector] public bool monstruoRojoActivo  = false; 
    [HideInInspector] public bool monstruoVerdeActivo = false;
    [HideInInspector] public int tiposActivosEnNivel = 4;

    void Awake()
    {
        if (instancia == null) instancia = this;
    }

    void Start()
    {
        tablero = new Pieza[columnas, filas];
        GenerarTablero();
    }

    void GenerarTablero()
    {
        for (int col = 0; col < columnas; col++)
            for (int fil = 0; fil < filas; fil++)
                CrearPieza(col, fil);
    }

    void CrearPieza(int col, int fil)
    {
        Vector2 posicion = ObtenerPosicionMundo(col, fil);

        int tipo;
        int intentos = 0;
        do
        {
            tipo = Random.Range(0, tiposActivosEnNivel);
            intentos++;
        }
        while (intentos < 10 && FormaríaMatch(col, fil, tipo));

        GameObject obj = Instantiate(prefabPieza, posicion, Quaternion.identity);
        obj.transform.parent = this.transform;

        Pieza pieza = obj.GetComponent<Pieza>();
        pieza.Iniciar(tipo, col, fil, this);
        obj.GetComponent<SpriteRenderer>().sprite = spritePiezas[tipo];

        tablero[col, fil] = pieza;
    }

    bool FormaríaMatch(int col, int fil, int tipo)
    {
        // Dos piezas a la izquierda del mismo tipo
        if (col >= 2 &&
            tablero[col-1, fil] != null && tablero[col-1, fil].tipoPieza == tipo &&
            tablero[col-2, fil] != null && tablero[col-2, fil].tipoPieza == tipo)
            return true;

        // Dos piezas abajo del mismo tipo
        if (fil >= 2 &&
            tablero[col, fil-1] != null && tablero[col, fil-1].tipoPieza == tipo &&
            tablero[col, fil-2] != null && tablero[col, fil-2].tipoPieza == tipo)
            return true;

        return false;
    }

    public Vector2 ObtenerPosicionMundo(int col, int fil)
    {
        float x = col * tamañoCelda - (columnas * tamañoCelda / 2f) + tamañoCelda / 2f;
        float y = fil * tamañoCelda - (filas    * tamañoCelda / 2f) + tamañoCelda / 2f;
        return new Vector2(x, y) + (Vector2)transform.position;
    }

    public void AlHacerClicEnPieza(Pieza pieza)
    {
        if (estaProcesando) return;

        if (UsarPoderes.instancia != null &&
            UsarPoderes.instancia.ModoMartilloActivo)
        {
            UsarPoderes.instancia.UsarMartilloEnPieza(pieza);
            return;
        }

        if (piezaSeleccionada == null)
        {
            piezaSeleccionada = pieza;
            piezaSeleccionada.transform.localScale = Vector3.one * 1.2f;
        }
        else
        {
            if (SonVecinas(piezaSeleccionada, pieza))
            {
                StartCoroutine(IntentarIntercambio(piezaSeleccionada, pieza));
            }
            else
            {
                piezaSeleccionada.transform.localScale = Vector3.one;
                piezaSeleccionada = pieza;
                piezaSeleccionada.transform.localScale = Vector3.one * 1.2f;
            }
        }
    }

    bool SonVecinas(Pieza a, Pieza b)
    {
        int difCol = Mathf.Abs(a.col - b.col);
        int difFil = Mathf.Abs(a.fil - b.fil);
        return (difCol == 1 && difFil == 0) || (difCol == 0 && difFil == 1);
    }

    IEnumerator IntentarIntercambio(Pieza a, Pieza b)
    {
        estaProcesando = true;
        a.transform.localScale = Vector3.one;
        piezaSeleccionada = null;


        if (a.esEspecial || b.esEspecial)
        {
            yield return StartCoroutine(ActivarMonstruoDeIntercambio(a, b));
            estaProcesando = false;
            yield break;
        }

        IntercambiarPiezas(a, b);
        yield return new WaitForSeconds(0.2f);

        bool hayMatch = VerificarMatches();

        if (hayMatch)
        {
            GameManager.instancia.UsarMovimiento();
            GestorPreguntas.instancia.RegistrarMovimiento();
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
            IntercambiarPiezas(a, b);
        }

        estaProcesando = false;
    }

    IEnumerator ActivarMonstruoDeIntercambio(Pieza a, Pieza b)
    {
        Pieza monstruo = a.esEspecial ? a : b;
        Pieza objetivo = a.esEspecial ? b : a;

        bool[,] aDestruir = new bool[columnas, filas];

        if (monstruo.tipoEspecial == 1)
        {
            ActivarMonstruoRojo(monstruo.col, monstruo.fil, aDestruir);
        }
        else if (monstruo.tipoEspecial == 2)
        {
            ActivarMonstruoVerde(monstruo.col, monstruo.fil,
                                objetivo.tipoPieza, aDestruir);
        }


        aDestruir[monstruo.col, monstruo.fil] = true;

        int cantidad = 0;
        for (int c = 0; c < columnas; c++)
            for (int f = 0; f < filas; f++)
                if (aDestruir[c, f]) cantidad++;


        GameManager.instancia.AgregarPuntos(CalcularPuntos(cantidad));
        GameManager.instancia.UsarMovimiento();
        GestorPreguntas.instancia.RegistrarMovimiento();

        for (int c = 0; c < columnas; c++)
        {
            for (int f = 0; f < filas; f++)
            {
                if (aDestruir[c, f] && tablero[c, f] != null)
                {
                    Destroy(tablero[c, f].gameObject);
                    tablero[c, f] = null;
                }
            }
        }

        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(CaerYRellenar());
    }

    void IntercambiarPiezas(Pieza a, Pieza b)
    {
        int tempCol = a.col;
        int tempFil = a.fil;

        tablero[a.col, a.fil] = b;
        tablero[b.col, b.fil] = a;

        a.col = b.col; a.fil = b.fil;
        b.col = tempCol; b.fil = tempFil;

        a.transform.position = ObtenerPosicionMundo(a.col, a.fil);
        b.transform.position = ObtenerPosicionMundo(b.col, b.fil);
    }

    bool VerificarMatches()
    {
        bool[,] aDestruir   = new bool[columnas, filas];
        bool    encontróMatch = false;

        for (int fil = 0; fil < filas; fil++)
        {
            for (int col = 0; col < columnas; col++)
            {
                int longitud = ContarLineaHorizontal(col, fil);

                if (longitud >= 3)
                {
                    for (int i = 0; i < longitud; i++)
                        aDestruir[col + i, fil] = true;

                    if (SeCrearáMonstruo(longitud))
                    {
                        aDestruir[col, fil] = false; 
                        ProcesarMonstruo(col, fil, longitud, true);
                    }

                    encontróMatch = true;
                    col += longitud - 1;
                }
            }
        }

        for (int col = 0; col < columnas; col++)
        {
            for (int fil = 0; fil < filas; fil++)
            {
                int longitud = ContarLineaVertical(col, fil);

                if (longitud >= 3)
                {
                    for (int i = 0; i < longitud; i++)
                        aDestruir[col, fil + i] = true;

                    if (SeCrearáMonstruo(longitud))
                    {
                        aDestruir[col, fil] = false; // ← celda protegida
                        ProcesarMonstruo(col, fil, longitud, false);
                    }

                    encontróMatch = true;
                    fil += longitud - 1;
                }
            }
        }

        if (encontróMatch)
            DestruirMatches(aDestruir);

        return encontróMatch;
    }

    int ContarLineaHorizontal(int colInicio, int fil)
    {
        if (tablero[colInicio, fil] == null) return 0;

        int tipo    = tablero[colInicio, fil].tipoPieza;
        int conteo  = 1;

        for (int c = colInicio + 1; c < columnas; c++)
        {
            if (tablero[c, fil] != null && tablero[c, fil].tipoPieza == tipo)
                conteo++;
            else
                break;
        }

        return conteo;
    }

    int ContarLineaVertical(int col, int filInicio)
    {
        if (tablero[col, filInicio] == null) return 0;

        int tipo   = tablero[col, filInicio].tipoPieza;
        int conteo = 1;

        for (int f = filInicio + 1; f < filas; f++)
        {
            if (tablero[col, f] != null && tablero[col, f].tipoPieza == tipo)
                conteo++;
            else
                break;
        }

        return conteo;
    }

    void ProcesarMonstruo(int col, int fil, int longitud, bool esHorizontal)
    {

        if (longitud == 4 && monstruoRojoActivo)
        {
            CrearMonstruo(col, fil, 1);
            return;
        }

        if (longitud >= 5 && monstruoVerdeActivo)
        {
            CrearMonstruo(col, fil, 2);
            return;
        }

    }

    bool SeCrearáMonstruo(int longitud)
    {
        if (longitud == 4 && monstruoRojoActivo)  return true;
        if (longitud >= 5 && monstruoVerdeActivo) return true;
        return false;
    }

    void CrearMonstruo(int col, int fil, int tipoMonstruo)
    {
        Pieza pieza = tablero[col, fil];
        if (pieza == null) return;

        pieza.esEspecial   = true;
        pieza.tipoEspecial = tipoMonstruo;

        if (tipoMonstruo == 1 && spriteMonstruoRojo != null)
            pieza.spriteRenderer.sprite = spriteMonstruoRojo;

        if (tipoMonstruo == 2 && spriteMonstruoVerde != null)
            pieza.spriteRenderer.sprite = spriteMonstruoVerde;

    }

    void DestruirMatches(bool[,] aDestruir)
    {
        ActivarMonstruosMarcados(aDestruir);

        int cantidad = 0;

        for (int col = 0; col < columnas; col++)
        {
            for (int fil = 0; fil < filas; fil++)
            {
                if (aDestruir[col, fil] && tablero[col, fil] != null)
                {
                    Destroy(tablero[col, fil].gameObject);
                    tablero[col, fil] = null;
                    cantidad++;
                }
            }
        }

        int puntos = CalcularPuntos(cantidad);
        GameManager.instancia.AgregarPuntos(puntos);

        StartCoroutine(CaerYRellenar());
    }

    void ActivarMonstruosMarcados(bool[,] aDestruir)
    {
        for (int col = 0; col < columnas; col++)
        {
            for (int fil = 0; fil < filas; fil++)
            {
                if (!aDestruir[col, fil]) continue;

                Pieza pieza = tablero[col, fil];
                if (pieza == null || !pieza.esEspecial) continue;

                if (pieza.tipoEspecial == 1)
                    ActivarMonstruoRojo(col, fil, aDestruir);

                if (pieza.tipoEspecial == 2)
                    ActivarMonstruoVerde(col, fil, pieza.tipoPieza, aDestruir);
            }
        }
    }

    void ActivarMonstruoRojo(int centroCol, int centroFil, bool[,] aDestruir)
    {

        for (int c = 0; c < columnas; c++)
        {
            aDestruir[c, centroFil] = true;
        }

        for (int f = 0; f < filas; f++)
        {
            aDestruir[centroCol, f] = true;
        }
    }

    void ActivarMonstruoVerde(int col, int fil, int colorObjetivo, bool[,] aDestruir)
    {

        for (int c = 0; c < columnas; c++)
        {
            for (int f = 0; f < filas; f++)
            {
                if (tablero[c, f] != null && tablero[c, f].tipoPieza == colorObjetivo)
                    aDestruir[c, f] = true;
            }
        }
    }

    int CalcularPuntos(int cantidad)
    {
        if (cantidad <= 3) return 30;
        if (cantidad == 4) return 60;
        return 100 + (cantidad - 5) * 20;
    }


    IEnumerator CaerYRellenar()
    {
        yield return new WaitForSeconds(0.2f);

        for (int col = 0; col < columnas; col++)
        {
            for (int fil = 0; fil < filas; fil++)
            {
                if (tablero[col, fil] == null)
                {
                    for (int filArriba = fil + 1; filArriba < filas; filArriba++)
                    {
                        if (tablero[col, filArriba] != null)
                        {
                            Pieza cayendo = tablero[col, filArriba];
                            tablero[col, fil]      = cayendo;
                            tablero[col, filArriba] = null;
                            cayendo.fil = fil;
                            cayendo.transform.position = ObtenerPosicionMundo(col, fil);
                            break;
                        }
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.2f);

        for (int col = 0; col < columnas; col++)
            for (int fil = 0; fil < filas; fil++)
                if (tablero[col, fil] == null)
                    CrearPieza(col, fil);

        yield return new WaitForSeconds(0.2f);

        bool nuevoMatch = VerificarMatches();
        if (!nuevoMatch)
            estaProcesando = false;
    }

    public void DestruirPiezaEn(int col, int fil)
    {
        if (tablero[col, fil] == null) return;
        Destroy(tablero[col, fil].gameObject);
        tablero[col, fil] = null;
        StartCoroutine(CaerYRellenar());
    }

    public void MezclarTablero()
    {
        System.Collections.Generic.List<Pieza> lista = new();
        for (int col = 0; col < columnas; col++)
            for (int fil = 0; fil < filas; fil++)
                if (tablero[col, fil] != null)
                    lista.Add(tablero[col, fil]);

        for (int i = lista.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (lista[i], lista[j]) = (lista[j], lista[i]);
        }

        int indice = 0;
        for (int col = 0; col < columnas; col++)
        {
            for (int fil = 0; fil < filas; fil++)
            {
                if (tablero[col, fil] != null)
                {
                    tablero[col, fil]           = lista[indice];
                    lista[indice].col           = col;
                    lista[indice].fil           = fil;
                    lista[indice].transform.position = ObtenerPosicionMundo(col, fil);
                    indice++;
                }
            }
        }

        //GameManager.instancia.AgregarMovimientos(3);
    }

    public void ReiniciarTablero()
    {
        for (int col = 0; col < columnas; col++)
        {
            for (int fil = 0; fil < filas; fil++)
            {
                if (tablero[col, fil] != null)
                {
                    Destroy(tablero[col, fil].gameObject);
                    tablero[col, fil] = null;
                }
            }
        }

        GenerarTablero();
        estaProcesando = false;
    }
}
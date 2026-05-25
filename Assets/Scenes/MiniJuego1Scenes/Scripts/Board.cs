// Board.cs
// Controla todo el tablero: generar piezas, intercambios, matches, caída y relleno.

using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour
{
    // ── Singleton ─────────────────────────────────────────
    public static Board instancia;

    // ── Configuración del tablero ─────────────────────────
    [Header("Configuración del Tablero")]
    public int columnas = 7;
    public int filas    = 8;
    public float tamañoCelda = 1f;

    // ── Prefabs y sprites ─────────────────────────────────
    [Header("Prefabs y Sprites")]
    public GameObject prefabPieza;     // Arrastra el prefab "Pieza" aquí
    public Sprite[]   spritePiezas;    // Arrastra tus 4 sprites aquí
    public Sprite     spriteMonstruoRojo;   // ← NUEVO arrastra aquí
    public Sprite     spriteMonstruoVerde;

    // ── Estado interno ────────────────────────────────────
    private Pieza[,] tablero;          // El array 2D con todas las piezas
    private Pieza piezaSeleccionada;   // La pieza que el jugador tocó primero
    private bool estaProcesando = false; // Bloquea clics mientras se anima

    [HideInInspector] public bool monstruoRojoActivo  = false; // ← NUEVO
    [HideInInspector] public bool monstruoVerdeActivo = false; // ← NUEVO
    [HideInInspector] public int tiposActivosEnNivel = 4; // ← NUEVO

    // ─────────────────────────────────────────────────────
    void Awake()
    {
        if (instancia == null) instancia = this;
    }

    void Start()
    {
        tablero = new Pieza[columnas, filas];
        GenerarTablero();
    }

    // ── Genera todas las piezas al inicio ─────────────────
    void GenerarTablero()
    {
        for (int col = 0; col < columnas; col++)
            for (int fil = 0; fil < filas; fil++)
                CrearPieza(col, fil);
    }

    // ── Crea una pieza en la posición [col, fil] ──────────
    void CrearPieza(int col, int fil)
    {
        Vector2 posicion = ObtenerPosicionMundo(col, fil);

        // Elige un tipo que no forme match inmediato
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

    // ── Evita matches al generar ──────────────────────────
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

    // ── Convierte columna/fila a posición en el mundo ─────
    public Vector2 ObtenerPosicionMundo(int col, int fil)
    {
        float x = col * tamañoCelda - (columnas * tamañoCelda / 2f) + tamañoCelda / 2f;
        float y = fil * tamañoCelda - (filas    * tamañoCelda / 2f) + tamañoCelda / 2f;
        return new Vector2(x, y) + (Vector2)transform.position;
    }

    // ─────────────────────────────────────────────────────
    // SELECCIÓN E INTERCAMBIO
    // ─────────────────────────────────────────────────────

    // Llamado desde Pieza.cs al hacer clic
    public void AlHacerClicEnPieza(Pieza pieza)
    {
        if (estaProcesando) return;

        // Martillo activo: destruye la pieza directamente
        if (TiendaManager.instancia != null && TiendaManager.instancia.martilloActivo)
        {
            TiendaManager.instancia.UsarMartilloEn(pieza);
            return;
        }

        // 🔥 NUEVO: Si el jugador toca un monstruo, explota con UN SOLO CLIC
        if (pieza != null && pieza.esEspecial)
        {
            // Creamos el mapa de destrucción temporal
            bool[,] aDestruir = new bool[columnas, filas];
            
            // Marcamos la posición de este monstruo para que se limpie
            aDestruir[pieza.col, pieza.fil] = true;

            // Disparamos los efectos que ya programaste con Claude
            if (pieza.tipoEspecial == 1)
                ActivarMonstruoRojo(pieza.col, pieza.fil, aDestruir);
            
            if (pieza.tipoEspecial == 2)
                ActivarMonstruoVerde(pieza.col, pieza.fil, pieza.tipoPieza, aDestruir);

            // Reseteamos la selección por si el jugador tenía otra pieza agrandada
            if (piezaSeleccionada != null)
            {
                piezaSeleccionada.transform.localScale = Vector3.one;
                piezaSeleccionada = null;
            }

            // Destruimos, sumamos puntos, hacemos caer runas y rellenamos todo solo
            DestruirMatches(aDestruir);
            return; // ← Súper importante: detiene el método para que no intente mover nada
        }

        // ── AQUÍ SIGUE TU CÓDIGO NORMAL DE SELECCIÓN INTERNA ─────────────────
        if (piezaSeleccionada == null)
        {
            // Primera selección
            piezaSeleccionada = pieza;
            piezaSeleccionada.transform.localScale = Vector3.one * 1.2f;
        }
        else
        {
            if (SonVecinas(piezaSeleccionada, pieza))
            {
                // Son vecinas: intentar intercambio
                StartCoroutine(IntentarIntercambio(piezaSeleccionada, pieza));
            }
            else
            {
                // No son vecinas: cambiar selección
                piezaSeleccionada.transform.localScale = Vector3.one;
                piezaSeleccionada = pieza;
                piezaSeleccionada.transform.localScale = Vector3.one * 1.2f;
            }
        }
    }

    // Verifica si dos piezas son adyacentes
    bool SonVecinas(Pieza a, Pieza b)
    {
        int difCol = Mathf.Abs(a.col - b.col);
        int difFil = Mathf.Abs(a.fil - b.fil);
        return (difCol == 1 && difFil == 0) || (difCol == 0 && difFil == 1);
    }

    // Coroutine principal del intercambio
    IEnumerator IntentarIntercambio(Pieza a, Pieza b)
    {
        estaProcesando = true;
        a.transform.localScale = Vector3.one;
        piezaSeleccionada = null;

        IntercambiarPiezas(a, b);
        yield return new WaitForSeconds(0.2f);

        bool hayMatch = VerificarMatches();

        if (hayMatch)
        {
            // Movimiento válido
            GameManager.instancia.UsarMovimiento();
            GestorPreguntas.instancia.RegistrarMovimiento();
        }
        else
        {
            // Sin match: revertir
            yield return new WaitForSeconds(0.2f);
            IntercambiarPiezas(a, b);
        }

        estaProcesando = false;
    }

    // Intercambia dos piezas en el array y en pantalla
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

    // ─────────────────────────────────────────────────────
    // DETECCIÓN DE MATCHES
    // ─────────────────────────────────────────────────────

    bool VerificarMatches()
    {
        // Guardamos cuántas piezas del mismo tipo hay en cada línea
        bool[,] aDestruir   = new bool[columnas, filas];
        bool    encontróMatch = false;

        // ── Horizontal ────────────────────────────────────
        for (int fil = 0; fil < filas; fil++)
        {
            for (int col = 0; col < columnas; col++)
            {
                int longitud = ContarLineaHorizontal(col, fil);

                if (longitud >= 3)
                {
                    for (int i = 0; i < longitud; i++)
                        aDestruir[col + i, fil] = true;

                    // Si se crea monstruo, protege la primera celda
                    if (SeCrearáMonstruo(longitud))
                    {
                        aDestruir[col, fil] = false; // ← celda protegida
                        ProcesarMonstruo(col, fil, longitud, true);
                    }

                    encontróMatch = true;
                    col += longitud - 1;
                }
            }
        }

        // ── Vertical ──────────────────────────────────────
        for (int col = 0; col < columnas; col++)
        {
            for (int fil = 0; fil < filas; fil++)
            {
                int longitud = ContarLineaVertical(col, fil);

                if (longitud >= 3)
                {
                    for (int i = 0; i < longitud; i++)
                        aDestruir[col, fil + i] = true;

                    // Si se crea monstruo, protege la primera celda
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

    // ── Cuenta piezas iguales seguidas hacia la derecha ──
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

    // ── Cuenta piezas iguales seguidas hacia arriba ───────
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

    // ── Decide si crear monstruo según longitud del match ─
    void ProcesarMonstruo(int col, int fil, int longitud, bool esHorizontal)
    {
        Debug.Log($"ProcesarMonstruo — col:{col} fil:{fil} longitud:{longitud} " +
              $"RojoActivo:{monstruoRojoActivo} VerdeActivo:{monstruoVerdeActivo}");

        if (longitud == 4 && monstruoRojoActivo)
        {
            Debug.Log("→ Creando Monstruo Rojo");
            CrearMonstruo(col, fil, 1);
            return;
        }

        if (longitud >= 5 && monstruoVerdeActivo)
        {
            Debug.Log("→ Creando Monstruo Verde");
            CrearMonstruo(col, fil, 2);
            return;
        }

        Debug.Log("→ Sin monstruo (longitud < 4 o monstruos desactivados)");
    }

    bool SeCrearáMonstruo(int longitud)
    {
        if (longitud == 4 && monstruoRojoActivo)  return true;
        if (longitud >= 5 && monstruoVerdeActivo) return true;
        return false;
    }

    // ── Convierte una pieza normal en monstruo ────────────
    void CrearMonstruo(int col, int fil, int tipoMonstruo)
    {
        Pieza pieza = tablero[col, fil];
        if (pieza == null) return;

        pieza.esEspecial   = true;
        pieza.tipoEspecial = tipoMonstruo;

        // Cambia el sprite para que se vea diferente
        if (tipoMonstruo == 1 && spriteMonstruoRojo != null)
            pieza.spriteRenderer.sprite = spriteMonstruoRojo;

        if (tipoMonstruo == 2 && spriteMonstruoVerde != null)
            pieza.spriteRenderer.sprite = spriteMonstruoVerde;

        Debug.Log($"Monstruo {(tipoMonstruo == 1 ? "Rojo" : "Verde")} creado en [{col},{fil}]");
    }

    // ── Destruye las piezas marcadas y suma puntos ────────
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

    // ── Revisa si alguna pieza marcada es monstruo ────────
    // Si lo es, activa su efecto ANTES de destruir
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

    // ── Monstruo Rojo: destruye área 3x3 ─────────────────
    void ActivarMonstruoRojo(int centroCol, int centroFil, bool[,] aDestruir)
    {
        Debug.Log($"Monstruo Rojo activado en [{centroCol},{centroFil}] — Explosión en Cruz");

        // Destruye toda la fila horizontal (de izquierda a derecha)
        for (int c = 0; c < columnas; c++)
        {
            aDestruir[c, centroFil] = true;
        }

        // Destruye toda la columna vertical (de arriba a abajo)
        for (int f = 0; f < filas; f++)
        {
            aDestruir[centroCol, f] = true;
        }
    }

    // ── Monstruo Verde: destruye todas las piezas del color vecino
    void ActivarMonstruoVerde(int col, int fil, int colorObjetivo, bool[,] aDestruir)
    {
        Debug.Log($"Monstruo Verde activado — elimina todas las piezas de tipo {colorObjetivo}");

        for (int c = 0; c < columnas; c++)
        {
            for (int f = 0; f < filas; f++)
            {
                if (tablero[c, f] != null && tablero[c, f].tipoPieza == colorObjetivo)
                    aDestruir[c, f] = true;
            }
        }
    }

    // Fórmula de puntos según el tamaño del match
    int CalcularPuntos(int cantidad)
    {
        if (cantidad <= 3) return 30;
        if (cantidad == 4) return 60;
        return 100 + (cantidad - 5) * 20;
    }

    // ─────────────────────────────────────────────────────
    // CAÍDA Y RELLENO
    // ─────────────────────────────────────────────────────

    IEnumerator CaerYRellenar()
    {
        yield return new WaitForSeconds(0.2f);

        // Caída: mueve piezas hacia abajo
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

        // Relleno: crea nuevas piezas donde haya huecos
        for (int col = 0; col < columnas; col++)
            for (int fil = 0; fil < filas; fil++)
                if (tablero[col, fil] == null)
                    CrearPieza(col, fil);

        yield return new WaitForSeconds(0.2f);

        // Cascada: revisa si las nuevas piezas forman matches
        bool nuevoMatch = VerificarMatches();
        if (!nuevoMatch)
            estaProcesando = false;
    }

    // ─────────────────────────────────────────────────────
    // POWER-UPS EXTERNOS
    // ─────────────────────────────────────────────────────

    // Usado por TiendaManager — Martillo
    public void DestruirPiezaEn(int col, int fil)
    {
        if (tablero[col, fil] == null) return;
        Destroy(tablero[col, fil].gameObject);
        tablero[col, fil] = null;
        StartCoroutine(CaerYRellenar());
    }

    // Usado por TiendaManager — Shuffle
    public void MezclarTablero()
    {
        // Recoge todas las piezas
        System.Collections.Generic.List<Pieza> lista = new();
        for (int col = 0; col < columnas; col++)
            for (int fil = 0; fil < filas; fil++)
                if (tablero[col, fil] != null)
                    lista.Add(tablero[col, fil]);

        // Mezcla aleatoria (Fisher-Yates)
        for (int i = lista.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (lista[i], lista[j]) = (lista[j], lista[i]);
        }

        // Reasigna posiciones
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

        GameManager.instancia.AgregarMovimientos(3);
    }

    // ── Destruye todo el tablero y lo regenera ────────────
    public void ReiniciarTablero()
    {
        // Destruye todas las piezas existentes
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

        // Genera el tablero de nuevo
        GenerarTablero();
        estaProcesando = false;
    }
}
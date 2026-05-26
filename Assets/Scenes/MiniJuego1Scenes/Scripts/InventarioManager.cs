// InventarioManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InventarioManager : MonoBehaviour
{
    // ── Singleton ─────────────────────────────────────────
    public static InventarioManager instancia;

    // ── Inventario ────────────────────────────────────────
    [HideInInspector] public int martillos = 0;
    [HideInInspector] public int shuffles  = 0;
    [HideInInspector] public int movExtras = 0;

    // ── UI — se reconectan automáticamente por nombre ─────
    // NO arrastres nada aquí en el Inspector
    // El script los busca solo al cargar GameScene
    private TextMeshProUGUI MartilloT;
    private TextMeshProUGUI SuffleT;
    private TextMeshProUGUI MovExtT;

    // ── Nombres exactos de los GameObjects en el Canvas ───
    private const string NOMBRE_TEXTO_MARTILLOS  = "MartilloT";
    private const string NOMBRE_TEXTO_SHUFFLES   = "SuffleT";
    private const string NOMBRE_TEXTO_MOV_EXTRAS = "MovExtT";

    // ── Claves PlayerPrefs ────────────────────────────────
    private const string CLAVE_MARTILLOS  = "Inv_Martillos";
    private const string CLAVE_SHUFFLES   = "Inv_Shuffles";
    private const string CLAVE_MOV_EXTRAS = "Inv_MovExtras";

    // ─────────────────────────────────────────────────────
    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Cargar();
    }

    void Start()
    {
        // Por si ya estamos en GameScene cuando se inicializa
        if (SceneManager.GetActiveScene().name == "EscenadeJuego")
        {
            ReconectarTextos();
            ActualizarUI();
        }
    }

    void OnEnable()
    {
        // Se suscribe al evento de carga de escena
        SceneManager.sceneLoaded += AlCargarEscena;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= AlCargarEscena;
    }

    // Se ejecuta cada vez que carga cualquier escena
    void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        if (escena.name != "EscenadeJuego") return;

        // Espera un frame para que todos los GameObjects existan
        StartCoroutine(ReconectarConDelay());
    }

    System.Collections.IEnumerator ReconectarConDelay()
    {
        yield return null; // espera 1 frame

        ReconectarTextos();
        ActualizarUI();

        Debug.Log($"UI reconectada — " +
                $"Martillos:{martillos} " +
                $"Shuffles:{shuffles} " +
                $"MovExtras:{movExtras}");
    }

    // Busca los TextMeshPro por el nombre del GameObject
    void ReconectarTextos()
    {
        MartilloT = BuscarTexto(NOMBRE_TEXTO_MARTILLOS);
        SuffleT  = BuscarTexto(NOMBRE_TEXTO_SHUFFLES);
        MovExtT = BuscarTexto(NOMBRE_TEXTO_MOV_EXTRAS);

        Debug.Log($"Textos reconectados — " +
                  $"Martillos:{MartilloT != null} " +
                  $"Shuffles:{SuffleT != null} " +
                  $"MovExtras:{MovExtT != null}");
    }

    // Busca un TextMeshProUGUI por el nombre de su GameObject
    TextMeshProUGUI BuscarTexto(string nombreObjeto)
    {
        GameObject obj = GameObject.Find(nombreObjeto);
        if (obj == null)
        {
            Debug.LogWarning("No encontré el objeto: " + nombreObjeto);
            return null;
        }
        return obj.GetComponent<TextMeshProUGUI>();
    }

    // ── Actualiza los textos en pantalla ──────────────────
    public void ActualizarUI()
    {
        if (MartilloT != null)
            MartilloT.text = "Martillo x" + martillos;

        if (SuffleT != null)
            SuffleT.text = "Shuffle x" + shuffles;

        if (MovExtT != null)
            MovExtT.text = "+3 Mov x" + movExtras;
    }

    // ── Agregar items ─────────────────────────────────────
    public void AgregarMartillo(int cantidad = 1)
    {
        martillos += cantidad;
        Guardar();
        ActualizarUI();
    }

    public void AgregarShuffle(int cantidad = 1)
    {
        shuffles += cantidad;
        Guardar();
        ActualizarUI();
    }

    public void AgregarMovExtras(int cantidad = 1)
    {
        movExtras += cantidad;
        Guardar();
        ActualizarUI();
    }

    // ── Usar items ────────────────────────────────────────
    public bool UsarMartillo()
    {
        if (martillos <= 0) return false;
        martillos--;
        Guardar();
        ActualizarUI();
        return true;
    }

    public bool UsarShuffle()
    {
        if (shuffles <= 0) return false;
        shuffles--;
        Guardar();
        ActualizarUI();
        return true;
    }

    public bool UsarMovExtra()
    {
        if (movExtras <= 0) return false;
        movExtras--;
        Guardar();
        ActualizarUI();
        return true;
    }

    // ── Reiniciar inventario ──────────────────────────────
    public void ReiniciarInventario()
    {
        martillos = 0;
        shuffles  = 0;
        movExtras = 0;
        Guardar();
        ActualizarUI();
        Debug.Log("Inventario reiniciado");
    }

    // ── PlayerPrefs ───────────────────────────────────────
    void Guardar()
    {
        PlayerPrefs.SetInt(CLAVE_MARTILLOS,  martillos);
        PlayerPrefs.SetInt(CLAVE_SHUFFLES,   shuffles);
        PlayerPrefs.SetInt(CLAVE_MOV_EXTRAS, movExtras);
        PlayerPrefs.Save();
    }

    void Cargar()
    {
        martillos = PlayerPrefs.GetInt(CLAVE_MARTILLOS,  0);
        shuffles  = PlayerPrefs.GetInt(CLAVE_SHUFFLES,   0);
        movExtras = PlayerPrefs.GetInt(CLAVE_MOV_EXTRAS, 0);
        Debug.Log($"Inventario cargado — " +
                  $"Martillos:{martillos} " +
                  $"Shuffles:{shuffles} " +
                  $"MovExtras:{movExtras}");
    }

    public void AsignarTextos(
    TextMeshProUGUI txtMartillos,
    TextMeshProUGUI txtShuffles,
    TextMeshProUGUI txtMovExtras)
    {
        MartilloT = txtMartillos;
        SuffleT  = txtShuffles;
        MovExtT = txtMovExtras;

        ActualizarUI(); // actualiza inmediatamente con los valores reales
    }
}
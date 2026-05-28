using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InventarioManager : MonoBehaviour
{
    public static InventarioManager instancia;

    [HideInInspector] public int martillos = 0;
    [HideInInspector] public int shuffles  = 0;
    [HideInInspector] public int movExtras = 0;

    private TextMeshProUGUI MartilloT;
    private TextMeshProUGUI SuffleT;
    private TextMeshProUGUI MovExtT;

    private const string NOMBRE_TEXTO_MARTILLOS  = "MartilloT";
    private const string NOMBRE_TEXTO_SHUFFLES   = "SuffleT";
    private const string NOMBRE_TEXTO_MOV_EXTRAS = "MovExtT";

    private const string CLAVE_MARTILLOS  = "Inv_Martillos";
    private const string CLAVE_SHUFFLES   = "Inv_Shuffles";
    private const string CLAVE_MOV_EXTRAS = "Inv_MovExtras";

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
        if (SceneManager.GetActiveScene().name == "EscenadeJuego")
        {
            ReconectarTextos();
            ActualizarUI();
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += AlCargarEscena;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= AlCargarEscena;
    }

    void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        if (escena.name != "EscenadeJuego") return;

        StartCoroutine(ReconectarConDelay());
    }

    System.Collections.IEnumerator ReconectarConDelay()
    {
        yield return null;

        ReconectarTextos();
        ActualizarUI();
    }

    void ReconectarTextos()
    {
        MartilloT = BuscarTexto(NOMBRE_TEXTO_MARTILLOS);
        SuffleT  = BuscarTexto(NOMBRE_TEXTO_SHUFFLES);
        MovExtT = BuscarTexto(NOMBRE_TEXTO_MOV_EXTRAS);

    }

    TextMeshProUGUI BuscarTexto(string nombreObjeto)
    {
        GameObject obj = GameObject.Find(nombreObjeto);
        if (obj == null)
        {
            return null;
        }
        return obj.GetComponent<TextMeshProUGUI>();
    }

    public void ActualizarUI()
    {
        if (MartilloT != null)
            MartilloT.text = "Martillo x" + martillos;

        if (SuffleT != null)
            SuffleT.text = "Shuffle x" + shuffles;

        if (MovExtT != null)
            MovExtT.text = "+3 Mov x" + movExtras;
    }

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

    public void ReiniciarInventario()
    {
        martillos = 0;
        shuffles  = 0;
        movExtras = 0;
        Guardar();
        ActualizarUI();
    }

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
    }

    public void AsignarTextos(TextMeshProUGUI txtMartillos, TextMeshProUGUI txtShuffles, TextMeshProUGUI txtMovExtras)
    {
        MartilloT = txtMartillos;
        SuffleT  = txtShuffles;
        MovExtT = txtMovExtras;

        ActualizarUI();
    }
}
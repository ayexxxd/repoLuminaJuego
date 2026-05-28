using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    [Header("Movimientos")]
    public int movimientosIniciales = 20;
    private int movimientosRestantes;

    [Header("Puntos")]
    private int puntosActuales = 0; 
    private int puntosTotales = 0; 
    public int puntosGastados = 0;


    [Header("UI")]
    public TextMeshProUGUI textoMovimientos;
    public TextMeshProUGUI textoPuntos;
    public TextMeshProUGUI textoNivel;  
    public TextMeshProUGUI textoMeta;  

    [Header("Niveles")]
    public int nivelActual = 1;
    public int nivelMaximo = 3;

    private int[] metasPorNivel = { 500, 1000, 1500 };

    private int puntosNecesarios;

    void Awake()
    {
        if (instancia == null) instancia = this;
    }

    void Start()
    {
        int nivelGuardado = PlayerPrefs.GetInt("NivelActual", 0);

        if (nivelGuardado > 0)
        {
            nivelActual = nivelGuardado;

            PlayerPrefs.DeleteKey("NivelActual");
            PlayerPrefs.Save();
        }
        else
        {
            nivelActual = 1;
        }

        movimientosRestantes = movimientosIniciales;
        InicializarNivel();
        ActualizarUI();
    }

    public void UsarMovimiento()
    {
        movimientosRestantes--;
        ActualizarUI();

        if (movimientosRestantes <= 0)
            AlQuedarSinMovimientos();
    }

    public void AgregarMovimientos(int cantidad)
    {
        movimientosRestantes += cantidad;
        ActualizarUI();
    }

    void AlQuedarSinMovimientos()
    {
        VerificarFinDeNivel();
    }


    public void AgregarPuntos(int cantidad)
    {
        puntosActuales += cantidad;
        puntosTotales  += cantidad;
        ActualizarUI();

        if (puntosActuales >= puntosNecesarios)
        {
            VerificarFinDeNivel();
        }
    }

    public bool GastarPuntos(int cantidad)
    {
        if (puntosActuales < cantidad) return false;
        puntosActuales  -= cantidad;
        puntosGastados  += cantidad;
        ActualizarUI();
        return true;
    }

    public int ObtenerPuntos() => puntosActuales;

    void ActualizarUI()
    {
        if (textoMovimientos != null)
            textoMovimientos.text = "Movimientos: " + movimientosRestantes;

        if (textoPuntos != null)
            textoPuntos.text = "Puntos: " + puntosActuales;

        if (textoNivel != null)
            textoNivel.text = "Nivel: " + nivelActual;

        if (textoMeta != null)
            textoMeta.text = "Meta: " + puntosActuales + " / " + puntosNecesarios;
    }

    public void InicializarNivel()
    {
        int indice = Mathf.Clamp(nivelActual - 1, 0, metasPorNivel.Length - 1);
        puntosNecesarios = metasPorNivel[indice];

        movimientosRestantes = movimientosIniciales;

        puntosActuales = 0;
        puntosGastados = 0;

        if (Board.instancia != null)
        {
            Board.instancia.monstruoRojoActivo  = nivelActual >= 2;
            Board.instancia.monstruoVerdeActivo = nivelActual >= 3;

            if (nivelActual == 1) Board.instancia.tiposActivosEnNivel = 4;
            if (nivelActual == 2) Board.instancia.tiposActivosEnNivel = 5;
            if (nivelActual == 3) Board.instancia.tiposActivosEnNivel = 6;
        }

        ActualizarUI();
        
    }

    public void VerificarFinDeNivel()
    {
        bool gano = puntosActuales >= puntosNecesarios;

        PlayerPrefs.SetInt("Gano",           gano ? 1 : 0);
        PlayerPrefs.SetInt("PuntosNivel",    puntosActuales);   // puntos de ESTE nivel
        PlayerPrefs.SetInt("PuntosTotales",  puntosTotales);    // acumulado global
        PlayerPrefs.SetInt("PuntosGastados", puntosGastados);
        PlayerPrefs.SetInt("NivelActual",    nivelActual);
        PlayerPrefs.SetInt("EsUltimoNivel",  nivelActual >= nivelMaximo ? 1 : 0);
        PlayerPrefs.Save();

        SceneManager.LoadScene("FinalEscena");
    }

    public void SiguienteNivel()
    {
        if (nivelActual >= nivelMaximo)
        {
            return;
        }

        nivelActual++;
        InicializarNivel();
        Board.instancia.ReiniciarTablero();
    }

    public void ReintentarNivel()
    {
        InicializarNivel();
        Board.instancia.ReiniciarTablero();
    }

    public int ObtenerPuntosNivel()   => puntosActuales;
    public int ObtenerPuntosTotales() => puntosTotales;
    public int ObtenerPuntosNecesarios() => puntosNecesarios;

    
}
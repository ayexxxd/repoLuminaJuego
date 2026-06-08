using UnityEngine;
using UnityEngine.Events;

// PuntosManager controla los puntos dentro de la partida
// y calcula cuántos tokens se generan al terminar
public class PuntosManager : MonoBehaviour
{
    [Header("Configuración de puntos")]

    public int puntosPorVuelta = 100;


    public int puntosPorEstrella = 25;


    public int puntosPorVictoria = 200;


    public int puntosPorSegundo = 2;

    [Header("Conversión a tokens")]

    public int puntosPorToken = 50;

    [Header("Estado actual (solo lectura)")]

    public int puntosActuales = 0;


    public int tokensGenerados = 0;

    public UnityEvent<int> onPuntosCambiaron;


    public static PuntosManager instancia;


    private UIManager uiManager;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();

        puntosActuales = 0;
        tokensGenerados = 0;


        ActualizarUI();
    }


    public void AgregarPuntosPorVuelta()
    {
        AgregarPuntos(puntosPorVuelta);
        Debug.Log("Puntos por vuelta: +" + puntosPorVuelta +
                  " | Total: " + puntosActuales);
    }


    public void AgregarPuntosPorEstrella()
    {
        AgregarPuntos(puntosPorEstrella);
        Debug.Log("Puntos por estrella: +" + puntosPorEstrella +
                  " | Total: " + puntosActuales);
    }


    public void AgregarPuntosPorVictoria(float tiempoRestante)
    {
        AgregarPuntos(puntosPorVictoria);


        int bonusTiempo = Mathf.FloorToInt(tiempoRestante) * puntosPorSegundo;
        AgregarPuntos(bonusTiempo);

        Debug.Log("Puntos por victoria: +" + puntosPorVictoria);
        Debug.Log("Bonus de tiempo: +" + bonusTiempo +
                  " (" + Mathf.FloorToInt(tiempoRestante) + "s x " + puntosPorSegundo + ")");
        Debug.Log("Total final: " + puntosActuales);
    }

    void AgregarPuntos(int cantidad)
    {
        puntosActuales += cantidad;

        onPuntosCambiaron?.Invoke(puntosActuales);

        ActualizarUI();
    }


    public int CalcularTokens()
    {

        tokensGenerados = puntosActuales / puntosPorToken;

        Debug.Log("Puntos totales: " + puntosActuales);
        Debug.Log("Tokens generados: " + tokensGenerados +
                " (" + puntosActuales + " / " + puntosPorToken + ")");

        return tokensGenerados;
    }

    public int ObtenerPuntos()
    {
        return puntosActuales;
    }

    void ActualizarUI()
    {
        if (uiManager != null)
        {
            uiManager.ActualizarPuntos(puntosActuales);
        }
    }
}
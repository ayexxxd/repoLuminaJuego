using UnityEngine;
using UnityEngine.Events;


public class TimerManager : MonoBehaviour
{
    [Header("Configuración")]
    public float tiempoTotal = 60f;

    [Header("Estado actual")]
    public float tiempoRestante;

    private bool timerActivo = false;
    private bool yaTermino = false;

    private CarrerasGameManager gameManager;
    private UIManager uiManager;

    public UnityEvent onTiempoAgotado;

    void Start()
    {
        tiempoRestante = tiempoTotal;
        yaTermino = false;

        gameManager = FindObjectOfType<CarrerasGameManager>();

        if (gameManager == null)
        {
            Debug.LogError("TimerManager: NO se encontró el GameManager en la escena.");
        }
        else
        {
            Debug.Log("TimerManager: GameManager encontrado correctamente.");
        }

        uiManager = FindObjectOfType<UIManager>();

        ActualizarUI();

        timerActivo = true;
        Debug.Log("Timer iniciado: " + tiempoTotal + " segundos.");
    }

    void Update()
    {
        if (!timerActivo || yaTermino) return;

        tiempoRestante -= Time.deltaTime;

        ActualizarUI();

        if (tiempoRestante <= 0f)
        {
            tiempoRestante = 0f;
            timerActivo = false;
            yaTermino = true;

            Debug.Log("¡TIEMPO AGOTADO! Llamando al GameManager...");

            onTiempoAgotado?.Invoke();

            if (gameManager != null)
            {
                gameManager.JugadorPerdioTiempo();
            }
            else
            {
                Debug.LogError("TimerManager: GameManager es null. No se puede procesar derrota.");
            }
        }
    }

    void ActualizarUI()
    {
        if (uiManager != null)
        {
            uiManager.ActualizarTimer(tiempoRestante);
        }
    }

    public void DetenerTimer()
    {
        timerActivo = false;
        yaTermino = true;
        Debug.Log("Timer detenido. Tiempo restante: " + tiempoRestante);
    }

    public void PausarTimer()
    {
        timerActivo = false;
    }
    public void ReanudarTimer()
    {
        if (!yaTermino)
            timerActivo = true;
    }
    public void AgregarTiempo(float segundos)
    {
        tiempoRestante += segundos;
        tiempoRestante = Mathf.Min(tiempoRestante, tiempoTotal);
        ActualizarUI();
        Debug.Log("Tiempo extra: +" + segundos + "s. Ahora: " + tiempoRestante);
    }


    public float ObtenerTiempoTranscurrido()
    {
        return tiempoTotal - tiempoRestante;
    }
}
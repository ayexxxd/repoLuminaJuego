using UnityEngine;
using UnityEngine.Events;

// PuntosManager controla los puntos dentro de la partida
// y calcula cuántos tokens se generan al terminar
public class PuntosManager : MonoBehaviour
{
    [Header("Configuración de puntos")]
    // Cuántos puntos da completar una vuelta
    public int puntosPorVuelta = 100;

    // Cuántos puntos da recoger una estrella
    public int puntosPorEstrella = 25;

    // Cuántos puntos extra da ganar la carrera
    public int puntosPorVictoria = 200;

    // Cuántos puntos da cada segundo restante al ganar
    public int puntosPorSegundo = 2;

    [Header("Conversión a tokens")]
    // Cuántos puntos equivalen a 1 token
    public int puntosPorToken = 50;

    [Header("Estado actual (solo lectura)")]
    // Puntos acumulados en esta partida
    public int puntosActuales = 0;

    // Tokens generados en esta partida
    public int tokensGenerados = 0;

    // Evento que avisa cuando cambian los puntos
    // El int es la nueva cantidad de puntos
    public UnityEvent<int> onPuntosCambiaron;

    // Singleton para acceso fácil
    public static PuntosManager instancia;

    // Referencia al UIManager
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

        // Inicializamos los puntos en 0
        puntosActuales = 0;
        tokensGenerados = 0;

        // Actualizamos la UI
        ActualizarUI();
    }

    // ============================================================
    // MÉTODOS PARA AGREGAR PUNTOS
    // ============================================================

    // ---- Agrega puntos por completar una vuelta ----
    // Llamado por el LapManager
    public void AgregarPuntosPorVuelta()
    {
        AgregarPuntos(puntosPorVuelta);
        Debug.Log("Puntos por vuelta: +" + puntosPorVuelta +
                  " | Total: " + puntosActuales);
    }

    // ---- Agrega puntos por recoger una estrella ----
    // Llamado por el PowerUpManager
    public void AgregarPuntosPorEstrella()
    {
        AgregarPuntos(puntosPorEstrella);
        Debug.Log("Puntos por estrella: +" + puntosPorEstrella +
                  " | Total: " + puntosActuales);
    }

    // ---- Agrega puntos por ganar y por tiempo restante ----
    // Llamado por el GameManager cuando el jugador gana
    public void AgregarPuntosPorVictoria(float tiempoRestante)
    {
        // Puntos base por ganar
        AgregarPuntos(puntosPorVictoria);

        // Puntos bonus por el tiempo que sobró
        int bonusTiempo = Mathf.FloorToInt(tiempoRestante) * puntosPorSegundo;
        AgregarPuntos(bonusTiempo);

        Debug.Log("Puntos por victoria: +" + puntosPorVictoria);
        Debug.Log("Bonus de tiempo: +" + bonusTiempo +
                  " (" + Mathf.FloorToInt(tiempoRestante) + "s x " + puntosPorSegundo + ")");
        Debug.Log("Total final: " + puntosActuales);
    }

    // ---- Método interno que suma puntos y actualiza la UI ----
    void AgregarPuntos(int cantidad)
    {
        puntosActuales += cantidad;

        // Disparamos el evento para que la UI se actualice
        onPuntosCambiaron?.Invoke(puntosActuales);

        // Actualizamos la UI directamente también
        ActualizarUI();
    }

    // ============================================================
    // CONVERSIÓN A TOKENS
    // ============================================================

    // ---- Calcula y devuelve los tokens generados en esta partida ----
    // Llamado por el GameManager al terminar la partida
    public int CalcularTokens()
    {
        // División entera: 350 puntos / 50 = 7 tokens (se pierden los 0 sobrantes)
        tokensGenerados = puntosActuales / puntosPorToken;

        Debug.Log("Puntos totales: " + puntosActuales);
        Debug.Log("Tokens generados: " + tokensGenerados +
                " (" + puntosActuales + " / " + puntosPorToken + ")");

        return tokensGenerados;
    }

    // ---- Devuelve los puntos actuales ----
    public int ObtenerPuntos()
    {
        return puntosActuales;
    }

    // ---- Actualiza el texto de puntos en la UI ----
    void ActualizarUI()
    {
        if (uiManager != null)
        {
            uiManager.ActualizarPuntos(puntosActuales);
        }
    }
}
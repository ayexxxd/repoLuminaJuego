// MonedasManager.cs
// Maneja las monedas del jugador.
// Guarda localmente con PlayerPrefs por ahora.
// Para conectar API en el futuro: reemplaza GuardarMonedas()
// y CargarMonedas() sin tocar el resto de la lógica.

using UnityEngine;

public class MonedasManager : MonoBehaviour
{
    // ── Singleton ─────────────────────────────────────────
    public static MonedasManager instancia;

    // ── Configuración ─────────────────────────────────────
    [Header("Configuración")]
    public int puntosPorMoneda = 10; // Cada 10 puntos = 1 moneda

    // ── Estado interno ────────────────────────────────────
    private int monedasActuales = 0;

    // ── Clave para PlayerPrefs ────────────────────────────
    // Si cambias a API, solo toca CargarMonedas() y GuardarMonedas()
    private const string CLAVE_MONEDAS = "Monedas";

    // ─────────────────────────────────────────────────────
    void Awake()
    {
        // Singleton que sobrevive entre escenas
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

        CargarMonedas();
    }

    // ── Convierte puntos sobrantes a monedas ──────────────
    // Llamado desde GameManager al terminar el nivel
    public int ConvertirPuntosAMonedas(int puntosRestantes)
    {
        if (puntosRestantes <= 0) return 0;

        int monedasGanadas = puntosRestantes / puntosPorMoneda;

        Debug.Log($"Puntos sobrantes: {puntosRestantes} " +
                  $"→ {monedasGanadas} monedas nuevas");

        if (monedasGanadas > 0)
        {
            monedasActuales += monedasGanadas;
            GuardarMonedas();
        }

        return monedasGanadas;
    }

    // ── Getters ───────────────────────────────────────────
    public int ObtenerMonedas() => monedasActuales;

    // ── PUNTO DE EXTENSIÓN PARA API ───────────────────────
    // Reemplaza estos dos métodos cuando tengas backend.
    // El resto del código no necesita cambiar.

    void GuardarMonedas()
    {
        // TODO: reemplazar por llamada a API
        PlayerPrefs.SetInt(CLAVE_MONEDAS, monedasActuales);
        PlayerPrefs.Save();
        Debug.Log("Monedas guardadas: " + monedasActuales);
    }

    void CargarMonedas()
    {
        // TODO: reemplazar por llamada a API
        monedasActuales = PlayerPrefs.GetInt(CLAVE_MONEDAS, 0);
        Debug.Log("Monedas cargadas: " + monedasActuales);
    }
}
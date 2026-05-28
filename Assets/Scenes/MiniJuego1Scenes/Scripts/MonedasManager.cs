using UnityEngine;

public class MonedasManager : MonoBehaviour
{
    public static MonedasManager instancia;

    [Header("Configuración")]
    public int puntosPorMoneda = 10;

    private int monedasActuales = 0;

    private const string CLAVE_MONEDAS = "Monedas";

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

        CargarMonedas();
    }
    public int ConvertirPuntosAMonedas(int puntosRestantes)
    {
        if (puntosRestantes <= 0) return 0;

        int monedasGanadas = puntosRestantes / puntosPorMoneda;


        if (monedasGanadas > 0)
        {
            monedasActuales += monedasGanadas;
            GuardarMonedas();
        }

        return monedasGanadas;
    }

    public int ObtenerMonedas() => monedasActuales;

    void GuardarMonedas()
    {
        PlayerPrefs.SetInt(CLAVE_MONEDAS, monedasActuales);
        PlayerPrefs.Save();
        
    }

    void CargarMonedas()
    {
        monedasActuales = PlayerPrefs.GetInt(CLAVE_MONEDAS, 0);
    }
}
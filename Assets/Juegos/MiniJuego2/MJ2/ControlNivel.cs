using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ControlNivel : MonoBehaviour
{
    [Header("Monedas")]
    public int totalMonedas = 6;
    public int monedasRecolectadas = 0;
    public TMP_Text textoMonedas;
    public GameObject[] monedas;

    [Header("Tiempo")]
    public float tiempoInicial = 50f;
    public TMP_Text textoTiempo;
    private float tiempoActual;
    private bool nivelActivo = true;

    [Header("Panel tiempo terminado")]
    public GameObject panelTiempoTerminado;

    [Header("Panel advertencia")]
    public GameObject panelAdvertencia;

    [Header("Información del nivel")]
    public int numeroNivel = 1;

    void Start()
    {
        monedasRecolectadas = 0;

        tiempoActual = tiempoInicial;
        nivelActivo = true;

        ActualizarTextoMonedas();
        ActualizarTextoTiempo();

        ActivarSoloPrimeraMoneda();



        if (panelTiempoTerminado != null)
        {
            panelTiempoTerminado.SetActive(false);
        }

        if (panelAdvertencia != null && DebeMostrarAdvertencia())
        {
            panelAdvertencia.SetActive(true);
            nivelActivo = false;
        }
        else
        {
            if (panelAdvertencia != null)
            {
                panelAdvertencia.SetActive(false);
            }

            nivelActivo = true;
        }
    }


    bool DebeMostrarAdvertencia()
    {
        if (numeroNivel == 1 && DatosJuego.instruccionesEN1Vista == false)
        {
            return true;
        }

        if (numeroNivel == 2 && DatosJuego.advertenciaEN2Vista == false)
        {
            return true;
        }

        if (numeroNivel == 3 && DatosJuego.advertenciaEN3Vista == false)
        {
            return true;
        }

        return false;
    }

    void Update()
    {
        if (nivelActivo)
        {
            ContarTiempo();
        }
    }

    void ContarTiempo()
    {
        tiempoActual -= Time.deltaTime;

        if (tiempoActual <= 0)
        {
            tiempoActual = 0;
            nivelActivo = false;

            if (panelTiempoTerminado != null)
            {
                panelTiempoTerminado.SetActive(true);
            }
        }

        ActualizarTextoTiempo();
    }


    public void CerrarPanelAdvertencia()
    {
        if (panelAdvertencia != null)
        {
            panelAdvertencia.SetActive(false);
        }

        if (numeroNivel == 1)
        {
            DatosJuego.instruccionesEN1Vista = true;
        }
        else if (numeroNivel == 2)
        {
            DatosJuego.advertenciaEN2Vista = true;
        }
        else if (numeroNivel == 3)
        {
            DatosJuego.advertenciaEN3Vista = true;
        }

        nivelActivo = true;
    }

    public void RecolectarMoneda()
    {
        if (nivelActivo == false)
        {
            return;
        }

        monedasRecolectadas++;

        ActualizarTextoMonedas();

        ActivarSiguienteMoneda();
    }

    void ActualizarTextoMonedas()
    {
        textoMonedas.text = monedasRecolectadas + "/" + totalMonedas;
    }

    void ActualizarTextoTiempo()
    {
        textoTiempo.text = Mathf.CeilToInt(tiempoActual).ToString();
    }

    void ActivarSoloPrimeraMoneda()
    {
        for (int i = 0; i < monedas.Length; i++)
        {
            monedas[i].SetActive(false);
        }

        if (monedas.Length > 0)
        {
            monedas[0].SetActive(true);
        }
    }

    void ActivarSiguienteMoneda()
    {
        if (monedasRecolectadas < monedas.Length)
        {
            monedas[monedasRecolectadas].SetActive(true);
        }
    }

    public bool YaRecolectoTodas()
    {
        return monedasRecolectadas >= totalMonedas;
    }
    public bool FaltaUltimaMoneda()
    {
        return monedasRecolectadas == totalMonedas - 1;
    }

    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RegresarInicio()
    {
        SceneManager.LoadScene("EMI");
    }
}
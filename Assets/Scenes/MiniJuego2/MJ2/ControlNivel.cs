using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ControlNivel : MonoBehaviour
{
    [Header("Monedas")] // //////////////////
    public int totalMonedas = 6;
    public int monedasRecolectadas = 0;
    public TMP_Text textoMonedas;
    public GameObject[] monedas; // este es el arreglo en donde aparecerá moneda tras moneda si las arrastras
 // //////////////////


    [Header("Tiempo")]  // //////////////////
    public float tiempoInicial = 50f;
    public TMP_Text textoTiempo;
    private float tiempoActual;
    private bool nivelActivo = true; // para saber si el nivel está corriendo o en pausa, si está corriendo empieza a bajar el tiempo sino se mantiene 
 // //////////////////



    [Header("Panel tiempo terminado")]  // //////////////////
    public GameObject panelTiempoTerminado;
 // //////////////////



    [Header("Panel advertencia")]  // //////////////////
    public GameObject panelAdvertencia;
 // //////////////////


    [Header("Información del nivel")]  // //////////////////
    public int numeroNivel = 1;
 // //////////////////



    void Start()
    {
        monedasRecolectadas = 0;

        tiempoActual = tiempoInicial;
        nivelActivo = true;

        ActualizarTextoMonedas();
        ActualizarTextoTiempo();

        ActivarSoloPrimeraMoneda(); // esta es la función que hace que solo aparezca la primera moneda, o sea como que apaga las demas 



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
 // //////////////////



// Sirve para saber si el panel del nivel ya fue visto o no y en caso de que si que ya no se repita
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
 // //////////////////



// si el nivel está activo, el tiempo sigue bajando constantemente 
    void Update()
    {
        if (nivelActivo)
        {
            ContarTiempo();
        }
    }
// /////////////////


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
 // //////////////////


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



// este metodo se llama desde el script Moneda
    public void RecolectarMoneda()
    {

        if (nivelActivo == false) // revisa si el nivel está activo
        {
            return;
        }

        monedasRecolectadas++; // cada que recolecta monedas las va sumando

        ActualizarTextoMonedas();

        ActivarSiguienteMoneda();
    }
 // //////////////////


    void ActualizarTextoMonedas()
    {
        textoMonedas.text = monedasRecolectadas + "/" + totalMonedas;
    }
 // //////////////////

    void ActualizarTextoTiempo()
    {
        textoTiempo.text = Mathf.CeilToInt(tiempoActual).ToString();
    } // ese de mathf es para que se vea como un numero entero en la pantalla
 // //////////////////



// apaga todas las monedas del arreglo y solo prende la primera
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
 // //////////////////



// prende la moneda que sigue segun la cantidad recolectada
    void ActivarSiguienteMoneda()
    {
        if (monedasRecolectadas < monedas.Length)
        {
            monedas[monedasRecolectadas].SetActive(true);
        }
    }
 // //////////////////


// este es el que usa la PUERTA
// la puerta se activa solo si ya recolectó todas las monedas
    public bool YaRecolectoTodas()
    {
        return monedasRecolectadas >= totalMonedas;
    }
 // //////////////////



// este es el que usa la PALANCA
// la palanca se activa solo cuando falte la última moneda 
    public bool FaltaUltimaMoneda()
    {
        return monedasRecolectadas == totalMonedas - 1;
    }
 // //////////////////


// para los botones que aparecen de reiniiar y regresar a home
    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RegresarInicio()
    {
        SceneManager.LoadScene("EMI");
    }
}
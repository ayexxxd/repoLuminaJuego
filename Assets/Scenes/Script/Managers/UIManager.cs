using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{
    [Header("Referencias a los textos del HUD")]

    public TextMeshProUGUI textoVueltas;
    public TextMeshProUGUI textoTimer;
    public TextMeshProUGUI textoVidas;

    public TextMeshProUGUI textoMensaje;
    

    public TextMeshProUGUI textoPuntos;

    [Header("Configuración")]

    public int vueltasTotales = 3;


    private LapManager lapManager;

    void Start()
    {

        void Start()
        {
            lapManager = Object.FindAnyObjectByType<LapManager>();

            MostrarMensajeTemporal("FUNCIONA!");

            ActualizarVueltas(1);
            ActualizarVidas(3);
            ActualizarTimer(60f);
        }
        lapManager = Object.FindAnyObjectByType<LapManager>();
        
        if (lapManager == null)
        {
            Debug.LogWarning("UIManager: No se encontró un LapManager en la escena. ¡Asegúrate de que exista!");
        }

        // Actualizamos la UI con los valores iniciales
        ActualizarVueltas(1);
        ActualizarVidas(3);
        ActualizarTimer(60f);
    }

    // ---- Actualiza el texto de puntos en pantalla ----
    public void ActualizarPuntos(int puntos)
    {
        if (textoPuntos != null)
        {
            textoPuntos.text = "Puntos:  " + puntos;
        }
    }

    // ---- Actualiza el texto de vueltas en pantalla ----
    // Llamado por el LapManager cuando se completa una vuelta
    public void ActualizarVueltas(int vueltaActual)
    {
        if (textoVueltas != null)
        {
            // Mostramos "VUELTA 2 / 3" por ejemplo
            textoVueltas.text = "VUELTA " + vueltaActual + " / " + vueltasTotales;
        }
        else
        {
            Debug.LogWarning("UIManager: textoVueltas no está asignado en el Inspector.");
        }
    }

    // ---- Actualiza el texto del timer en pantalla ----
    // Llamado por el TimerManager en cada segundo
    public void ActualizarTimer(float segundosRestantes)
    {
        if (textoTimer != null)
        {
            // Mathf.CeilToInt redondea hacia arriba para mostrar número entero
            int segundos = Mathf.CeilToInt(segundosRestantes);
            textoTimer.text = "Tiempo:  " + segundos;

            // Cambiamos el color a rojo cuando queden menos de 10 segundos
            if (segundos <= 10)
            {
                textoTimer.color = Color.red;
            }
            else
            {
                textoTimer.color = Color.white;
            }
        }
    }

    public void ActualizarVidas(int vidasActuales)
    {
        if (textoVidas != null)
        {
            // Construimos la cadena de corazones dinámicamente
            string corazones = "";
            for (int i = 0; i < vidasActuales; i++)
            {
                corazones += " ";
            }
            textoVidas.text = corazones;
        }
    }

    // ---- Muestra el resumen de puntos y tokens al ganar ----
    public void MostrarResumenVictoria(int puntos, int tokens)
    {
        // Construimos el mensaje de resumen
        string resumen = " ¡VICTORIA!\n" +
                        "Puntos: " + puntos + "\n" +
                        "Tokens: " + tokens;

        // Usamos el texto de mensaje para mostrarlo
        MostrarMensajeTemporal(resumen, 3f);

        Debug.Log("Resumen mostrado: " + resumen);
    }


    public void MostrarMensajeTemporal(string mensaje, float duracion = 2f)
    {
        // Cancelamos cualquier mensaje anterior que esté mostrándose
        StopCoroutine("CorrutinaMensaje");
        StartCoroutine(CorrutinaMensaje(mensaje, duracion));
    }

    System.Collections.IEnumerator CorrutinaMensaje(string mensaje, float duracion)
    {
        if (textoMensaje != null)
        {
            // Mostramos el mensaje
            textoMensaje.text = mensaje;
            textoMensaje.gameObject.SetActive(true);

            yield return new WaitForSeconds(duracion);

            // Ocultamos el texto después de la duración
            textoMensaje.gameObject.SetActive(false);
            textoMensaje.text = "";
        }
        else
        {
            // Si no hay texto asignado, al menos lo mostramos en consola
            Debug.Log("MENSAJE: " + mensaje);
            yield return new WaitForSeconds(duracion);
        }
    }
}
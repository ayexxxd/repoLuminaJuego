// GestorPreguntas.cs
// Muestra preguntas cada N movimientos. Listo para conectar API después.

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GestorPreguntas : MonoBehaviour
{
    // ── Singleton ─────────────────────────────────────────
    public static GestorPreguntas instancia;

    // ── UI ────────────────────────────────────────────────
    [Header("Panel de Pregunta")]
    public GameObject panelPreguntas;
    public TextMeshProUGUI textoPregunta;
    public Button[]        botonesRespuesta;   // 4 botones en el Inspector
    public TextMeshProUGUI[] textoBotones;     // TMP de cada botón

    // ── Preguntas locales ─────────────────────────────────
    [Header("Preguntas Locales")]
    public Pregunta[] preguntasLocales;

    // ── Configuración ─────────────────────────────────────
    [Header("Configuración")]
    public int movimientosPorPregunta = 5;
    public int movimientosBonus       = 3;

    // ── Estado interno ────────────────────────────────────
    private int contadorMovimientos   = 0;
    private int indicePregunta        = 0;
    private Pregunta preguntaActual;

    // ─────────────────────────────────────────────────────
    void Awake()
    {
        if (instancia == null) instancia = this;
    }

    // Llamado desde Board.cs tras cada movimiento válido
    public void RegistrarMovimiento()
    {
        contadorMovimientos++;
        if (contadorMovimientos >= movimientosPorPregunta)
        {
            contadorMovimientos = 0;
            StartCoroutine(CargarYMostrar());
        }
    }

    // ── PUNTO DE EXTENSIÓN ────────────────────────────────
    // Para API en el futuro: comenta CargarPreguntaLocal()
    // y descomenta ObtenerPreguntaDesdeAPI()
    IEnumerator CargarYMostrar()
    {
        preguntaActual = CargarPreguntaLocal();
        // yield return StartCoroutine(ObtenerPreguntaDesdeAPI());

        MostrarPregunta(preguntaActual);
        yield return null;
    }

    Pregunta CargarPreguntaLocal()
    {
        if (preguntasLocales == null || preguntasLocales.Length == 0)
        {
            return new Pregunta
            {
                textoPregunta  = "¿Pregunta de prueba?",
                respuestas     = new string[] { "A", "B", "C", "D" },
                indiceCorrecta = 0
            };
        }

        Pregunta p = preguntasLocales[indicePregunta % preguntasLocales.Length];
        indicePregunta++;
        return p;
    }

    // Stub para API futura
    // IEnumerator ObtenerPreguntaDesdeAPI() { ... }

    // ── Muestra el panel ──────────────────────────────────
    void MostrarPregunta(Pregunta p)
    {
        Time.timeScale = 0f;
        panelPreguntas.SetActive(true);

        // Si hay TriviaController activo, él llena la UI desde el API
        if (TriviaController.instancia != null)
        {
            // Solo pausa y abre el panel — TriviaController llena los textos
            // cuando termina GetData()
            StartCoroutine(TriviaController.instancia.GetData());
            return;
        }

        // Fallback: llena con preguntas locales si no hay API
        textoPregunta.text = p.textoPregunta;

        for (int i = 0; i < botonesRespuesta.Length; i++)
        {
            textoBotones[i].text = p.respuestas[i];
            botonesRespuesta[i].onClick.RemoveAllListeners();

            int indiceCapturado = i;
            int correcto        = p.indiceCorrecta;
            botonesRespuesta[i].onClick.AddListener(() =>
                AlResponder(indiceCapturado, correcto));
        }
    }

    void AlResponder(int seleccionado, int correcto)
    {
        panelPreguntas.SetActive(false);
        Time.timeScale = 1f;

        if (seleccionado == correcto)
        {
            Debug.Log("¡Correcto! +" + movimientosBonus + " movimientos.");
            GameManager.instancia.AgregarMovimientos(movimientosBonus);

            // ── ELIMINADO: antes abría TiendaManager.instancia.AbrirTienda()
            // La tienda ahora solo aparece en ResultadoScene
        }
        else
        {
            Debug.Log("Incorrecto. Continúa jugando.");
        }
    }

    // Llamado desde TriviaController cuando la respuesta es correcta
    public void RespuestaCorrecta()
    {
        panelPreguntas.SetActive(false);
        Time.timeScale = 1f;

        Debug.Log("¡Correcto! +" + movimientosBonus + " movimientos.");
        GameManager.instancia.AgregarMovimientos(movimientosBonus);
    }

    // Llamado desde TriviaController cuando la respuesta es incorrecta
    public void RespuestaIncorrecta()
    {
        panelPreguntas.SetActive(false);
        Time.timeScale = 1f;

        Debug.Log("Incorrecto. Continúa jugando.");
    }
}
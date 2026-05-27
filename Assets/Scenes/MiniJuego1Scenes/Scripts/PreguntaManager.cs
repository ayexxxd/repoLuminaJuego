// GestorPreguntas.cs
// Muestra preguntas cada N movimientos. Listo para conectar API después.

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GestorPreguntas : MonoBehaviour
{
    public static GestorPreguntas instancia;

    [Header("Panel de Pregunta")]
    public GameObject panelPreguntas;
    public TextMeshProUGUI textoPregunta;
    public Button[] botonesRespuesta;  
    public TextMeshProUGUI[] textoBotones;

    [Header("Preguntas")]
    public Pregunta[] preguntasLocales;

    [Header("Configuración")]
    public int movimientosPorPregunta = 5;
    public int movimientosBonus       = 3;

    private int contadorMovimientos   = 0;
    private int indicePregunta        = 0;
    private Pregunta preguntaActual;

    void Awake()
    {
        if (instancia == null) instancia = this;
    }

    public void RegistrarMovimiento()
    {
        contadorMovimientos++;
        if (contadorMovimientos >= movimientosPorPregunta)
        {
            contadorMovimientos = 0;
            StartCoroutine(CargarYMostrar());
        }
    }


    IEnumerator CargarYMostrar()
    {
        preguntaActual = CargarPreguntaLocal();

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

    void MostrarPregunta(Pregunta p)
    {
        Time.timeScale = 0f;
        panelPreguntas.SetActive(true);

        if (TriviaController.instancia != null)
        {
            StartCoroutine(TriviaController.instancia.GetData());
            return;
        }

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
        }
        else
        {
            Debug.Log("Incorrecto. Continúa jugando.");
        }
    }

    public void RespuestaCorrecta()
    {
        panelPreguntas.SetActive(false);
        Time.timeScale = 1f;

        Debug.Log("¡Correcto! +" + movimientosBonus + " movimientos.");
        GameManager.instancia.AgregarMovimientos(movimientosBonus);
    }

    public void RespuestaIncorrecta()
    {
        panelPreguntas.SetActive(false);
        Time.timeScale = 1f;

        Debug.Log("Incorrecto. Continúa jugando.");
    }
}
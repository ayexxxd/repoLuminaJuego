using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;

public class TriviaController : MonoBehaviour
{
    [Header("UI Components (Trivia MJ1)")]
    public TextMeshProUGUI DialogueText;   // El texto grande donde se lee la PREGUNTA
    public TextMeshProUGUI[] BotonesText; // Tus 4 componentes de texto de los botones de respuestas

    private List<PreguntaTrivia> listaRespuestas; 
    private int indiceCorrecto = 0; 

    public static TriviaController instancia; // ← AGREGA

    void Awake()                              // ← AGREGA
    {                                         // ← AGREGA
        if (instancia == null) instancia = this; // ← AGREGA
    }   

    void Start()
    {
        // Al arrancar la escena, pide la pregunta de la trivia de inmediato
        //StartCoroutine(GetData());
    }

    public IEnumerator GetData()
    {
        
        // Tu URL usando la IP de tu red local y la ruta de la trivia MJ1
        string JSONurl = "https://10.22.207.200:5001/MJ1/preguntas";

        UnityWebRequest web = UnityWebRequest.Get(JSONurl);
        web.certificateHandler = new ForceAcceptAll(); // Ignora el SSL local inseguro
        yield return web.SendWebRequest();

        if (web.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error API Trivia MJ1: " + web.error);
        }
        else
        {
            // Deserializamos el JSON en una lista, idéntico a tus canciones
            listaRespuestas = JsonConvert.DeserializeObject<List<PreguntaTrivia>>(web.downloadHandler.text);
            
            if (listaRespuestas != null && listaRespuestas.Count > 0)
            {
                // Como todas las filas de la consulta traen la misma pregunta, 
                // ponemos el texto de la primera fila en el cuadro grande
                DialogueText.text = listaRespuestas[0].Pregunta;

                // Repartimos las respuestas en los textos de tus 4 botones
                for (int i = 0; i < listaRespuestas.Count; i++)
                {
                    if (i < BotonesText.Length)
                    {
                        BotonesText[i].text = listaRespuestas[i].Respuesta;
                        
                        // Si esta opción vale 1, guardamos el índice del botón correcto
                        if (listaRespuestas[i].EsCorrecta == 1)
                        {
                            indiceCorrecto = i;
                        }
                        
                        Debug.Log($"Opción {i}: {listaRespuestas[i].Respuesta} | Correcta: {listaRespuestas[i].EsCorrecta}");
                    }
                }
            }
        }
    }

    // ── Función para conectar a los clicks de tus botones de respuestas ──
    public void ValidarRespuesta(int botonPresionado)
    {
        if (botonPresionado == indiceCorrecto)
        {
            Debug.Log("Respuesta correcta");

            // Avisa a GestorPreguntas que fue correcta
            // Él maneja puntos, movimientos y todo lo demás
            if (GestorPreguntas.instancia != null)
                GestorPreguntas.instancia.RespuestaCorrecta();
            else
                Debug.LogWarning("GestorPreguntas.instancia es null");
        }
        else
        {
            Debug.Log("Respuesta incorrecta");

            if (GestorPreguntas.instancia != null)
                GestorPreguntas.instancia.RespuestaIncorrecta();
            else
                Debug.LogWarning("GestorPreguntas.instancia es null");
        }
    }

    
}

// ── Clase para aceptar el certificado local de Flask ──
public class ForceAcceptAll : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true; 
    }
}
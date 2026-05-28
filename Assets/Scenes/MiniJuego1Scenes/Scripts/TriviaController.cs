using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;

public class TriviaController : MonoBehaviour
{
    [Header("Trivia MJ1")]
    public TextMeshProUGUI DialogueText;   
    public TextMeshProUGUI[] BotonesText; 

    private List<PreguntaTrivia> listaRespuestas; 
    private int indiceCorrecto = 0; 

    public static TriviaController instancia; 

    void Awake()                                             
    {                                                         
        if (instancia == null) instancia = this; 
    }   

    void Start()
    {
        // ARREGLO 1: Arrancamos la petición a la API al iniciar el juego
        StartCoroutine(GetData());
    }

    public IEnumerator GetData()
    {
        string JSONurl = "https://10.22.207.200:5001/MJ1/preguntas";

        // Usamos "using" para liberar la memoria de la petición web cuando termine
        using (UnityWebRequest web = UnityWebRequest.Get(JSONurl))
        {
            web.certificateHandler = new ForceAcceptAll(); 
            
            yield return web.SendWebRequest();

            if (web.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error API Trivia MJ1: " + web.error);
            }
            else
            {
                string jsonResult = web.downloadHandler.text;
                Debug.Log("JSON Recibido: " + jsonResult); // Para que verifiques qué te llega

                listaRespuestas = JsonConvert.DeserializeObject<List<PreguntaTrivia>>(jsonResult);
                
                if (listaRespuestas != null && listaRespuestas.Count > 0)
                {
                    // Asignamos el texto de la primera pregunta
                    DialogueText.text = listaRespuestas[0].Pregunta;

                    // ARREGLO 2: Controlamos que no se pase del límite de botones reales que tienes en escena
                    int limite = Mathf.Min(listaRespuestas.Count, BotonesText.Length);

                    for (int i = 0; i < limite; i++)
                    {
                        BotonesText[i].text = listaRespuestas[i].Respuesta;
                        
                        if (listaRespuestas[i].EsCorrecta == 1)
                        {
                            indiceCorrecto = i;
                        }
                    }
                }
            }
        }
    }

    public void ValidarRespuesta(int botonPresionado)
    {
        if (botonPresionado == indiceCorrecto)
        {
            Debug.Log("Respuesta correcta");
            if (GestorPreguntas.instancia != null)
                GestorPreguntas.instancia.RespuestaCorrecta();
        }
        else
        {
            Debug.Log("Respuesta incorrecta");
            if (GestorPreguntas.instancia != null)
                GestorPreguntas.instancia.RespuestaIncorrecta();
        }
    }
}
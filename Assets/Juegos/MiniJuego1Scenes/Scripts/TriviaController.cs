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
        
        StartCoroutine(GetData());
    }

    public IEnumerator GetData()
    {
        string JSONurl = "https://127.0.0.1:5002/MJ1/preguntas";

        
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
                Debug.Log("JSON Recibido: " + jsonResult);

                listaRespuestas = JsonConvert.DeserializeObject<List<PreguntaTrivia>>(jsonResult);
                
                if (listaRespuestas != null && listaRespuestas.Count > 0)
                {
                    DialogueText.text = listaRespuestas[0].Pregunta;

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
            if (GestorPreguntas.instancia != null)
                GestorPreguntas.instancia.RespuestaCorrecta();
        }
        else
        {
            if (GestorPreguntas.instancia != null)
                GestorPreguntas.instancia.RespuestaIncorrecta();
        }
    }
}
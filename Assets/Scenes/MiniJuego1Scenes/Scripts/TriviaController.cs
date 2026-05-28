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

    }

    public IEnumerator GetData()
    {
               
        string JSONurl = "https://10.22.207.200:5001/MJ1/preguntas";

        UnityWebRequest web = UnityWebRequest.Get(JSONurl);
        web.certificateHandler = new ForceAcceptAll(); 
        yield return web.SendWebRequest();

        if (web.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error API Trivia MJ1: " + web.error);
        }
        else
        {
            
            listaRespuestas = JsonConvert.DeserializeObject<List<PreguntaTrivia>>(web.downloadHandler.text);
            
            if (listaRespuestas != null && listaRespuestas.Count > 0)
            {
            
                DialogueText.text = listaRespuestas[0].Pregunta;

                for (int i = 0; i < listaRespuestas.Count; i++)
                {
                    if (i < BotonesText.Length)
                    {
                        BotonesText[i].text = listaRespuestas[i].Respuesta;
                        
                        if (listaRespuestas[i].EsCorrecta == 1)
                        {
                            indiceCorrecto = i;
                        }
                        
                        //Debug.Log($"Opción {i}: {listaRespuestas[i].Respuesta} | Correcta: {listaRespuestas[i].EsCorrecta}");
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

    public class ForceAcceptAll : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;

        }
    }
}

using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class PreguntaApi : MonoBehaviour
{
    public TMP_Text textoPregunta;

    private string respuestaCorrecta;
    // Aquí se guarda internamente la respuesta correcta que viene desde la base de datos

    void Start()
    {
        StartCoroutine(CargarPregunta());
    }

    IEnumerator CargarPregunta()
    {
        string url = "http://127.0.0.1:5000/mision/pregunta";

        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest(); // espera la respuesta 

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;

            DatosPregunta pregunta = JsonUtility.FromJson<DatosPregunta>(json);

            textoPregunta.text = pregunta.TextoPregunta;

            respuestaCorrecta = pregunta.RespuestaCorrecta;
        }
        else
        {
            textoPregunta.text = "No se pudo cargar la pregunta.";
        }
    }


// este metodo lo usa ControlPregunta.cs
// Cuando el jugador presiona el botón Correcto o Incorrecto, ControlPregunta manda esa respuesta a este método
    public bool RespuestaEsCorrecta(string respuestaJugador)
    {
        return respuestaJugador == respuestaCorrecta;
    }
}

[System.Serializable]
public class DatosPregunta
{
    public int IdPreguntaMision;
    public string TextoPregunta;
    public string RespuestaCorrecta;
}
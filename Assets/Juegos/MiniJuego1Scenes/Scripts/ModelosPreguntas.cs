using System.Collections.Generic;
[System.Serializable]
public class FilaPreguntaAPI
{
    public int IdPregunta;
    public string Pregunta;
    public int IdRespuesta;
    public string Respuesta;
    public int EsCorrecta;
}

[System.Serializable]
public class PreguntaJuego
{
    public int idPregunta;
    public string textoPregunta;
    public List<RespuestaJuego> respuestas;
}

[System.Serializable]
public class RespuestaJuego
{
    public int idRespuesta;
    public string texto;
    public bool esCorrecta;
}
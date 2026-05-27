// ModelosPregunta.cs
// Clases que representan el JSON que devuelve tu API.
// Una fila = una respuesta de una pregunta.

using System.Collections.Generic;

// ── Una fila del JSON que devuelve el API ─────────────
// El API devuelve algo así por cada fila:
// { "IdPregunta":41, "Pregunta":"...", "IdRespuesta":161,
//   "Respuesta":"...", "EsCorrecta":1 }
[System.Serializable]
public class FilaPreguntaAPI
{
    public int    IdPregunta;
    public string Pregunta;
    public int    IdRespuesta;
    public string Respuesta;
    public int    EsCorrecta;   // 1 = correcta, 0 = incorrecta
}

// ── Pregunta ya procesada con sus respuestas agrupadas ─
// Esta es la que usarás dentro del juego
[System.Serializable]
public class PreguntaJuego
{
    public int               idPregunta;
    public string            textoPregunta;
    public List<RespuestaJuego> respuestas;
}

// ── Una respuesta dentro de PreguntaJuego ─────────────
[System.Serializable]
public class RespuestaJuego
{
    public int    idRespuesta;
    public string texto;
    public bool   esCorrecta;
}
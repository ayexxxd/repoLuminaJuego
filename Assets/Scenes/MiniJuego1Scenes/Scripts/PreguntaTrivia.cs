// PreguntaTrivia.cs
// Molde que representa cada fila del JSON que devuelve tu API.
// Los nombres deben coincidir EXACTAMENTE con las claves del JSON.

[System.Serializable]
public class PreguntaTrivia
{
    public int    IdPregunta;
    public string Pregunta;
    public int    IdRespuesta;
    public string Respuesta;
    public int    EsCorrecta;  // 1 = correcta, 0 = incorrecta
}
[System.Serializable]
public class PreguntaTrivia
{
    // Estos nombres deben coincidir EXACTAMENTE con las columnas de tu JSON de Flask
    public int IdPregunta;
    public string Pregunta;
    public int IdRespuesta;
    public string Respuesta;
    public int EsCorrecta; // 1 si es la buena, 0 si es incorrecta
}
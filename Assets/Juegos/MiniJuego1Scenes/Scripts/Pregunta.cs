using System;

[Serializable]
public class Pregunta
{
    public string textoPregunta;
    public string[] respuestas  = new string[4];
    public int indiceCorrecta;
}
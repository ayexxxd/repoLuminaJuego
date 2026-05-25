// Pregunta.cs
// Estructura de datos de una pregunta. Se edita directo en el Inspector.

using System;

[Serializable]
public class Pregunta
{
    public string   textoPregunta;
    public string[] respuestas     = new string[4];
    public int      indiceCorrecta; // 0, 1, 2 o 3
}
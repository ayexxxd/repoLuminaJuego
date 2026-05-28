// memoria general del juego

//guarda información que se debe de mantener aunque se cambie de escenas
public static class DatosJuego
{
    public static int nivelActual = 1; //para saber nivel de jugador
                                    //Para ENP. Como todas las preguntas usan la misma escena, el juego necesita saber si el jugador venía de EN1, EN2 o EN3.


// sirven par que los paneles no aparezcan muchas veces 
    public static bool instruccionesEN1Vista = false;
    public static bool advertenciaEN2Vista = false;
    public static bool advertenciaEN3Vista = false;
// //


//tokens que acumula al final de la partida (3 niveles completados)
    public static int tokensPartida = 0;

// tokens que acumula por nivel
    public static int tokensNivelPendiente = 0;




// reinicia los paneles para que vuelvan a aparecer en una nueva partida
    public static void ReiniciarAdvertencias()
    {
        instruccionesEN1Vista = false;
        advertenciaEN2Vista = false;
        advertenciaEN3Vista = false;
    }

/// ////////



// te reinicia todo lo acumulado en partida pasada
// reinicia todo para partida nueva
    public static void ReiniciarPartida()
    {
        nivelActual = 1;
        tokensPartida = 0;
        tokensNivelPendiente = 0;

        ReiniciarAdvertencias();
    }
}

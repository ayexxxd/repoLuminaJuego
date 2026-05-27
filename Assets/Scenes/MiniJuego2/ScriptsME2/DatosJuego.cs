public static class DatosJuego
{
    public static int nivelActual = 1;

    public static bool instruccionesEN1Vista = false;
    public static bool advertenciaEN2Vista = false;
    public static bool advertenciaEN3Vista = false;

    public static int tokensPartida = 0;
    public static int tokensNivelPendiente = 0;

    public static void ReiniciarAdvertencias()
    {
        instruccionesEN1Vista = false;
        advertenciaEN2Vista = false;
        advertenciaEN3Vista = false;
    }

    public static void ReiniciarPartida()
    {
        nivelActual = 1;
        tokensPartida = 0;
        tokensNivelPendiente = 0;

        ReiniciarAdvertencias();
    }
}
//Este script guarda información general del juego. 
//En este caso guarda el número del nivel que el jugador acaba de completar.
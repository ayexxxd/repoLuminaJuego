using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Controla la pantalla de victoria
// Lee los datos guardados por el GameManager via PlayerPrefs
// Este script va en un GameObject vacío llamado "VictoriaManager"
public class PantallaVictoria : MonoBehaviour
{
    [Header("Textos del panel de estadísticas")]
    // Arrastra cada texto desde el Hierarchy al Inspector
    public TextMeshProUGUI textoPuntos;
    public TextMeshProUGUI textoTokens;
    public TextMeshProUGUI textoTiempoActual;
    public TextMeshProUGUI textoMejorTiempo;

    [Header("Nombres de escenas")]
    public string nombreEscenaJuego = "Juego";
    public string nombreEscenaMenu = "MenuPrincipal";

    void Start()
    {
        // Restauramos la velocidad del tiempo por si quedó pausado
        Time.timeScale = 1f;

        // ---- Leemos los datos que guardó el GameManager ----
        int puntos      = PlayerPrefs.GetInt("PuntosFinales", 0);
        int tokens      = PlayerPrefs.GetInt("TokensGanados", 0);
        float tiempo    = PlayerPrefs.GetFloat("TiempoFinal", 0f);
        float mejorTiempo = PlayerPrefs.GetFloat("MejorTiempo", 0f);

        // ---- Mostramos los datos en los textos ----
        if (textoPuntos != null)
            textoPuntos.text = "Puntos: " + puntos;

        if (textoTokens != null)
            textoTokens.text = "Tokens: " + tokens;

        if (textoTiempoActual != null)
            textoTiempoActual.text = "Tiempo: " + FormatearTiempo(tiempo);

        // Si el mejor tiempo es 0 significa que no hay record guardado aún
        if (textoMejorTiempo != null)
        {
            if (mejorTiempo <= 0f)
                textoMejorTiempo.text = "Mejor: --:--";
            else
                textoMejorTiempo.text = "Mejor: " + FormatearTiempo(mejorTiempo);
        }

        // ---- Acumulamos los tokens al total del jugador ----
        // Esto es temporal — en la Etapa 14 lo reemplaza la API
        int tokensAcumulados = PlayerPrefs.GetInt("TokensAcumulados", 0);
        tokensAcumulados += tokens;
        PlayerPrefs.SetInt("TokensAcumulados", tokensAcumulados);
        PlayerPrefs.Save();

        Debug.Log("Victoria cargada. Puntos: " + puntos +
                " | Tokens: " + tokens +
                " | Tiempo: " + tiempo);
    }

    // ---- Convierte segundos a formato MM:SS ----
    // Ejemplo: 75.3 segundos → "01:15"
    string FormatearTiempo(float segundos)
    {
        int minutos = Mathf.FloorToInt(segundos / 60f);
        int segs    = Mathf.FloorToInt(segundos % 60f);
        return string.Format("{0:00}:{1:00}", minutos, segs);
    }

    // ---- Botones ----

    public void BotonVolverAJugar()
    {
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void BotonMenu()
    {
        SceneManager.LoadScene(nombreEscenaMenu);
    }

    public void BotonSalir()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
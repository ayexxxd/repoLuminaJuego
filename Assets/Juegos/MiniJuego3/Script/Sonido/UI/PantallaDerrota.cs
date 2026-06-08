using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Controla la pantalla de derrota
// Lee la razón de derrota y los datos guardados por el GameManager
// Este script va en un GameObject vacío llamado "DerrotaManager"
public class PantallaDerrota : MonoBehaviour
{
    [Header("Textos de la pantalla")]
    public TextMeshProUGUI textoRazon;
    public TextMeshProUGUI textoPuntos;
    public TextMeshProUGUI textoMejorTiempo;

    [Header("Nombres de escenas")]
    public string nombreEscenaJuego = "Juego";
    public string nombreEscenaMenu = "MenuPrincipal";

    void Start()
    {
        Time.timeScale = 1f;

        // ---- Leemos los datos guardados por el GameManager ----
        // RazonDerrota: 0 = sin vidas, 1 = tiempo agotado
        int razon           = PlayerPrefs.GetInt("RazonDerrota", 0);
        int puntos          = PlayerPrefs.GetInt("PuntosFinales", 0);
        float mejorTiempo   = PlayerPrefs.GetFloat("MejorTiempo", 0f);

        // ---- Mostramos la razón de derrota ----
        if (textoRazon != null)
        {
            if (razon == 0)
                textoRazon.text = "Te quedaste sin vidas ";
            else
                textoRazon.text = "Se agotó el tiempo ";
        }

        // ---- Mostramos los puntos ----
        if (textoPuntos != null)
            textoPuntos.text = "Puntos: " + puntos;

        // ---- Mostramos el mejor tiempo ----
        if (textoMejorTiempo != null)
        {
            if (mejorTiempo <= 0f)
                textoMejorTiempo.text = "Mejor: --:--";
            else
                textoMejorTiempo.text = "Mejor: " + FormatearTiempo(mejorTiempo);
        }

        Debug.Log("Derrota cargada. Razón: " + razon + " | Puntos: " + puntos);
    }

    // ---- Convierte segundos a formato MM:SS ----
    string FormatearTiempo(float segundos)
    {
        int minutos = Mathf.FloorToInt(segundos / 60f);
        int segs    = Mathf.FloorToInt(segundos % 60f);
        return string.Format("{0:00}:{1:00}", minutos, segs);
    }

    // ---- Botones ----

    public void BotonReintentar()
    {
        SceneManager.LoadScene("SceneCarro");
    }

    public void BotonMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void BotonSalir()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
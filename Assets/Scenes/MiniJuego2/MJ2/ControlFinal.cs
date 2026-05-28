using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ControlFinal : MonoBehaviour
{
    public int tokensObtenidos;

    public TMP_Text textoTokensObtenidos;




    void Start()
    {
        tokensObtenidos = DatosJuego.tokensPartida;

        MostrarTokens();
    }



    void MostrarTokens()
    {
        textoTokensObtenidos.text = tokensObtenidos.ToString();
    } // conviert el numero de tokens a texto y lo coloca en la interfaz



    void GuardarTokensTotales()
    {
        int tokensActuales = PlayerPrefs.GetInt("TokensTotales", 0);

        tokensActuales += tokensObtenidos;

        PlayerPrefs.SetInt("TokensTotales", tokensActuales);

        PlayerPrefs.Save();
    }
    // Este método era para guardar tokens localmente con PlayerPrefs
    //pero después conectamos el juego al API
    //entonces la actualización real de tokens se hace en TokensApi.cs contra la base de datos




//botones
    public void JugarDeNuevo()
    {
        SceneManager.LoadScene("EMI");
    }

    public void IrMenuPrincipal()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
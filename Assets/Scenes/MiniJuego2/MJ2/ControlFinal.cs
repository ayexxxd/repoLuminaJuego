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
    }

    void GuardarTokensTotales()
    {
        int tokensActuales = PlayerPrefs.GetInt("TokensTotales", 0);

        tokensActuales += tokensObtenidos;

        PlayerPrefs.SetInt("TokensTotales", tokensActuales);

        PlayerPrefs.Save();
    }

    public void JugarDeNuevo()
    {
        SceneManager.LoadScene("EMI");
    }

    public void IrMenuPrincipal()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
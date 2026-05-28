using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class ControlInicioMision : MonoBehaviour
{
    public TMP_Text textoTokensFinales;

    public int idUsuario = 1;

    void Start()
    {
        StartCoroutine(CargarTokensDesdeApi());
    }

    IEnumerator CargarTokensDesdeApi()
    {
        string url = "http://127.0.0.1:5000/mision/tokens/" + idUsuario;

        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;

            UsuarioTokens usuario = JsonUtility.FromJson<UsuarioTokens>(json);

            textoTokensFinales.text = usuario.WhirlTokens.ToString();
        }
        else
        {
            textoTokensFinales.text = "0";

            Debug.Log("Error al cargar tokens: " + request.error);
        }
    }

    public void IniciarJuego()
    {
        DatosJuego.ReiniciarPartida();

        SceneManager.LoadScene("EN1");
    }

    public void RegresarMenuPrincipal()
    {
        SceneManager.LoadScene("MenuScene");
    }
}

[System.Serializable]
public class UsuarioTokens
{
    public int IdUsuario;
    public string Nombre;
    public int WhirlTokens;
}
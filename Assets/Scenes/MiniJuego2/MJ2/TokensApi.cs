using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class TokensApi : MonoBehaviour
{
    public int idUsuario = 1;
    public int tokensObtenidos;

    void Start()
    {
        tokensObtenidos = DatosJuego.tokensPartida;

        StartCoroutine(ActualizarTokens());
        StartCoroutine(GuardarPartida());
    }

    IEnumerator ActualizarTokens()
    {
        string url = "http://127.0.0.1:5003/mision/tokens/" + idUsuario;

        string json = "{\"tokensObtenidos\":" + tokensObtenidos + "}";

        UnityWebRequest request = new UnityWebRequest(url, "PUT");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Tokens actualizados correctamente: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error al actualizar tokens: " + request.error);
        }
    }

    IEnumerator GuardarPartida()
    {
        string url = "http://127.0.0.1:5000/mision/partida";

        string json = "{\"idUsuario\":" + idUsuario + ",\"tokensObtenidos\":" + tokensObtenidos + "}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Partida guardada correctamente: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error al guardar partida: " + request.error);
        }
    }
}
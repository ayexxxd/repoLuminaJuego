using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI nombreJuego;

    public TextMeshProUGUI tokensTexto;
    public Image gameImage;

    IEnumerator Start()
    {
        yield return ObtenerJuego();

        yield return ActualizarPuntos(1, 0);
    }

    IEnumerator ObtenerJuego()
{
    string url = "https://127.0.0.1:5010/img/5";

    UnityWebRequest web = UnityWebRequest.Get(url);

    web.certificateHandler = new ForceAcceptAll5();

    yield return web.SendWebRequest();

    if (web.result != UnityWebRequest.Result.Success)
    {
        Debug.Log(web.error);
    }
    else
    {
        Juego juego =
            JsonUtility.FromJson<Juego>
            (web.downloadHandler.text);

        nombreJuego.text = juego.Nombre;

        UnityWebRequest imageWeb =
            UnityWebRequestTexture
            .GetTexture(juego.imagen);

        imageWeb.certificateHandler =
            new ForceAcceptAll5();

        yield return imageWeb.SendWebRequest();

        if (imageWeb.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(imageWeb.error);
        }
        else
        {
            Texture2D texture =
                DownloadHandlerTexture
                .GetContent(imageWeb);

            Sprite sprite =
                Sprite.Create(
                    texture,
                    new Rect(
                        0,
                        0,
                        texture.width,
                        texture.height),
                    new Vector2(0.5f, 0.5f)
                );

            gameImage.sprite = sprite;
        }
    }
}

    IEnumerator ActualizarPuntos(int idUser, int points)
    {
        string url = "https://127.0.0.1:5010/updatepoints";

        int score = PlayerPrefs.GetInt("Score");

        int tokensGanados = score / 170;

        tokensTexto.text ="WhirlTokens: " + tokensGanados;

        UpdatePointsRequest data =new UpdatePointsRequest();

        data.idUser = idUser;

        data.points = tokensGanados;

        string json =JsonUtility.ToJson(data);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest web =
        new UnityWebRequest(url, "PUT");

        web.uploadHandler =new UploadHandlerRaw(bodyRaw);

        web.downloadHandler =new DownloadHandlerBuffer();

        web.SetRequestHeader("Content-Type", "application/json");

        web.certificateHandler =new ForceAcceptAll5();

        yield return web.SendWebRequest();

        if (web.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(web.error);
        }
        else
        {
            Debug.Log(web.downloadHandler.text);
        }
    }
}
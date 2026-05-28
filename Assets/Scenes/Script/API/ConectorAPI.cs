using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ConectorAPI : MonoBehaviour
{
    [Header("URL de tu API")]
    public string urlBase = "https://127.0.0.1:5001";

    public static ConectorAPI instancia;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GuardarTiempo(int tiempo, System.Action<bool> callback = null)
    {
        StartCoroutine(CorrutinaGuardarTiempo(tiempo, callback));
    }

    IEnumerator CorrutinaGuardarTiempo(int tiempo, System.Action<bool> callback)
    {
        EnvioTiempo datos = new EnvioTiempo { Tiempo = tiempo };
        string json = JsonUtility.ToJson(datos);
        string url  = urlBase + "/MJ3/guardarTiempo";

        Debug.Log("ConectorAPI: Enviando → " + json);
        Debug.Log("ConectorAPI: URL → " + url);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);

            UploadHandlerRaw uploadHandler = new UploadHandlerRaw(bytes);
            uploadHandler.contentType = "application/json";

            request.uploadHandler   = uploadHandler;
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            request.timeout = 10;

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Tiempo guardado correctamente.");
                Debug.Log("Respuesta: " + request.downloadHandler.text);
                callback?.Invoke(true);
            }
            else
            {
                Debug.LogError("❌ Error: " + request.error);
                Debug.LogError("Código HTTP: " + request.responseCode);
                callback?.Invoke(false);
            }
        }
    }

    [ContextMenu("Probar Guardar Tiempo")]
    public void ProbarGuardarTiempo()
    {
        Debug.Log("=== PRUEBA: Enviando tiempo = 45 ===");

        GuardarTiempo(45, (exito) =>
        {
            if (exito)
                Debug.Log("PRUEBA EXITOSA");
            else
                Debug.LogError("PRUEBA FALLIDA — revisa la URL en el Inspector");
        });
    }
}
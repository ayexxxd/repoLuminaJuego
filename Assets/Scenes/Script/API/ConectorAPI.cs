using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// Conecta Unity con tu endpoint /MJ3/guardarTiempo
// Va en un GameObject vacío llamado "ConectorAPI" en la escena MenuPrincipal
public class ConectorAPI : MonoBehaviour
{
    [Header("URL de tu API")]
    // Pon tu URL real aquí — sin slash al final
    // Ejemplo: "https://mi-api.onrender.com"
    public string urlBase = "https://10.22.189.195:5001";

    // Singleton — accesible desde cualquier script
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

    // ---- Envía el tiempo a tu API ----
    // Llama esto cuando el jugador gana
    public void GuardarTiempo(int tiempo, System.Action<bool> callback = null)
    {
        StartCoroutine(CorrutinaGuardarTiempo(tiempo, callback));
    }

    IEnumerator CorrutinaGuardarTiempo(int tiempo, System.Action<bool> callback)
    {
        // Construimos el JSON: { "Tiempo": 38 }
        EnvioTiempo datos = new EnvioTiempo { Tiempo = tiempo };
        string json = JsonUtility.ToJson(datos);
        string url  = urlBase + "/MJ3/guardarTiempo";

        Debug.Log("ConectorAPI: Enviando → " + json);
        Debug.Log("ConectorAPI: URL → " + url);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);

            UploadHandlerRaw uploadHandler = new UploadHandlerRaw(bytes);
            // Forzamos el Content-Type directamente en el UploadHandler
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

    // ---- Prueba desde el Inspector ----
    // Clic derecho en el componente → "Probar Guardar Tiempo"
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
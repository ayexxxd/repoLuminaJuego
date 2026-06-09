using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

namespace Login
{   
public class LoginController : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button submitButton;
    public TextMeshProUGUI feedbackText;

    public void OnSubmit()
    {
        StartCoroutine(Login());
    }

    IEnumerator Login()
    {
        string JSONurl = "https://10.14.255.45:5010/login";

        string body = "{\"email\":\"" + emailInput.text + "\",\"password\":\"" + passwordInput.text + "\"}";

        UnityWebRequest web = new UnityWebRequest(JSONurl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(body);
        web.uploadHandler = new UploadHandlerRaw(bodyRaw);
        web.downloadHandler = new DownloadHandlerBuffer();
        web.SetRequestHeader("Content-Type", "application/json");
        web.certificateHandler = new ForceAcceptAll();

        yield return web.SendWebRequest();

        if (web.result != UnityWebRequest.Result.Success)
        {
            feedbackText.text = "Credenciales incorrectas. Inténtalo de nuevo.";
        }
        else
        {
            int userId = int.Parse(web.downloadHandler.text);
            if (userId > 0)
            {
                PlayerPrefs.SetInt("userid", userId);
                // Load your next scene here
                UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
            }
        }
    }
}}
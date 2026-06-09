using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

namespace TopDown.Enemy
{
    public class EndScript : MonoBehaviour
    {
        [SerializeField] private AudioClip endSFX;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI gameOverText;
        [SerializeField] private TextMeshProUGUI waveText;

        public void Retry()
        {
            SceneManager.LoadScene("ShooterScene");
        }
        public void Quit()
        {
            SceneManager.LoadScene("StartScene");
        }

        IEnumerator UpdatePoints(int idUser, int points)
        {
            string url = "https://10.14.255.45:5010/updatepoints";
            string jsonBody = "{\"idUser\":" + idUser + ",\"points\":" + points + "}";
            
            UnityWebRequest web = new UnityWebRequest(url, "PUT");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            web.uploadHandler = new UploadHandlerRaw(bodyRaw);
            web.downloadHandler = new DownloadHandlerBuffer();
            web.SetRequestHeader("Content-Type", "application/json");
            web.certificateHandler = new ForceAcceptAll();
            yield return web.SendWebRequest();

            if (web.result != UnityWebRequest.Result.Success)
                Debug.Log("Error API: " + web.error);
            else
                Debug.Log("Puntos actualizados");
        }

        void Start()
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null && endSFX != null)
                audioSource.PlayOneShot(endSFX);

            if (ScoreManager.instance != null)
                scoreText.text = "Puntaje Final: " + ScoreManager.instance.GetScore();
            else
                scoreText.text = "Puntaje Final: 0";

            int finalWave = Mathf.Min(PlayerPrefs.GetInt("CurrentWave"), 10);
            if (waveText != null)
                waveText.text = "Oleada Final: " + finalWave;

            if (PlayerPrefs.GetInt("CurrentWave") > 10 && gameOverText != null)
                gameOverText.text = "¡Has Ganado!";

            int userId = PlayerPrefs.GetInt("user_id");

            int sco = ScoreManager.instance != null ? ScoreManager.instance.GetScore() : 0;
            int score = sco / 100;

            if (userId > 0)
                StartCoroutine(UpdatePoints(userId, score));
                StartCoroutine(SaveTransaccion(userId, score, null,  "Puntaje Final en Cazador de Errores"));
        }
    IEnumerator SaveTransaccion(int idUser, int monto, int? idReward, string description)
{
    string jsonBody = "{\"idUser\":" + idUser + ",\"idReward\":null,\"monto\":" + monto + ",\"description\":\"" + description + "\"}";
    UnityWebRequest web = new UnityWebRequest("https://10.14.255.45:5010/transaccion", "POST");
    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
    web.uploadHandler = new UploadHandlerRaw(bodyRaw);
    web.downloadHandler = new DownloadHandlerBuffer();
    web.SetRequestHeader("Content-Type", "application/json");
    web.certificateHandler = new ForceAcceptAll();
    yield return web.SendWebRequest();

    if (web.result != UnityWebRequest.Result.Success)
        Debug.Log("Error API: " + web.error);
    else
        Debug.Log("Transacción guardada");
}}}

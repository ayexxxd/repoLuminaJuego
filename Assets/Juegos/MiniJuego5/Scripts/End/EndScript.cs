using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
    }
}
}
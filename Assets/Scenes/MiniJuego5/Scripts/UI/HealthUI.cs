using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    private Health playerHealth;
    private Spawner spawner;
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI waveText;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");   
        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
        }else
        {
            Debug.LogError("no jay jugador");
        }

        spawner = FindAnyObjectByType<Spawner>();
    }
    private void UpdateBar(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }
    private void UpdateWave(int currentWave, int totalWaves)
    {
        if (waveText != null)
        {
            waveText.text = "OLEADA: " + currentWave + "/" + totalWaves;
        }
    }
    private void Update()
{
    if (playerHealth != null)
        UpdateBar(playerHealth.getHealth(), playerHealth.getMax_Health());

    if (spawner != null)
        UpdateWave(spawner.CurrentWave, spawner.TotalWaves);
}
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TopDown.UI
{
using TopDown.Enemy;
using TopDown.Shooting;

public class HealthUI : MonoBehaviour
{
    private Health playerHealth;
    private Spawner spawner;
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI statsText;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<Health>();
        spawner = FindAnyObjectByType<Spawner>();
    }
    private void UpdateBar(float current)
    {
        if (fillImage != null && current > 0)
        {
            fillImage.fillAmount = current / 100;
        }
    }
    private void UpdateWave(int currentWave, int totalWaves)
    {
        if (waveText != null)
        {
            waveText.text = "OLEADA: " + Mathf.Min(currentWave, totalWaves) + "/" + totalWaves;
        }
    }
    private void UpdateStatsText()
    {
        GunController gun = FindAnyObjectByType<GunController>();
        if (gun != null && statsText != null)
        {
            gun.GetBulletStats(out int dmg, out float spd, out float cd);
            string newText = "DMG: " + dmg + " SPD: " + spd.ToString("F1") + " CD: " + cd.ToString("F2");
            if (statsText.text != newText)
            {
                statsText.text = newText;
            }
        }
    }
    private void Update()
    {
        if (playerHealth != null)
            UpdateBar(playerHealth.getHealth());
        if (spawner != null)
            UpdateWave(spawner.CurrentWave, spawner.TotalWaves);
        UpdateStatsText();
    }
}
} 
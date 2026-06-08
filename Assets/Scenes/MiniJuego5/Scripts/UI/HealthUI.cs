using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TopDown.UI
{
using TopDown.Enemy;
using TopDown.Shooting;
    using Unity.VisualScripting;

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
        if(current > 0){
            fillImage.fillAmount = current / 100;}
        
    }
    private void UpdateWave(int currentWave, int totalWaves)
    {
        waveText.text = "OLEADA: " + currentWave + "/" + totalWaves;
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
                Debug.Log("HealthUI: Stats text updated -> " + newText);
            }
        }
        else
        {
            if (gun == null) Debug.LogWarning("HealthUI: GunController not found!");
            if (statsText == null) Debug.LogWarning("HealthUI: statsText is null!");
        }
    }
    private void Update()
    {
        UpdateBar(playerHealth.getHealth());
        UpdateWave(spawner.CurrentWave, spawner.TotalWaves);
        UpdateStatsText();
    }
}
} 
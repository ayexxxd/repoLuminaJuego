using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TopDown.UI
{
    using TopDown.Enemy;//namespace to organize code and avoid naming conflicts

    public class HealthBar : MonoBehaviour
    {
        private Health playerHealth;
        private Spawner spawner;

        [SerializeField] private Image fillImage;
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private TextMeshProUGUI statsText;

        private void Start()
        {
            TryAcquirePlayerHealth();
            TryAcquireSpawner();
        }

        private void Update()
        {
            if (playerHealth == null)
            {
                TryAcquirePlayerHealth();
            }

            if (spawner == null)
            {
                TryAcquireSpawner();
            }

            if (playerHealth != null)
            {
                UpdateBar(playerHealth.getHealth());
            }

            if (spawner != null)
            {
                UpdateWave(spawner.CurrentWave, spawner.TotalWaves);
            }
        }

        private void TryAcquirePlayerHealth()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<Health>();
            }
        }

        private void TryAcquireSpawner()
        {
            spawner = FindAnyObjectByType<Spawner>();
        }

        private void UpdateBar(float current)
        {
            if (fillImage == null)
            {
                return;
            }

            fillImage.fillAmount = Mathf.Clamp01(current / 100f);
        }

        private void UpdateWave(int currentWave, int totalWaves)
        {
            if (waveText == null)
            {
                return;
            }

            waveText.text = "OLEADA: " + currentWave + "/" + totalWaves;
        }
    }
}
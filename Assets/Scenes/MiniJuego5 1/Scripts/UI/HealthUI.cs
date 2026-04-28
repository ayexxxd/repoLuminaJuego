using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image fillImage;

    void Update()
    {
        float current = playerHealth.getHealth();
        float max = playerHealth.getMax_Health();

        fillImage.fillAmount = current / max;
    }
}
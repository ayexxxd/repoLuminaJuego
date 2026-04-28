using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Health playerHealth;
    [SerializeField] private Image fillImage;

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
    }
    private void UpdateBar(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }
    private void Update()
{
    if (playerHealth != null)
        UpdateBar(playerHealth.getHealth(), playerHealth.getMax_Health());
}
}
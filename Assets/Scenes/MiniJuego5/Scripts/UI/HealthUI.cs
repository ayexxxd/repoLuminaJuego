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

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");//buscar objeto con tag de jugador  
        playerHealth = player.GetComponent<Health>();//obtener componente de salud del jugador
        spawner = FindAnyObjectByType<Spawner>();//buscar objeto con componente Spawner
    }
    private void UpdateBar(float current)
    {//funcion para actualizar barra de vida
        fillImage.fillAmount = current / 100;
    }
    private void UpdateWave(int currentWave, int totalWaves)
    {
        waveText.text = "OLEADA: " + currentWave + "/" + totalWaves;
    }
    private void Update()
    {
        if (waveText!=null && spawner != null)
        {
            UpdateBar(playerHealth.getHealth());
            UpdateWave(spawner.CurrentWave, spawner.TotalWaves);
        }
    }
    /*public void UpdateStats(int damage, float speed, float cooldown)
    {
        statsText.text = "DMG: " + damage + " SPD: " + speed + " CD: "+ cooldown;
}*/
}}  
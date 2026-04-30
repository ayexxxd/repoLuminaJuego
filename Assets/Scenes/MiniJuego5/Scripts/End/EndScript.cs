using UnityEngine;
using TMPro;
using UnityEditor.Search;
namespace TopDown.Enemy{//namespace to organize code and avoid naming conflicts

public class EndScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;//reference to score text UI element
    [SerializeField] private Spawner spawner;//reference to spawner to get final wave info
    [SerializeField] private TextMeshProUGUI gameOverText;//reference to game over text UI element
    [SerializeField] private ScoreManager scoreManager;//reference to score manager to get final score
    [SerializeField] private TextMeshProUGUI waveText;//reference to wave text UI element
    private int finalScore, finalWave;//variables to store final score and wave info for display

    void Start()
    {   
    //get final score and wave info from score manager and spawner
        finalScore = ScoreManager.instance.GetScore(); //get final score from ScoreManager singleton instance
        finalWave = PlayerPrefs.GetInt("CurrentWave", 0); //get final wave from PlayerPrefs, default to 0 if not found

    void Update()
    {
        scoreText.text = "Puntaje Final: " + finalScore;
        if(finalWave > 10)gameOverText.text = "¡Has Ganado!";
        waveText.text = "Oleada Final: " + finalWave + "/" + 10;
    }
}}
}
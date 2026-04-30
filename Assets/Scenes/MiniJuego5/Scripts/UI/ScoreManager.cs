using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int pointsForEnemyS = 10;
    [SerializeField] private int pointsForEnemyM = 25;
    [SerializeField] private int pointsForEnemyL = 50;
    private void Start()
    {
        UpdateScoreDisplay();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreDisplay();
    }

    public void EnemyKilled(string Tag)
    {
        AddScore(GetPointsForEnemyKillt(Tag));
    }

    private int GetPointsForEnemyKillt(string Tag)
    {
        switch (Tag)
        {
            case "EnemyS":
                return pointsForEnemyS;
            case "EnemyM":
                return pointsForEnemyM;
            case "EnemyL":
                return pointsForEnemyL;
            default:
                return 0;
        }
    }

    private void UpdateScoreDisplay()
    {//actualiaar el texto del score en la UI
        scoreText.text = "Puntos: " + score;
    }

    public int GetScore()//lo usamos en endscreen
    {
        return score;
    }
}

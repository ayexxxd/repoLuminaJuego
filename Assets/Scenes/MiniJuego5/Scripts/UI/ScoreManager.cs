using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int pointsForEnemyS = 10;
    [SerializeField] private int pointsForEnemyM = 20;
    [SerializeField] private int pointsForEnemyL = 50;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
    {
        if (scoreText != null)
        {
            scoreText.text = "Puntos: " + score;
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void SaveScore()
{
    PlayerPrefs.SetInt("FinalScore", score);
    PlayerPrefs.Save(); // makes sure it writes to disk
}
}

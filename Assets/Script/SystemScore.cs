using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SystemScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText; 
    public TextMeshProUGUI killText; 
    private int score = 0;
    private int kill = 0;

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddKill()
    {
        kill++;
        UpdateScoreUI();
    }
    public void AddScore()
    {
        score+=10;
        UpdateScoreUI();
    }
    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (killText != null)
        {
            killText.text = "Kills: " + kill;
        }
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}

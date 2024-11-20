using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int player1Score = 0;
    public Text player1ScoreText;

    public int player2Score = 0;
    public Text player2ScoreText;

    public bool canOpenChest = false;

    public void AddPlayer1Score(int points)
    {
        player1Score += points;
        UpdatePlayer1ScoreUI();
        CheckChestUnlock();
    }

    public void AddPlayer2Score(int points)
    {
        player2Score += points;
        UpdatePlayer2ScoreUI();
        CheckChestUnlock();
    }

    void UpdatePlayer1ScoreUI()
    {
        if (player1ScoreText != null)
        {
            player1ScoreText.text = "Pumpkin Score: " + player1Score.ToString();
        }
    }

    void UpdatePlayer2ScoreUI()
    {
        if (player2ScoreText != null)
        {
            player2ScoreText.text = "Apple Score: " + player2Score.ToString();
        }
    }
    private void CheckChestUnlock()
    {
        if (player1Score == 8 && player2Score == 11)
        {
            canOpenChest = true;
            Debug.Log("The chest can now be opened!");
        }
    }

    public void ResetScores()
    {
        player1Score = 0;
        player2Score = 0;
        UpdatePlayer1ScoreUI();
        UpdatePlayer2ScoreUI();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    [SerializeField] private Text scoreText;
    [SerializeField] private float bonusMultiplier = 2f;

    private int totalChestHits = 0;
    private int totalGoldHits = 0;
    private bool isBonusActive = false;

    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreText();
    }

    public void AddScore(int scoreToAdd)
    {
        float finalScore = isBonusActive ? scoreToAdd * bonusMultiplier : scoreToAdd;
        score += Mathf.RoundToInt(finalScore);
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void RegisterHit(string objectTag)
    {
        switch (objectTag)
        {
            case "Chest":
                totalChestHits++;
                AddScore(50);
                Debug.Log($"Chest destroyed, total chest hits: {totalChestHits}");
                break;
            case "Gold":
                totalGoldHits++;
                AddScore(100);
                Debug.Log($"Golden Chest destroyed, total gold hits: {totalGoldHits}");
                break;
            case "Goal":
                AddScore(10);
                Debug.Log($"Goal!!!!!!, adding {(isBonusActive ? 10 * bonusMultiplier : 10)} points");
                break;
            case "Target":
                AddScore(30);
                Debug.Log($"Target hit!!!!!!, adding {(isBonusActive ? 30 * bonusMultiplier : 30)} points");
                break;
        }

        CheckBonusCondition();
    }

    private void CheckBonusCondition()
    {
        if (!isBonusActive && totalChestHits >= 2 && totalGoldHits >= 1)
        {
            ActivateBonus();
        }
    }

    private void ActivateBonus()
    {
        isBonusActive = true;
        Debug.Log($"Bonus Activated! All subsequent scores will be multiplied by {bonusMultiplier}x!");
    }
}
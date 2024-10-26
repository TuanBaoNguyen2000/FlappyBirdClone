using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("UI References")]
    [SerializeField] private Text scoreText, gameoverScoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Toggle birdAIToggle;

    private void Start()
    {
        // Add listener
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(() => {
                GameManager.Instance.ResetGame();
            });
        }

        if (birdAIToggle != null)
        {
            birdAIToggle.onValueChanged.AddListener(GameManager.Instance.SetBirdAI);
        }

        // Initialize UI
        UpdateScore(0);
        ShowGameOver(false);
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }

        if (gameoverScoreText != null)
        {
            gameoverScoreText.text = "Score: " + score.ToString();
        }
    }

    public void ShowGameOver(bool show)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(show);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Bird bird;
    [SerializeField] private ObstacleSpawner spawner;

    private UIManager uiManager => UIManager.Instance;

    private int score;
    private bool isGameOver;

    private void Start()
    {
        ResetGame();
    }

    public void ResetGame()
    {
        // Reset score
        score = 0;
        isGameOver = false;

        // Reset UI
        uiManager.UpdateScore(score);
        uiManager.ShowGameOver(false);

        // Reset bird
        bird.Reset();

        // Clear obstacles
        ObstacleManager.Instance.ClearObstacles();
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        uiManager.ShowGameOver(true);
        SoundManager.Instance.PlaySound("GameOver");
    }

    public void AddScore()
    {
        score++;
        uiManager.UpdateScore(score);
        SoundManager.Instance.PlaySound("Score");
    }

    public void SetBirdAI(bool enable)
    {
        bird.SetAIEnabled(enable);
    }
}
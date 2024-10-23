using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Bird bird;
    [SerializeField] private ObstacleSpawner spawner;
    //[SerializeField] private UIManager uiManager;

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
        //uiManager.UpdateScore(score);
        //uiManager.ShowGameOver(false);

        // Reset bird
        bird.Reset();

        // Clear obstacles
        foreach (var obstacle in FindObjectsOfType<Obstacle>())
        {
            Destroy(obstacle.gameObject);
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        //uiManager.ShowGameOver(true);
    }

    public void AddScore()
    {
        score++;
        //uiManager.UpdateScore(score);
    }
}
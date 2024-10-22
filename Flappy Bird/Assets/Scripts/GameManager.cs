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
        score = 0;
        isGameOver = false;

        // Clear existing obstacles
        foreach (var obstacle in FindObjectsOfType<Obstacle>())
        {
            Destroy(obstacle.gameObject);
        }

        // Reset bird position
        bird.transform.position = new Vector3(-4, 0, 0);
        bird.enabled = true;

        //uiManager.UpdateScore(score);
        //uiManager.ShowGameOver(false);
    }

    public void ToggleAI()
    {
        bird.SetAIEnabled(!bird.enabled);
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        bird.Die();
        //uiManager.ShowGameOver(true);
    }

    public void AddScore()
    {
        score++;
        //uiManager.UpdateScore(score);
    }
}
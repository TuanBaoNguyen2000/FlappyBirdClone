using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : Singleton<ObstacleManager>
{
    [SerializeField] private ObstacleSpawner obstacleSpawner;
    private List<Obstacle> activeObstacles = new List<Obstacle>();

    public ObstacleSpawner ObstacleSpawner => obstacleSpawner;

    public void RegisterObstacle(Obstacle obstacle)
    {
        activeObstacles.Add(obstacle);
    }

    public void UnregisterObstacle(Obstacle obstacle)
    {
        activeObstacles.Remove(obstacle);
    }

    public List<Obstacle> GetActiveObstacles()
    {
        return activeObstacles;
    }

    public void ClearObstacles()
    {
        foreach (var obstacle in activeObstacles.ToArray())
        {
            Destroy(obstacle.gameObject);
        }
        activeObstacles.Clear();
    }
}
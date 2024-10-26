using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Obstacle obstaclePrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float heightRange = 2f;
    [SerializeField] private int initialPoolSize = 5;

    private ObjectPool<Obstacle> obstaclePool;
    private float timer;
    private bool isSpawning = true;

    private void Start()
    {
        // Initialize object pool
        obstaclePool = new ObjectPool<Obstacle>(
            obstaclePrefab,
            initialPoolSize,
            transform
        );
    }

    private void Update()
    {
        if (!isSpawning) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0;
        }
    }

    private void SpawnObstacle()
    {
        // Get obstacle from pool instead of instantiating
        var obstacle = obstaclePool.Get();

        // Reset position and properties
        float randomY = Random.Range(-heightRange, heightRange);
        obstacle.transform.position = transform.position + Vector3.up * randomY;
        obstacle.ResetObstacle();
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    public void StartSpawning()
    {
        isSpawning = true;
        timer = 0;
    }

    public void ClearObstacles()
    {
        // Return all active obstacles to pool
        var activeObstacles = ObstacleManager.Instance.GetActiveObstacles();
        foreach (var obstacle in activeObstacles.ToArray())
        {
            ReturnObstacle(obstacle);
        }
    }

    public void ReturnObstacle(Obstacle obstacle)
    {
        obstaclePool.Return(obstacle);
    }

    private void OnDestroy()
    {
        // Clean up pool when spawner is destroyed
        obstaclePool.Clear();
    }
}
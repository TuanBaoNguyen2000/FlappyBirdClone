using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float scrollSpeed = 3f;
    [SerializeField] private float gapSize = 4f;
    [SerializeField] private float yPosRange = 3f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0;
        }
    }

    private void SpawnObstacle()
    {
        float randomY = Random.Range(-yPosRange, yPosRange);
        Vector3 spawnPos = new Vector3(transform.position.x, randomY, 0);

        GameObject obstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
        Obstacle obstacleScript = obstacle.AddComponent<Obstacle>();
        obstacleScript.Initialize(scrollSpeed, gapSize);
    }
}
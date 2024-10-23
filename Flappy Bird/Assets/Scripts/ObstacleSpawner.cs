using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float heightRange = 2f;

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
        // Randomize height position
        float randomY = Random.Range(-heightRange, heightRange);
        Vector3 spawnPos = transform.position + Vector3.up * randomY;

        Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
    }
}
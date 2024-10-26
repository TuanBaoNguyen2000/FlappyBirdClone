using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Pipe Settings")]
    [SerializeField] private float pipeWidth = 1f;
    [SerializeField] private float pipeHeight = 10f;
    [SerializeField] private float gapSize = 4f;
    [SerializeField] private float scrollSpeed = 3f;

    [Header("Debug Visualization")]
    [SerializeField] private bool showCollisionGizmos = true;
    [SerializeField] private Color collisionColor = Color.green;

    private float destroyXPosition = -10f;
    public bool IsScored { get; set; }
    private ObstacleSpawner spawner => ObstacleManager.Instance.ObstacleSpawner;

    private void OnEnable()
    {
        ObstacleManager.Instance.RegisterObstacle(this);
    }

    private void OnDisable()
    {
        ObstacleManager.Instance.UnregisterObstacle(this);
    }

    public void ResetObstacle()
    {
        IsScored = false;
    }

    private void Update()
    {
        // Move obstacle
        transform.position += scrollSpeed * Time.deltaTime * Vector3.left ;

        // Destroy if off screen
        if (transform.position.x < destroyXPosition)
        {
            spawner.ReturnObstacle(this);
        }
    }
    public float GetMoveSpeed()
    {
        return scrollSpeed;
    }

    public (Rect topRect, Rect bottomRect) GetPipeRects()
    {
        // Calculate centers of pipes
        Vector2 topCenter = transform.position + Vector3.up * (gapSize / 2 + pipeHeight / 2);
        Vector2 bottomCenter = transform.position + Vector3.down * (gapSize / 2 + pipeHeight / 2);

        // Create rectangles for both pipes
        Rect topRect = new Rect(
            topCenter.x - pipeWidth / 2,    // xMin
            topCenter.y - pipeHeight / 2,    // yMin
            pipeWidth,                     // width
            pipeHeight                     // height
        );

        Rect bottomRect = new Rect(
            bottomCenter.x - pipeWidth / 2,  // xMin
            bottomCenter.y - pipeHeight / 2,  // yMin
            pipeWidth,                     // width
            pipeHeight                     // height
        );

        return (topRect, bottomRect);
    }


    private void OnDrawGizmos()
    {
        if (!showCollisionGizmos) return;

        var (topRect, bottomRect) = GetPipeRects();

        // Draw pipe rectangles
        Gizmos.color = collisionColor;

        // Top pipe
        Gizmos.DrawWireCube(
            new Vector3(topRect.center.x, topRect.center.y, 0),
            new Vector3(topRect.width, topRect.height, 0)
        );

        // Bottom pipe
        Gizmos.DrawWireCube(
            new Vector3(bottomRect.center.x, bottomRect.center.y, 0),
            new Vector3(bottomRect.width, bottomRect.height, 0)
        );
    }
}

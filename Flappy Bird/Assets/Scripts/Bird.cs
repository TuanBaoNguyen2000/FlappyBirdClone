using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float maxFallSpeed = -10f;

    [Header("Collision Settings")]
    [SerializeField] private float birdRadius = 0.25f; // Bird's collision circle radius

    private Vector2 velocity;
    private bool isDead;
    private bool isAIEnabled;
    private GameManager gameManager => GameManager.Instance;

    [Header("Debug Visualization")]
    [SerializeField] private bool showCollisionGizmos = true;
    [SerializeField] private Color collisionColor = Color.yellow;

    public float JumpForce => jumpForce;
    public float BirdRadius => birdRadius;

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        isDead = false;
        velocity = Vector2.zero;
        transform.position = new Vector3(-2f, 0f, 0f);
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (isDead) return;

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, maxFallSpeed);

        // Move bird
        transform.position += new Vector3(0, velocity.y * Time.deltaTime, 0);

        // Handle input
        if (!isAIEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Jump();
            }
        }

        // Update rotation based on velocity
        UpdateRotation();

        // Check collisions
        CheckCollisions();
    }

    private void UpdateRotation() 
    {
        float angle = Mathf.Lerp(-30f, 90f, -velocity.y / maxFallSpeed);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void CheckCollisions()
    {
        // Get all obstacles
        var obstacles = ObstacleManager.Instance.GetActiveObstacles();

        foreach (var obstacle in obstacles)
        {
            if (CheckCollisionWithObstacle(obstacle))
            {
                Die();
                return;
            }

            // Score point if passed obstacle
            if (!obstacle.IsScored && transform.position.x > obstacle.transform.position.x)
            {
                obstacle.IsScored = true;
                gameManager.AddScore();
            }
        }

        // Check boundaries (ground and ceiling)
        if (transform.position.y > 5f || transform.position.y < -5f)
        {
            Die();
        }
    }

    private bool CheckCollisionWithObstacle(Obstacle obstacle)
    {
        // Get pipe rectangles from obstacle
        var (topRect, bottomRect) = obstacle.GetPipeRects();

        // Check collision with both pipes
        return CheckCircleRectCollision(topRect) || CheckCircleRectCollision(bottomRect);
    }

    private bool CheckCircleRectCollision(Rect rect)
    {
        // Get circle center (bird position)
        Vector2 circleCenter = transform.position;

        // Find the closest point on the rectangle to the circle
        float closestX = Mathf.Clamp(circleCenter.x, rect.xMin, rect.xMax);
        float closestY = Mathf.Clamp(circleCenter.y, rect.yMin, rect.yMax);
        Vector2 closestPoint = new Vector2(closestX, closestY);

        // Calculate the distance between circle center and closest point
        float distanceSquared = Vector2.SqrMagnitude(circleCenter - closestPoint);

        // Compare with radius squared (avoiding square root for performance)
        return distanceSquared <= (birdRadius * birdRadius);
    }

    public void Jump()
    {
        velocity.y = jumpForce;
        SoundManager.Instance.PlaySound("Jump");
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        velocity = Vector2.zero;
        SoundManager.Instance.PlaySound("Hit");
        gameManager.GameOver();
    }

    public Vector2 GetVelocity()
    {
        return velocity;
    }

    public float GetGravity()
    {
        return gravity;
    }

    public void SetAIEnabled(bool enabled)
    {
        isAIEnabled = enabled;
        GetComponent<BirdAIController>().SetEnabled(enabled);
    }

    private void OnDrawGizmos()
    {
        if (!showCollisionGizmos) return;

        // Draw bird's collision circle
        Gizmos.color = collisionColor;
        Gizmos.DrawWireSphere(transform.position, birdRadius);

        if (Application.isPlaying)
        {
            // Draw velocity vector
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, velocity * 0.5f);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAIController : MonoBehaviour
{
    [Header("AI Settings")]
    [SerializeField] private float lookaheadDistance = 5f; // Distance to look ahead for obstacles
    [SerializeField] private float jumpThreshold = 0.5f; // How close to optimal height before jumping
    [SerializeField] private bool isEnabled = false;
    [SerializeField] private float jumpCooldown = 0.3f;

    [Header("Safe Flight Settings")]
    [SerializeField] private float safeMinHeight = -3f; // Minimum safe height
    [SerializeField] private float safeMaxHeight = 3f;  // Maximum safe height
    [SerializeField] private float safetyJumpThreshold = 0.8f; // How close to dangerous height before emergency jump

    [Header("Debug Visualization")]
    [SerializeField] private bool showDebugGizmos = true;
    [SerializeField] private Color debugLineColor = Color.yellow;
    [SerializeField] private Color safeBoundsColor = Color.green;
    [SerializeField] private Color predictionLineColor = Color.red;

    private Bird bird;
    private Obstacle targetObstacle;
    private float optimalHeight;
    private float lastPredictedHeight;
    private float lastJumpTime;

    private void Start()
    {
        bird = GetComponent<Bird>();
        lastJumpTime = -jumpCooldown;
    }

    private void Update()
    {
        if (!isEnabled || bird == null) return;

        UpdateTargetObstacle();

        if (targetObstacle != null)
        {
            HandleObstacleAvoidance();
        }
        else
        {
            MaintainSafeAltitude();
        }
    }

    private void UpdateTargetObstacle()
    {
        float closestDistance = float.MaxValue;
        targetObstacle = null;
        optimalHeight = 0f;

        float jumpMargin = 1f; // Downward adjustment to give space for the jump

        var obstacles = ObstacleManager.Instance.GetActiveObstacles();
        foreach (var obstacle in obstacles)
        {
            float distance = obstacle.transform.position.x - transform.position.x;

            if (distance > 0 && distance < lookaheadDistance && distance < closestDistance)
            {
                closestDistance = distance;
                targetObstacle = obstacle;

                // Calculate the optimal height and safe zone boundaries
                var (topRect, bottomRect) = obstacle.GetPipeRects();
                float gapSize = (topRect.yMin - bottomRect.yMax);
                optimalHeight = bottomRect.yMax + (gapSize / 2f) - jumpMargin;
                safeMaxHeight = topRect.yMin;
                safeMinHeight = bottomRect.yMax;
            }
        }
    }

    private bool CanJump()
    {
        return Time.time >= lastJumpTime + jumpCooldown;
    }

    private void TryJump()
    {
        if (CanJump())
        {
            bird.Jump();
            lastJumpTime = Time.time;
        }
    }

    private void HandleObstacleAvoidance()
    {
        float currentHeight = transform.position.y;
        float currentVelocity = bird.GetVelocity().y;
        float gravity = bird.GetGravity();
        float timeToObstacle = (targetObstacle.transform.position.x - transform.position.x) / targetObstacle.GetMoveSpeed();

        // Calculate predicted height at obstacle
        float predictedHeight = currentHeight + (currentVelocity * timeToObstacle) +
                                (0.5f * gravity * timeToObstacle * timeToObstacle);

        lastPredictedHeight = predictedHeight;

        float heightDifference = optimalHeight - predictedHeight;

        if (heightDifference > jumpThreshold && currentHeight < optimalHeight)
        {
            TryJump();
        }

    }

    private void MaintainSafeAltitude()
    {
        float currentHeight = transform.position.y;
        float currentVelocity = bird.GetVelocity().y;

        // Predict height after a short time
        float predictionTime = 0.4f; 
        float gravity = bird.GetGravity();
        float predictedHeight = currentHeight + (currentVelocity * predictionTime) +
                              (0.5f * gravity * predictionTime * predictionTime);

        // Emergency jump conditions
        bool tooLow = predictedHeight < safeMinHeight + safetyJumpThreshold;

        if (tooLow)
        {
            TryJump();
        }

        // Prevent jumping too high
        if (currentHeight > safeMaxHeight - safetyJumpThreshold && currentVelocity > 0)
        {
            // Let gravity do its work, don't jump
            return;
        }
    }

    private void OnDrawGizmos()
    {
        if (!showDebugGizmos) return;

        // Draw safe flight bounds
        Gizmos.color = safeBoundsColor;
        float xPos = transform.position.x + lookaheadDistance / 2f;

        // Draw safe zone boundaries
        Gizmos.DrawLine(
            new Vector3(transform.position.x - 1f, safeMinHeight, 0),
            new Vector3(transform.position.x + lookaheadDistance, safeMinHeight, 0)
        );
        Gizmos.DrawLine(
            new Vector3(transform.position.x - 1f, safeMaxHeight, 0),
            new Vector3(transform.position.x + lookaheadDistance, safeMaxHeight, 0)
        );

        // Draw target info if available
        if (targetObstacle != null)
        {
            Gizmos.color = debugLineColor;
            Gizmos.DrawLine(transform.position,
                new Vector3(targetObstacle.transform.position.x, optimalHeight, 0));
            Gizmos.DrawWireSphere(
                new Vector3(targetObstacle.transform.position.x, optimalHeight, 0), 0.2f);

            Gizmos.color = predictionLineColor;
            Gizmos.DrawWireSphere(
                new Vector3(targetObstacle.transform.position.x, lastPredictedHeight, 0), 0.2f);
        }
    }

    public void SetEnabled(bool enabled)
    {
        isEnabled = enabled;
    }
}
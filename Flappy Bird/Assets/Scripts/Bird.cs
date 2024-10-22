using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float maxFallSpeed = -10f;

    private Vector2 velocity;
    private bool isDead;
    private bool isAIEnabled;

    [Header("AI Settings")]
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float jumpThreshold = 1.5f;

    private void Update()
    {
        if (isDead) return;

        // Apply custom gravity
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
        else
        {
            HandleAI();
        }

        // Update rotation based on velocity
        float angle = Mathf.Lerp(-30f, 90f, -velocity.y / maxFallSpeed);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void HandleAI()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, raycastDistance, obstacleLayer);
        if (hit.collider != null)
        {
            float heightDifference = hit.point.y - transform.position.y;
            if (heightDifference > jumpThreshold || velocity.y < -2f)
            {
                Jump();
            }
        }
    }

    public void Jump()
    {
        velocity.y = jumpForce;
    }

    public void Die()
    {
        isDead = true;
        velocity = Vector2.zero;
    }

    public void SetAIEnabled(bool enabled)
    {
        isAIEnabled = enabled;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.right * raycastDistance);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float scrollSpeed;
    private float destroyXPosition = -15f;

    public void Initialize(float speed, float gapSize)
    {
        scrollSpeed = speed;

        // Create top and bottom parts
        GameObject top = CreatePipe(true, gapSize / 2);
        GameObject bottom = CreatePipe(false, -gapSize / 2);

        top.transform.SetParent(transform);
        bottom.transform.SetParent(transform);
    }

    private GameObject CreatePipe(bool isTop, float yOffset)
    {
        GameObject pipe = new GameObject(isTop ? "TopPipe" : "BottomPipe");
        pipe.transform.position = transform.position + new Vector3(0, yOffset, 0);

        // Add custom collision detection
        BoxCollider2D collider = pipe.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(1, 10);
        collider.isTrigger = true;

        return pipe;
    }

    private void Update()
    {
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        if (transform.position.x < destroyXPosition)
        {
            Destroy(gameObject);
        }
    }
}

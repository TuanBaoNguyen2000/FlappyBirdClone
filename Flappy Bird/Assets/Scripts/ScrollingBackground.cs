using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Renderer bgRenderer;

    private void Update()
    {
        if (bgRenderer != null)
        {
            bgRenderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjustment : MonoBehaviour
{
    public float targetAspect = 0.8f; 

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        float currentAspect = (float)Screen.width / Screen.height;

        if (currentAspect > targetAspect)
        {
            float inset = 1.0f - (targetAspect / currentAspect);
            cam.rect = new Rect(inset / 2, 0, 1.0f - inset, 1.0f);
        }
        else if (currentAspect < targetAspect)
        {
            float inset = 1.0f - (currentAspect / targetAspect);
            cam.rect = new Rect(0, inset / 2, 1.0f, 1.0f - inset);
        }
    }
}

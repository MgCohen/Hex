using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraUtility
{
    public static float CalculateOrtho(float size)
    {
        return CalculateOrtho(new Vector2(size, size));
    }

    public static float CalculateOrtho(Vector2 area)
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = area.x / area.y;

        if(screenRatio > targetRatio)
        {
            return area.y / 2f;
        }
        else
        {
            float diff = targetRatio/screenRatio;
            return (area.y / 2f) * diff;
        }
    }
}

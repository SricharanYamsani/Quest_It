using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static float MapTo(float min, float max, float value, float newMin, float newMax)
    {
        return newMin + ((value - min) / (max - min)) * (newMax - newMin);
    }
    // Extension Methods.
    public static int RoundToInt(this float x)
    {
        float decimalValue = x - (int)x;

        if (decimalValue > 0.5f)
        {
            return (int)x + 1;
        }

        return (int)x;
    }

    public static int Clamp(this int x, int min, int max)
    {
        if (x < min)
        {
            x = min;
        }
        else if (x > max)
        {
            x = max;
        }

        return x;
    }
    public static float Clamp(this float x, float min, float max)
    {
        if (x < min)
        {
            x = min;
        }
        else if (x > max)
        {
            x = max;
        }

        return x;
    }
}

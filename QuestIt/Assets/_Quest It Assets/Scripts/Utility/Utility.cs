using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Utility
{
    public static float MapTo(float min,float max,float value,float newMin,float newMax)
    {
        return newMin + ((value - min) / (max - min)) * (newMax - newMin);
    }
}

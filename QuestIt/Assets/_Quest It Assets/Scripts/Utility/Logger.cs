#if !DEBUG_LOGGER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logger
{
    public static void Error(string value)
    {
        Debug.LogError(value);
    }

    public static void Log(string value)
    {
        Debug.Log(value);
    }

    public static void Warning(string value)
    {
        Debug.LogWarning(value);
    }
}
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logger
{
    public static void Error(string value)
    {
#if DEBUG_LOGGER
        Debug.LogError(value);
#endif
    }

    public static void Log(string value)
    {
#if DEBUG_LOGGER
        Debug.Log(value);
#endif
    }

    public static void Warning(string value)
    {
#if DEBUG_LOGGER
        Debug.LogWarning(value);
#endif
    }
}

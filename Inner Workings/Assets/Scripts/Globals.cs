using UnityEngine;

public static class HelperFunctions
{
    /// <summary>
    /// Normalized a -1.0f -> 1.0f float to a 0.0f -> 1.0f float
    /// </summary>
    public static float Normalize(this float f)
    {
        return f * 0.5f + 0.5f;
    }
}
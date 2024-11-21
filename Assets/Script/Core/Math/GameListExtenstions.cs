using System;
using System.Collections.Generic;

public static class GameListExtenstions
{
    public static float Min(this List<float> list)
    {
        float min = list[0];
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i] < min)
            {
                min = list[i];
            }
        }
        return min;
    }

    public static float Min(this Span<float> list)
    {
        float min = list[0];
        for (int i = 1; i < list.Length; i++)
        {
            if (list[i] < min)
            {
                min = list[i];
            }
        }
        return min;
    }

    public static float Max(this List<float> list)
    {
        float max = list[0];
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i] > max)
            {
                max = list[i];
            }
        }
        return max;
    }

    public static float Max(this Span<float> list)
    {
        float max = list[0];
        for (int i = 1; i < list.Length; i++)
        {
            if (list[i] > max)
            {
                max = list[i];
            }
        }
        return max;
    }
}
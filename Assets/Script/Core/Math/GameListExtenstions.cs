using System;
using System.Collections.Generic;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Core
{
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


        public static GameVec2 Min(this Span<GameVec2> list)
        {
            GameVec2 min = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                if (list[i].sqrMagnitude < min.sqrMagnitude)
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
}

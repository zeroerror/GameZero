namespace GamePlay.Core
{
    public static class GameMathF
    {
        public static readonly float Deg2Rad = 0.0174532924f;
        public static readonly float Rad2Deg = 57.29578f;

        public static float Min(float a, float b) => a < b ? a : b;
        public static float Min(params float[] args)
        {
            if (args.Length == 0) return 0;
            float min = args[0];
            for (int i = 1; i < args.Length; i++)
            {
                if (args[i] < min) min = args[i];
            }
            return min;
        }

        public static float Max(float a, float b) => a > b ? a : b;

        public static float Max(params float[] args)
        {
            if (args.Length == 0) return 0;
            float max = args[0];
            for (int i = 1; i < args.Length; i++)
            {
                if (args[i] > max) max = args[i];
            }
            return max;
        }

        public static float Abs(float a) => a < 0 ? -a : a;
        public static float Sin(float a) => (float)System.Math.Sin(a);
        public static float Cos(float a) => (float)System.Math.Cos(a);

        public static bool NearlyEqual(this float a, float b, float epsilon = 0.0001f)
        {
            return GameMathF.Abs(a - b) < epsilon;
        }

        public static int FloorToInt(float a) => (int)System.Math.Floor(a);

        public static float Random(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static float Random()
        {
            return UnityEngine.Random.value;
        }

        public static float GetFixed(float value, int digits)
        {
            return System.MathF.Round(value, digits);
        }

        public static float ToFixed(this float value, int digits)
        {
            return System.MathF.Round(value, digits);
        }

        /// <summary>
        /// [min..max)
        /// </summary>
        /// <returns></returns>
        public static float RandomRange(float min, float max) => UnityEngine.Random.Range(min, max);
        public static float RandomRange(in UnityEngine.Vector2 range) => RandomRange(range.x, range.y);
    }
}
namespace Game.Core
{
    public static class GameMathF
    {
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

    }
}
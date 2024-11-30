namespace GamePlay.Core
{
    public static class GameMath
    {
        public static int Min(int a, int b) => a < b ? a : b;
        public static int Min(params int[] args)
        {
            if (args.Length == 0) return 0;
            int min = args[0];
            for (int i = 1; i < args.Length; i++)
            {
                if (args[i] < min) min = args[i];
            }
            return min;
        }

        public static int Max(int a, int b) => a > b ? a : b;

        public static int Max(params int[] args)
        {
            if (args.Length == 0) return 0;
            int max = args[0];
            for (int i = 1; i < args.Length; i++)
            {
                if (args[i] > max) max = args[i];
            }
            return max;
        }

        public static int Abs(int a) => a < 0 ? -a : a;
        public static int Sin(int a) => (int)System.Math.Sin(a);
        public static int Cos(int a) => (int)System.Math.Cos(a);

    }
}
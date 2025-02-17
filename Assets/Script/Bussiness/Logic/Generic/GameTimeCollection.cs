namespace GamePlay.Bussiness.Logic
{
    public static class GameTimeCollection
    {
        public static readonly int frameRate = 60;
        public static readonly float frameTime = 1.0f / frameRate;

        public static int ToFrame(this float time)
        {
            return (int)(time * frameRate);
        }

        public static float ToTime(this int frame)
        {
            return frame * frameTime;
        }
    }
}
namespace GamePlay.Bussiness.Render
{
    public class GameDrawTask
    {
        public System.Action drawAction;
        public float maintainTime;
        public float elapsedTime;

        public bool isOver()
        {
            return elapsedTime >= maintainTime;
        }
    }
}
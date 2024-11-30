namespace GamePlay.Bussiness.Renderer
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
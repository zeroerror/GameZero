namespace GamePlay.Bussiness.Render
{
    public class GameActionContextR
    {
        public GameActionTemplateR template { get; private set; }

        public GameActionContextR()
        {
            this.template = new GameActionTemplateR();
        }
    }
}
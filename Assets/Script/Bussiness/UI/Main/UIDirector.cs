using GamePlay.Bussiness.Logic;
namespace GamePlay.Bussiness.UI
{
    public class UIDirector
    {
        public GameDirectorTimelineComponent timeScaleCom { get; private set; } = new GameDirectorTimelineComponent();

        public void Tick(float dt)
        {
            this.timeScaleCom.Tick(dt);
        }
    }
}
namespace GamePlay.Bussiness.Renderer
{
    public class GameCoreR
    {
        public GameDirectDomainR directDomain { get; private set; } = new GameDirectDomainR();

        public void Update(float dt)
        {
            this.directDomain.Update(dt);
        }

        public void LateUpdate(float dt)
        {
            this.directDomain.LateUpdate(dt);
        }
    }
}
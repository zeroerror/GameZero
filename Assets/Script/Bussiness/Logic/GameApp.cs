using UnityEngine;

namespace GamePlay.Bussiness.Logic
{
    public class GameApp
    {
        public GameDirectDomain directDomain { get; private set; }

        public GameApp()
        {
            this.directDomain = new GameDirectDomain();
            Application.quitting += this.Destroy;
        }

        public void Destroy()
        {
            this.directDomain.Destroy();
        }

        public void Update(float dt)
        {
            this.directDomain.Update(dt);
        }
    }
}
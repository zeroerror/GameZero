using UnityEngine;

namespace GamePlay.Bussiness.Logic
{
    public class GameApp
    {
        public GameDirectDomain directDomain { get; private set; }

        public GameApp()
        {
            this.directDomain = new GameDirectDomain();
            Application.quitting += this.Dispose;
        }

        public void Dispose()
        {
            this.directDomain.Dispose();
        }

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
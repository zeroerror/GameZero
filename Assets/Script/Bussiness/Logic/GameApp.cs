using UnityEngine;

namespace GamePlay.Bussiness.Logic
{
    public class GameApp
    {
        public GameDirectorDomain directDomain { get; private set; }

        public GameApp()
        {
            this.directDomain = new GameDirectorDomain();
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
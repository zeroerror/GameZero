using UnityEngine;

namespace GamePlay.Bussiness.Logic
{
    public class GameApp
    {
        public GameDirectorDomain directorDomain { get; private set; }

        public GameApp()
        {
            this.directorDomain = new GameDirectorDomain();
            Application.quitting += this.Destroy;
        }

        public void Destroy()
        {
            this.directorDomain.Destroy();
        }

        public void Update(float dt)
        {
            this.directorDomain.Update(dt);
        }
    }
}
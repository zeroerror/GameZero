using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public class GameUIApp
    {
        public GameUIDirectDomain directDomain { get; private set; }
        public GameUIApp(GameObject uiRoot)
        {
            this.directDomain = new GameUIDirectDomain(uiRoot);
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

        public void LateUpdate(float dt)
        {
            this.directDomain.LateUpdate(dt);
        }
    }
}
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public class UIApp
    {
        public UIDirectDomain directDomain { get; private set; }

        public UIApp()
        {
            this.directDomain = new UIDirectDomain();
            Application.quitting += this.Destroy;
        }

        public void Inject(GameObject uiRoot, GameDomainApi logicApi, GameDomainApiR rendererApi)
        {
            this.directDomain.Inject(uiRoot, logicApi, rendererApi);
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
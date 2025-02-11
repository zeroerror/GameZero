using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Render;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public class UIApp
    {
        public UIDirectorDomain directorDomain { get; private set; }

        public UIApp()
        {
            this.directorDomain = new UIDirectorDomain();
            Application.quitting += this.Destroy;
        }

        public void Inject(GameObject uiRoot, GameDomainApi logicApi, GameDomainApiR rendererApi)
        {
            this.directorDomain.Inject(uiRoot, logicApi, rendererApi);
            Application.quitting += this.Destroy;
        }

        public void Destroy()
        {
            this.directorDomain.Destroy();
            Application.quitting -= this.Destroy;
        }

        public void Update(float dt)
        {
            this.directorDomain.Update(dt);
        }

        public void LateUpdate(float dt)
        {
            this.directorDomain.LateUpdate(dt);
        }
    }
}
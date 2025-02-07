using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
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

        public void BindEvents()
        {
            this.directorDomain.BindEvents();
        }

        public void UnbindEvents()
        {
            this.directorDomain.UnbindEvents();
        }

        public void Destroy()
        {
            this.directorDomain.UnbindEvents();
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
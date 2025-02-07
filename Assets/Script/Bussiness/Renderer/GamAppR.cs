using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.UI;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameAppR
    {
        public GameDirectorDomainR directorDomain { get; private set; }

        public GameAppR()
        {
            this.directorDomain = new GameDirectorDomainR();
            Application.quitting += this.Destroy;
        }

        public void Inject(GameObject sceneRoot, GameDomainApi logicApi, UIDomainApi uiApi)
        {
            this.directorDomain.Inject(sceneRoot, logicApi, uiApi);
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
            this.UnbindEvents();
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
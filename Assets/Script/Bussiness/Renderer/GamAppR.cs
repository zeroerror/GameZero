using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.UI;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameAppR
    {
        public GameDirectDomainR directDomain { get; private set; }

        public GameAppR(GameContext logicContext, GameObject sceneRoot, GameUIContext uiContext)
        {
            this.directDomain = new GameDirectDomainR(logicContext, sceneRoot, uiContext);
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
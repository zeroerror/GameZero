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
            Application.quitting += this.Dispose;
        }

        public void Dispose()
        {
        }

        public void Update(float dt)
        {
        }

        public void LateUpdate(float dt)
        {
        }
    }
}
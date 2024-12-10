using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public class GameUIDirectDomain
    {
        public GameUIContext context { get; private set; }

        public GameUIDirectDomain(GameObject uiRoot)
        {
            this._InitContext(uiRoot);
        }

        private void _InitContext(GameObject uiRoot)
        {
            this.context = new GameUIContext(uiRoot);
        }
    }
}
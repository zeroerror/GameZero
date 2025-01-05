using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public class GameUILayerDomain : GameUILayerDomainApi
    {
        GameUIContext _context;

        public GameUILayerDomain()
        {
        }

        public void Inject(GameUIContext context)
        {
            this._context = context;
            this._BindEvent();
        }

        public void Destroy()
        {
            this._UnbindEvents();
        }

        private void _BindEvent()
        {
        }

        private void _UnbindEvents()
        {
        }

        public void Tick(float dt)
        {
        }

        public void AddToUIRoot(Transform transform, GameUILayerType layerType)
        {
            var parent = this._context.layerDict[layerType].transform;
            transform.SetParent(parent, false);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
        }
    }
}
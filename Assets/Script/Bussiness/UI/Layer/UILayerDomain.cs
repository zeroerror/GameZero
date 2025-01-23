using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public class UILayerDomain : UILayerDomainApi
    {
        UIContext _context;

        public UILayerDomain()
        {
        }

        public void Inject(UIContext context)
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

        public void AddToUIRoot(Transform transform, UILayerType layerType)
        {
            var parent = this._context.layerDict[layerType].transform;
            transform.SetParent(parent, false);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
        }
    }
}
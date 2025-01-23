using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public interface UILayerDomainApi
    {
        public void AddToUIRoot(Transform transform, UILayerType layerType);
    }
}
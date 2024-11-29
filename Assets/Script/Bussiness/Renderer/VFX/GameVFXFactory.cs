using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameVFXFactoryR
    {
        public GameObject entityLayer { get; private set; }
        public GameVFXFactoryR()
        {
            this.entityLayer = GameObject.Find(GameFieldLayerCollection.DynamicLayer);
        }

        public GameVFXEntityR Load()
        {
            var go = new GameObject();
            go.transform.SetParent(this.entityLayer.transform);
            go.transform.localPosition = new Vector3(0, 0, 0);
            var entity = new GameVFXEntityR(go);
            return entity;
        }
    }
}
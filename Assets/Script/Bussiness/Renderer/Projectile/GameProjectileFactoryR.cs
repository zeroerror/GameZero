using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameProjectileFactoryR
    {
        public GameProjectileTemplateR template { get; private set; }
        public GameObject entityLayer { get; private set; }

        public GameProjectileFactoryR()
        {
            template = new GameProjectileTemplateR();
            this.entityLayer = GameObject.Find(GameFieldLayerCollection.DynamicLayer);
        }

        public GameProjectileEntityR Load(int typeId)
        {
            if (!template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("投射物创建失败，投射物ID不存在：" + typeId);
                return null;
            }

            var go = new GameObject();
            var body = new GameObject();
            body.name = "Body";
            body.transform.SetParent(go.transform);
            body.AddComponent<Animator>().runtimeAnimatorController = null;
            body.AddComponent<SpriteRenderer>();
            go.transform.SetParent(this.entityLayer.transform);
            go.transform.localPosition = new Vector3(0, 0, 0);

            var entity = new GameProjectileEntityR(model, go);
            return entity;
        }
    }
}
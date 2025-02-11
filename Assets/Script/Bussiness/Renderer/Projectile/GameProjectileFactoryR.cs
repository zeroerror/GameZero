using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public class GameProjectileFactoryR
    {
        public GameProjectileTemplateR template { get; private set; }

        public GameProjectileFactoryR()
        {
            template = new GameProjectileTemplateR();
        }

        public GameProjectileEntityR Load(int typeId)
        {
            if (!template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("投射物创建失败，投射物ID不存在：" + typeId);
                return null;
            }

            var go = new GameObject();
            go.transform.localPosition = new Vector3(0, 0, 0);

            Animator animator = null;
            if (model.animClip)
            {
                var clipBody = new GameObject();
                clipBody.name = "Body";
                clipBody.transform.SetParent(go.transform);
                animator = clipBody.AddComponent<Animator>();
                clipBody.AddComponent<SpriteRenderer>();
            }

            var prefabUrl = model.prefabUrl;
            var psGo = prefabUrl != null ? GameObject.Instantiate(Resources.Load<GameObject>(prefabUrl)) : null;
            psGo?.transform.SetParent(go.transform);
            var ps = psGo?.GetComponent<ParticleSystem>();
            if (psGo)
            {
                psGo.transform.eulerAngles = new Vector3(0, 0, 0);
                psGo.transform.localScale = model.prefabScale;
                psGo.transform.localPosition = new Vector3(model.prefabOffset.x, model.prefabOffset.y, 0);
            }

            var entity = new GameProjectileEntityR(model, go, animator, ps);
            return entity;
        }
    }
}
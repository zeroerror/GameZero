using GamePlay.Core;
using UnityEngine;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleFactoryR
    {
        public GameObject entityLayer { get; private set; }
        private GameContextR _context;

        public GameRoleFactoryR(GameContextR context)
        {
            this._context = context;
            this.entityLayer = GameObject.Find("Field/DynamicLayer");
        }

        public GameRoleEntityR Load(int typeId)
        {
            var res = Resources.Load<GameObject>("Role/Prefab/role");
            var go = GameObject.Instantiate(res);
            var body = go.transform.Find("body").gameObject;
            body.AddComponent<Animator>().runtimeAnimatorController = null;
            body.AddComponent<SpriteRenderer>();
            go.transform.SetParent(this.entityLayer.transform);
            go.transform.localPosition = new Vector3(0, 0, 0);
            var e = new GameRoleEntityR(typeId, go);
            return e;
        }

        public AnimationClip LoadAnimationClip(int typeId, string clipName)
        {
            var res = Resources.Load<AnimationClip>($"Role/{typeId}/Anim/{clipName}");
            if (!res)
            {
                GameLogger.Error($"角色工厂[渲染层]: 加载动画失败 {typeId} {clipName}");
            }
            return res;
        }
    }
}
using GamePlay.Core;
using UnityEngine;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleFactoryR
    {
        public GameObject entityLayer { get; private set; }

        public GameRoleFactoryR()
        {
            this.entityLayer = GameObject.Find("Field/Dynamic");
            GameLogger.Log("角色工厂[渲染层]: 创建角色根节点");
        }

        public GameRoleEntityR Load(int roleId = 1000)
        {
            var res = Resources.Load<GameObject>("Role/Prefab/role");
            var go = GameObject.Instantiate(res);
            var e = new GameRoleEntityR(go);
            e.go.transform.SetParent(this.entityLayer.transform);
            e.go.transform.localPosition = new Vector3(0, 0, 0);
            GameLogger.Log($"角色工厂[渲染层]: 创建角色实体 {e.idCom.entityId}");
            return e;
        }
    }
}
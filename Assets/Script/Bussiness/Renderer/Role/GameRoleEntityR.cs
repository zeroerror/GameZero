using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleEntityR : GameEntityBase
    {
        public GameVec2 position { get { return this.go.transform.position; } set { this.go.transform.position = value; } }
        public GameObject go { get; private set; }
        public GameObject foot { get; private set; }
        public GameObject body { get; private set; }

        public GameRoleFSMComR fsmCom { get; private set; }
        public GameAnimPlayableCom animCom { get; private set; }

        public GameRoleEntityR(int typeId, GameObject go) : base(typeId, GameEntityType.Role)
        {
            this.go = go;
            go.name = "role_" + this.idCom.entityId;
            this.foot = go.transform.Find("foot").gameObject;
            this.body = go.transform.Find("body").gameObject;
            this.fsmCom = new GameRoleFSMComR();
            var animator = this.body.GetComponent<Animator>();
            this.animCom = new GameAnimPlayableCom(animator);
        }

        public override void Tick(float dt)
        {
            this.animCom.Tick(dt);
        }

        public override void Reset(float dt) { }

        public override void Dispose()
        {
            base.Dispose();
            this.animCom.Dispose();
        }
    }
}
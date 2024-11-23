using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
using GameVec3 = UnityEngine.Vector3;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleEntityR : GameEntityBase
    {

        public GameObject go { get; private set; }
        public GameObject foot { get; private set; }
        public GameObject body { get; private set; }

        public Transform transform { get { return this.go.transform; } }
        public GameVec2 position { get { return transform.position; } set { transform.position = value; } }
        public float angle { get { return transform.eulerAngles.z; } set { transform.eulerAngles = new GameVec3(0, 0, value); } }
        public float scale { get { return transform.localScale.x; } set { transform.localScale = new GameVec3(value, value, 1); } }
        public float scaleX { get { return transform.localScale.x; } set { transform.localScale = new GameVec3(value, transform.localScale.y, transform.localScale.z); } }
        public float scaleY { get { return transform.localScale.y; } set { transform.localScale = new GameVec3(transform.localScale.x, value, transform.localScale.z); } }

        public GameRoleFSMComR fsmCom { get; private set; }
        public GameAnimPlayableCom animCom { get; private set; }

        private GameEasing2DCom _posEaseCom;

        public GameRoleEntityR(int typeId, GameObject go) : base(typeId, GameEntityType.Role)
        {
            this.go = go;
            go.name = "role_" + this.idCom.entityId;
            this.foot = go.transform.Find("foot").gameObject;
            this.body = go.transform.Find("body").gameObject;
            this.fsmCom = new GameRoleFSMComR();
            var animator = this.body.GetComponent<Animator>();
            this.animCom = new GameAnimPlayableCom(animator);
            this._posEaseCom = new GameEasing2DCom();
            this._posEaseCom.SetEase(0.05f, GameEasingType.Linear);
        }

        public override void Tick(float dt)
        {
            this.animCom.Tick(dt);
            var pos = this._posEaseCom.Tick(this.position, this.transformCom.position, dt);
            this.position = pos;
            var forward = this.transformCom.forward;
            this.scaleX = forward.x > 0 ? 1 : -1;
        }

        public override void Reset(float dt) { }

        public override void Dispose()
        {
            base.Dispose();
            this.animCom.Dispose();
        }

        public void SyncTrans()
        {
            this.position = this.transformCom.position;
            this.angle = this.transformCom.angle;
            this.scale = this.transformCom.scale;
        }
    }
}
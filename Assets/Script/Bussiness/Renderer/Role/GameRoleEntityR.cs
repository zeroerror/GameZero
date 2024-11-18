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


        public GameVec2 position { get { return this.go.transform.position; } set { this.go.transform.position = value; } }
        public float angle { get { return this.go.transform.eulerAngles.z; } set { this.go.transform.eulerAngles = new GameVec3(0, 0, value); } }
        public float scale { get { return this.go.transform.localScale.x; } set { this.go.transform.localScale = new GameVec3(value, value, value); } }

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
            this._posEaseCom.SetEase(0.1f, GameEasingType.Linear);
        }

        public override void Tick(float dt)
        {
            this.animCom.Tick(dt);
            var pos = this._posEaseCom.Tick(this.position, this.transformCom.position, dt);
            this.position = pos;
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
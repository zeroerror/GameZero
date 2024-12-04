using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
using GameVec3 = UnityEngine.Vector3;

namespace GamePlay.Bussiness.Renderer
{
    public class GameProjectileEntityR : GameEntityBase
    {
        public readonly GameProjectileModel model;
        public readonly GameObject go;
        public readonly GameObject body;
        public Transform transform { get { return this.go.transform; } }
        public GameVec2 position { get { return transform.position; } set { transform.position = new GameVec3(value.x, value.y, transform.position.z); } }
        public float angle { get { return transform.eulerAngles.z; } set { transform.eulerAngles = new GameVec3(0, 0, value); } }
        public GameVec2 scale { get { return transform.localScale; } set { transform.localScale = value; } }
        private GameEasing2DCom _posEaseCom;

        public GameProjectileFSMCom fsmCom { get; private set; }
        public GamePlayableCom animCom { get; private set; }
        public GameTimelineCom timelineCom { get; private set; }

        public GameProjectileEntityR(GameProjectileModel model, GameObject go) : base(model.typeId, GameEntityType.Projectile)
        {
            this.model = model;
            this.go = go;
            this.go.name = $"Projectile_{model.typeId}";
            this.body = go.transform.Find("Body").gameObject;

            this.fsmCom = new GameProjectileFSMCom();
            this.animCom = new GamePlayableCom(this.body.GetComponent<Animator>());
            this.timelineCom = new GameTimelineCom();
            this._posEaseCom = new GameEasing2DCom();
            this._posEaseCom.SetEase(0.05f, GameEasingType.Linear);
        }

        public override void Clear()
        {
            base.Clear();
            this.setActive(false);
        }

        public override void Dispose()
        {
            this.animCom.Dispose();
        }

        public override void Tick(float dt)
        {
            this.animCom.Tick(dt);
            this.timelineCom.Tick(dt);
            var pos = this._posEaseCom.Tick(this.position, this.transformCom.position, dt);
            this.position = pos;
            var forward = this.transformCom.forward;
            this.FaceTo(forward);
        }

        public void setActive(bool active)
        {
            this.go.SetActive(active);
        }

        public void FaceTo(in GameVec2 dir)
        {
            this.transform.eulerAngles = new GameVec3(0, 0, GameVec2.SignedAngle(GameVec2.right, dir));
        }

        public void SyncTrans()
        {
            this.position = this.transformCom.position;
            this.angle = this.transformCom.angle;
            this.scale = this.transformCom.scale;
            this.FaceTo(this.transformCom.forward);
        }
    }
}
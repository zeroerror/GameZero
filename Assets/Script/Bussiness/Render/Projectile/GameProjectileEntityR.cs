using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
using GameVec3 = UnityEngine.Vector3;

namespace GamePlay.Bussiness.Render
{
    public class GameProjectileEntityR : GameEntityBase
    {
        public readonly GameProjectileModelR model;
        public readonly GameObject root;
        public readonly GameParticlePlayCom particleCom;
        public Transform transform { get { return this.root.transform; } }
        public GameVec2 position { get { return transform.position; } set { transform.position = new GameVec3(value.x, value.y, transform.position.z); } }
        public override GameVec2 GetPosition() => this.position;
        public override void SetPosition(in GameVec2 pos)
        {
            base.SetPosition(pos);
            this.position = pos;
        }
        public float angle { get { return transform.eulerAngles.z; } set { transform.eulerAngles = new GameVec3(0, 0, value); } }
        public GameVec2 scale { get { return transform.localScale; } set { transform.localScale = value; } }
        private GameEasing2DCom _posEaseCom;

        public GameProjectileFSMCom fsmCom { get; private set; }
        public GameAnimPlayableCom animCom { get; private set; }
        public GameTimelineCom timelineCom { get; private set; }

        public GameProjectileEntityR(
            GameProjectileModelR model,
            GameObject go,
            Animator animator,
            ParticleSystem ps
        ) : base(model.typeId, GameEntityType.Projectile)
        {
            this.model = model;
            this.root = go;
            this.root.name = $"Projectile_{model.typeId}";
            if (ps != null)
            {
                this.particleCom = new GameParticlePlayCom(ps);
            }
            else if (animator != null)
            {
                this.animCom = new GameAnimPlayableCom(animator);
            }

            this.fsmCom = new GameProjectileFSMCom();
            this.timelineCom = new GameTimelineCom();
            this._posEaseCom = new GameEasing2DCom();
            this._posEaseCom.SetEase(GameTimeCollection.frameTime, GameEasingType.Linear);
        }

        public override void Clear()
        {
            base.Clear();
            this.setActive(false);
        }

        public override void Destroy()
        {
            this.animCom?.Destroy();
        }

        public override void Tick(float dt)
        {
            this.animCom?.Tick(dt);
            this.timelineCom.Tick(dt);
            this.particleCom?.Tick(dt);
            var pos = this._posEaseCom.Tick(this.position, this.transformCom.position, dt);
            this.position = pos;
            var forward = this.transformCom.forward;
            this.FaceTo(forward);
        }

        public void setActive(bool active)
        {
            this.root.SetActive(active);
            if (active && this.particleCom != null)
            {
                this.particleCom.Play(true);
            }
        }

        public void FaceTo(in GameVec2 dir)
        {
            this.transform.eulerAngles = new GameVec3(0, 0, GameVec2.SignedAngle(GameVec2.right, dir));
            this.transformCom.forward = dir;
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
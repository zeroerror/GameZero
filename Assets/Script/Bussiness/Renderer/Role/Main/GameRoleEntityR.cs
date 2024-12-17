using System.Collections.Specialized;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
using GameVec3 = UnityEngine.Vector3;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleEntityR : GameEntityBase
    {
        public readonly GameObject go;
        public readonly GameObject foot;
        public readonly GameObject shadow;
        public readonly GameObject body;
        public readonly GameBuffComR buffCom;

        public Transform transform { get { return this.go.transform; } }

        public GameVec2 position
        {
            get { return transform.position; }
            set
            {
                transform.position = new GameVec3(value.x, value.y, transform.position.z);
                if (this.shadow) this.shadow.transform.position = transform.position;
            }
        }

        public float angle { get { return transform.eulerAngles.z; } set { transform.eulerAngles = new GameVec3(0, 0, value); } }

        public GameVec2 scale { get { return transform.localScale; } set { transform.localScale = value; } }

        private GameEasing2DCom _posEaseCom;

        public GameRoleFSMComR fsmCom { get; private set; }
        public GameSkillComponentR skillCom { get; private set; }
        public GamePlayableCom animCom { get; private set; }
        public GameRoleAttributeBarCom attributeBarCom { get; private set; }

        public GameRoleEntityR(
            int typeId,
            GameObject go,
            GameObject foot,
            GameObject body
        ) : base(typeId, GameEntityType.Role)
        {
            this.go = go;
            this.foot = foot;
            this.shadow = foot.transform.Find("shadow")?.gameObject;
            Debug.Assert(this.shadow != null, "shadow is null");
            this.body = body;

            this.fsmCom = new GameRoleFSMComR();

            this.skillCom = new GameSkillComponentR(this);

            this.buffCom = new GameBuffComR();

            var animator = this.body.GetComponentInChildren<Animator>();
            this.animCom = new GamePlayableCom(animator);

            this._posEaseCom = new GameEasing2DCom();
            this._posEaseCom.SetEase(0.05f, GameEasingType.Linear);

            this.attributeBarCom = new GameRoleAttributeBarCom(this.transform);
        }

        public override void Clear()
        {
            base.Clear();
            this.setActive(false);
        }

        public override void Destroy()
        {
            this.animCom.Dispose();
        }

        public override void Tick(float dt)
        {
            this.animCom.Tick(dt);
            var pos = this._posEaseCom.Tick(this.position, this.transformCom.position, dt);
            this.position = pos;
            var forward = this.transformCom.forward;
            this.FaceTo(forward);
            this.attributeBarCom.Tick(dt);

            var hpRatio = this.attributeCom.GetValue(GameAttributeType.HP) / this.attributeCom.GetValue(GameAttributeType.MaxHP);
            this.attributeBarCom.hpSlider.SetRatio(hpRatio);

            var mpRatio = this.attributeCom.GetValue(GameAttributeType.MP) / this.attributeCom.GetValue(GameAttributeType.MaxMP);
            this.attributeBarCom.mpSlider.SetRatio(mpRatio);
        }

        public void setActive(bool active)
        {
            this.go.SetActive(active);
            this.shadow.SetActive(active);
        }

        public void FaceTo(in GameVec2 dir)
        {
            var scale = this.transform.lossyScale;
            var absx = Mathf.Abs(scale.x);
            scale.x = dir.x > 0 ? absx : -absx;
            this.transform.localScale = scale;
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
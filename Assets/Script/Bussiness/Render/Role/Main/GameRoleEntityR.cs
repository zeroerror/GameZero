using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using GamePlay.Infrastructure;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
using GameVec3 = UnityEngine.Vector3;
namespace GamePlay.Bussiness.Render
{
    public class GameRoleEntityR : GameEntityBase
    {
        public readonly GameRoleModelR model;
        public readonly GameRoleBodyCom bodyCom;

        public GameVec2 position
        {
            get { return transform.position; }
            set
            {
                transform.position = new GameVec3(value.x, value.y, transform.position.z);
                if (this.bodyCom.shadow) this.bodyCom.shadow.transform.position = transform.position;
            }
        }
        public override GameVec2 GetPosition() => this.position;
        public override void SetPosition(in GameVec2 pos)
        {
            base.SetPosition(pos);
            this.position = pos;
        }

        public float angle { get { return transform.eulerAngles.z; } set { transform.eulerAngles = new GameVec3(0, 0, value); } }
        public GameVec2 scale { get { return transform.localScale; } set { transform.localScale = value; } }

        private GameEasing2DCom _posEaseCom;
        public GameAnimPlayableCom animCom { get; private set; }
        public Transform transform { get { return this.bodyCom.tmRoot.transform; } }
        public GameRoleFSMComR fsmCom { get; private set; }
        public GameSkillComR skillCom { get; private set; }

        public GameRoleAttributeBarCom attributeBarCom { get; private set; }
        public readonly GameBuffComR buffCom;

        public GameRoleEntityR(
            GameRoleModelR model,
            GameRoleBodyCom bodyCom
        ) : base(model.typeId, GameEntityType.Role)
        {
            this.model = model;
            this.bodyCom = bodyCom;

            this.fsmCom = new GameRoleFSMComR();
            this.skillCom = new GameSkillComR(this);
            this.buffCom = new GameBuffComR();

            var animator = bodyCom.animator;
            this.animCom = new GameAnimPlayableCom(animator);

            this._posEaseCom = new GameEasing2DCom();
            this._posEaseCom.SetEase(0.05f, GameEasingType.Linear);

            this.attributeBarCom = new GameRoleAttributeBarCom(this);
        }

        public override void Clear()
        {
            base.Clear();
            this.setActive(false);
            this.skillCom.Clear();
        }

        public override void SetInvalid()
        {
            base.SetInvalid();
            this.skillCom.ForeachSkills((skill) =>
            {
                skill.SetInvalid();
            });
        }

        public override void Destroy()
        {
            this.animCom.Destroy();
        }

        public override void Tick(float dt)
        {
            // 动画
            this.animCom.Tick(dt);
            // 坐标
            var pos = this._posEaseCom.Tick(this.position, this.transformCom.position, dt);
            this.position = pos;
            // 朝向
            this._FaceToByScale(this.transformCom.scale);
            // 属性条
            this.attributeBarCom.Tick(this.attributeCom, dt);
        }

        public void setActive(bool active)
        {
            this.bodyCom.tmRoot.SetActive(active);
            this.bodyCom.shadow.SetActive(active);
            this.attributeBarCom.SetActive(active);
        }

        public void FaceToDir(in GameVec2 dir)
        {
            var scale = this.transform.lossyScale;
            var absx = Mathf.Abs(scale.x);
            scale.x = dir.x > 0 ? absx : -absx;
            this.transform.localScale = scale;
        }

        private void _FaceToByScale(in Vector2 scale)
        {
            var dir = scale.x > 0 ? Vector2.right : Vector2.left;
            this.FaceToDir(dir);
        }

        public void SyncTrans()
        {
            this.position = this.transformCom.position;
            this.angle = this.transformCom.angle;
            this.scale = this.transformCom.scale;
            this._FaceToByScale(this.transformCom.scale);
        }
    }
}
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
using GameVec3 = UnityEngine.Vector3;
namespace GamePlay.Bussiness.Renderer
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

        public float angle { get { return transform.eulerAngles.z; } set { transform.eulerAngles = new GameVec3(0, 0, value); } }
        public GameVec2 scale { get { return transform.localScale; } set { transform.localScale = value; } }

        private GameEasing2DCom _posEaseCom;
        public GamePlayableCom animCom { get; private set; }
        public Transform transform { get { return this.bodyCom.root.transform; } }
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

            var animator = bodyCom.body.GetComponentInChildren<Animator>();
            this.animCom = new GamePlayableCom(animator);

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

        public override void Destroy()
        {
            this.animCom.Destroy();
        }

        public override void Tick(float dt)
        {
            this.animCom.Tick(dt);
            var pos = this._posEaseCom.Tick(this.position, this.transformCom.position, dt);
            this.position = pos;
            this._FaceToByScale(this.transformCom.scale);
            this.attributeBarCom.Tick(dt);

            var hp = this.attributeCom.GetValue(GameAttributeType.HP);
            var maxHP = this.attributeCom.GetValue(GameAttributeType.MaxHP);
            var hpRatio = hp / maxHP;
            this.attributeBarCom.hpSlider.SetRatio(hpRatio);
            var hpStr = this.attributeCom.GetValueStr(GameAttributeType.HP);
            var maxHPStr = this.attributeCom.GetValueStr(GameAttributeType.MaxHP);
            this.attributeBarCom.hpSlider.SetText($"{hpStr}/{maxHPStr}");

            var mpRatio = this.attributeCom.GetValue(GameAttributeType.MP) / this.attributeCom.GetValue(GameAttributeType.MaxMP);
            this.attributeBarCom.mpSlider.SetRatio(mpRatio);
            var mpStr = this.attributeCom.GetValueStr(GameAttributeType.MP);
            var maxMPStr = this.attributeCom.GetValueStr(GameAttributeType.MaxMP);
            this.attributeBarCom.mpSlider.SetText($"{mpStr}/{maxMPStr}");

            var shieldRatio = this.attributeCom.GetValue(GameAttributeType.Shield) / this.attributeCom.GetValue(GameAttributeType.MaxHP);
            this.attributeBarCom.shieldSlider.SetRatio(shieldRatio);
            if (shieldRatio == 0)
            {
                this.attributeBarCom.shieldSlider.SetText("");
            }
            else
            {
                var shieldStr = this.attributeCom.GetValueStr(GameAttributeType.Shield);
                this.attributeBarCom.shieldSlider.SetText($"{shieldStr}");
            }
        }

        public void setActive(bool active)
        {
            this.bodyCom.root.SetActive(active);
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
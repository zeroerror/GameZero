using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
using GameVec3 = UnityEngine.Vector3;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleEntityR : GameEntityBase
    {
        /// <summary> 模型, 用于获取角色的属性、技能等信息. 如果正在变身, 则获取的是变身后的模型 </summary>
        public GameRoleModelR model => this.characterTransformCom.isTransforming ? this.characterTransformCom.model : this._originalModel;
        public GameRoleModelR originalModel => this._originalModel;
        private readonly GameRoleModelR _originalModel;

        /// <summary> 身体组件, 用于获取角色的根节点、身体节点、脚节点. 如果正在变身, 则获取的是变身后的身体组件 </summary>
        public GameRoleBodyCom bodyCom => this.characterTransformCom.isTransforming ? this.characterTransformCom.bodyCom : this._originalBodyCom;

        public GameRoleBodyCom originalBodyCom => this._originalBodyCom;
        private readonly GameRoleBodyCom _originalBodyCom;

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

        public GamePlayableCom animCom => this.characterTransformCom.isTransforming ? this.characterTransformCom.animCom : this._originalAnimCom;
        private GamePlayableCom _originalAnimCom;

        public Transform transform { get { return this.bodyCom.root.transform; } }
        public GameRoleFSMComR fsmCom { get; private set; }

        public GameSkillComR skillCom => this.characterTransformCom.isTransforming ? this.characterTransformCom.skillCom : this._skillCom;
        private GameSkillComR _skillCom;

        public GameRoleAttributeBarCom attributeBarCom { get; private set; }
        public readonly GameBuffComR buffCom;
        public GameRoleTransformComR characterTransformCom { get; private set; }

        public GameRoleEntityR(
            GameRoleModelR model,
            GameRoleBodyCom bodyCom
        ) : base(model.typeId, GameEntityType.Role)
        {
            this._originalModel = model;
            this._originalBodyCom = bodyCom;

            this.fsmCom = new GameRoleFSMComR();
            this._skillCom = new GameSkillComR(this);
            this.buffCom = new GameBuffComR();

            var animator = bodyCom.body.GetComponentInChildren<Animator>();
            this._originalAnimCom = new GamePlayableCom(animator);
            this.characterTransformCom = new GameRoleTransformComR(this, animator);

            this._posEaseCom = new GameEasing2DCom();
            this._posEaseCom.SetEase(0.05f, GameEasingType.Linear);

            this.attributeBarCom = new GameRoleAttributeBarCom(this);
        }

        public override void Clear()
        {
            base.Clear();
            this.setActive(false);
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
        }

        public void setActive(bool active)
        {
            this.bodyCom.root.SetActive(active);
            this.bodyCom.shadow.SetActive(active);
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
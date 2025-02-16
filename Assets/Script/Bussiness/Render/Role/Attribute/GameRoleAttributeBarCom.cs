using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public class GameRoleAttributeBarCom
    {
        private GameRoleEntityR _entity;

        public GameRoleAttributeSlider hpSlider { get; private set; }
        public GameRoleAttributeSlider mpSlider { get; private set; }
        public GameRoleAttributeSlider shieldSlider { get; private set; }

        public GameRoleAttributeBarCom(GameRoleEntityR entity)
        {
            this._entity = entity;
            this.hpSlider = new GameRoleAttributeSlider(1f, GameEasingType.Linear);
            this.mpSlider = new GameRoleAttributeSlider(0.0f, GameEasingType.Immediate);
            this.shieldSlider = new GameRoleAttributeSlider(0.0f, GameEasingType.Immediate);
        }

        /// <summary> 世界坐标转屏幕坐标接口 </summary>
        public WorldToScreenPointDelegate WorldToScreenPoint;
        public delegate Vector3 WorldToScreenPointDelegate(in Vector3 worldPos);
        private Vector3 _GetScreenPoint()
        {
            var worldPos = this._entity.position;
            var screenPoint = this.WorldToScreenPoint(worldPos);
            return screenPoint;
        }

        public void Tick(GameAttributeCom attributeCom, float dt)
        {
            if (!this.isActive) return;

            // 根据当前屏幕坐标设置每一个slider
            var screenPoint = this._GetScreenPoint();
            this.hpSlider.Tick(dt, screenPoint);
            this.mpSlider.Tick(dt, screenPoint);
            this.shieldSlider.Tick(dt, screenPoint);

            // 血条
            var hp = attributeCom.GetValue(GameAttributeType.HP);
            var maxHP = attributeCom.GetValue(GameAttributeType.MaxHP);
            var hpRatio = hp / maxHP;
            var hpSlider = this.hpSlider;
            hpSlider.SetRatio(hpRatio);
            // 设置血条分割线数量
            var division = GameRoleCollectionR.ROLE_ATTRIBUTE_SLIDER_DIVISION;
            var splitLineCount = hp / division;
            hpSlider.SetSlitLineCount(splitLineCount);
            // 蓝条
            var mpRatio = attributeCom.GetValue(GameAttributeType.MP) / attributeCom.GetValue(GameAttributeType.MaxMP);
            this.mpSlider.SetRatio(mpRatio);
            // 护盾条
            var shieldRatio = attributeCom.GetValue(GameAttributeType.Shield) / attributeCom.GetValue(GameAttributeType.MaxHP);
            var shieldSlider = this.shieldSlider;
            shieldSlider.SetRatio(shieldRatio);
            if (shieldRatio != 0)
            {
                // 设置护盾条分割线数量
                var shield = attributeCom.GetValue(GameAttributeType.Shield);
                var splitLineCountShield = shield / division;
                shieldSlider.SetSlitLineCount(splitLineCountShield);
            }
        }

        public void SetActive(bool active)
        {
            this.hpSlider.SetActive(active);
            this.mpSlider.SetActive(active);
            this.shieldSlider.SetActive(active);
            this.isActive = active;
        }
        public bool isActive { get; private set; }
    }
}
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleAttributeBarCom
    {
        private Transform _parent;

        public GameRoleAttributeSlider hpSlider { get; private set; }
        public GameRoleAttributeSlider mpSlider { get; private set; }

        public GameRoleAttributeBarCom(Transform parent)
        {
            this._parent = parent;
            this.hpSlider = new GameRoleAttributeSlider(1f, GameEasingType.Linear);
            this.mpSlider = new GameRoleAttributeSlider(0.0f, GameEasingType.Immediate);
        }

        /// <summary> 世界坐标转屏幕坐标接口 </summary>
        public WorldToScreenPointDelegate WorldToScreenPoint;
        public delegate Vector3 WorldToScreenPointDelegate(in Vector3 worldPos);
        private Vector3 _GetScreenPoint()
        {
            var worldPos = this._parent.position;
            var screenPoint = this.WorldToScreenPoint(worldPos);
            return screenPoint;
        }

        public void Tick(float dt)
        {
            var screenPoint = this._GetScreenPoint();
            this.hpSlider.Tick(dt, screenPoint);
            this.mpSlider.Tick(dt, screenPoint);
        }

        public void SetActive(bool active)
        {
            this.hpSlider.SetActive(active);
            this.mpSlider.SetActive(active);
        }
    }
}
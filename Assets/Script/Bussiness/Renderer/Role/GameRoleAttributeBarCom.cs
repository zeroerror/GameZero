using GamePlay.Core;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleAttributeBarCom
    {
        public Slider hpSlider { get; private set; }
        private Vector2 _offset;
        private RectTransform _hpRectTransform;
        private Transform _parent;
        private GameEasing2DCom _easingCom;

        public GameRoleAttributeBarCom(Transform parent)
        {
            this._parent = parent;
            this._easingCom = new GameEasing2DCom();
            this._easingCom.SetEase(0.2f, GameEasingType.Linear);
        }

        /// <summary> 相机提供的世界坐标转屏幕坐标的委托 </summary>
        public delegate Vector3 WorldToScreenPointDelegate(Vector3 worldPos);
        /// <summary> 世界坐标转屏幕坐标接口 </summary>
        public WorldToScreenPointDelegate WorldToScreenPoint;

        public void Tick(float dt)
        {
            // HP
            var worldPos = this._parent.position;
            worldPos.AddSelf(this._offset);
            var screenPoint = this.WorldToScreenPoint(worldPos);
            this._hpRectTransform.anchoredPosition = screenPoint;
        }

        public void SetHPSlider(Slider slider, in Vector2 offset)
        {
            this.hpSlider = slider;
            this._hpRectTransform = this.hpSlider.GetComponent<RectTransform>();
            this._offset = offset;
        }

        public void SetRatio(float ratio)
        {
            this.hpSlider.value = ratio;
        }

        public void SetActive(bool active)
        {
            this.hpSlider.gameObject.SetActive(active);
        }

        public void SetOffset(in Vector3 offset)
        {
            this.hpSlider.transform.localPosition = offset;
        }
    }
}
using GamePlay.Core;
using UnityEngine;
using UnityEngine.UI;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleAttributeSlider
    {
        public Slider slider { get; private set; }
        public Vector2 barOffset { get; private set; }
        public RectTransform rectTransform { get; private set; }
        public GameEasing1DCom easingCom { get; private set; }

        public GameRoleAttributeSlider(float duration, GameEasingType type)
        {
            this.easingCom = new GameEasing1DCom();
            this.easingCom.SetEase(duration, type);
        }

        public void SetActive(bool active)
        {
            if (!this.slider) return;
            this.slider.gameObject.SetActive(active);
        }

        /// <summary>
        /// 设置进度条
        /// <para>slider: 进度条</para>
        /// <para>offset: UI屏幕空间下的坐标偏移</para>
        /// </summary>
        public void SetSlider(Slider slider, in Vector2 offset)
        {
            this.slider = slider;
            this.rectTransform = this.slider.GetComponent<RectTransform>();
            this.barOffset = offset;
        }

        public void SetOffset(in Vector3 offset)
        {
            if (!this.slider) return;
            this.slider.transform.localPosition = offset;
        }

        public void SetRatio(float ratio)
        {
            if (!this.slider) return;
            this.slider.value = ratio;
        }

        public void SetAnchorPosition(Vector3 screenPoint)
        {
            if (!this.slider) return;
            this.rectTransform.anchoredPosition = screenPoint;
        }
    }
}
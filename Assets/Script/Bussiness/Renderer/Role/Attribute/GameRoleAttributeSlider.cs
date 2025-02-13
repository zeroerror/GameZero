using GamePlay.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace GamePlay.Bussiness.Render
{
    public class GameRoleAttributeSlider
    {
        public Slider slider { get; private set; }
        public RectTransform rectTransform { get; private set; }
        public Vector2 barOffset { get; private set; }

        public GameEasing1DCom easingCom { get; private set; }
        private Slider _easeSlider;
        public RectTransform _easeRectTransform { get; private set; }

        public GameRoleAttributeSlider(float duration, GameEasingType type)
        {
            this.easingCom = new GameEasing1DCom();
            this.easingCom.SetEase(duration, type);
        }

        public void Tick(float dt, in Vector2 screenPoint)
        {
            if (!this.slider) return;
            this.SetAnchorPosition(screenPoint.Add(this.barOffset));
            if (this._easeSlider) this._EaseSlider(dt);
        }

        private void _EaseSlider(float dt)
        {
            var easedValue = this.easingCom.Tick(this._easeSlider.value, this.slider.value, dt);
            this._easeSlider.value = easedValue;
        }

        public void SetActive(bool active)
        {
            if (!this.slider) return;
            this.slider.gameObject.SetActive(active);
        }

        public void SetSize(in Vector2 size)
        {
            if (!this.slider) return;
            this.rectTransform.sizeDelta = size;
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
            var fillRect = this.slider.fillRect;
            this._easeSlider = fillRect.parent.transform.GetComponentInChildren<Slider>();
            this._easeRectTransform = this._easeSlider?.GetComponent<RectTransform>();
            if (this._easeSlider) this._easeSlider.value = this.slider.value;
            if (!this._slitLineTransparentBg)
            {
                // 初始化一个透明背景
                var go = new GameObject("slitLineTransparentBg");
                this._slitLineTransparentBg = go.AddComponent<Image>();
                // 设置为和fillRect大小一样 且同一个父物体
                var rectTf = this._slitLineTransparentBg.rectTransform;
                rectTf.SetParent(fillRect.parent);
                rectTf.localScale = Vector3.one;
                go.SetPosZ(0);
                var fillWidth = fillRect.rect.width * (slider.maxValue / slider.value);
                var fillHeight = fillRect.rect.height;
                rectTf.sizeDelta = new Vector2(fillWidth, fillHeight);
                // 设置为unity默认的uisprite
                this._slitLineTransparentBg.sprite = Resources.Load<Sprite>("UI/Sprites/UISprite");
                this._slitLineTransparentBg.color = new Color(0, 0, 0, 0);
            }
        }
        private Image _slitLineTransparentBg;

        public void SetSlitLine(int count)
        {
            // 加载材质球
            var m = this.mat ?? Resources.Load<Material>("UI/Materials/SplitLine/mat_split_line");
            this.mat = m;
            this._slitLineTransparentBg.material = this.mat;
            this._slitLineTransparentBg.material.SetFloat("_LineCount", count);
        }
        private Material mat;

        public void SetOffset(in Vector3 offset)
        {
            if (!this.slider) return;
            this.slider.transform.localPosition = offset;
        }

        public void SetRatio(float ratio)
        {
            if (!this.slider) return;
            if (ratio > 1 || ratio < 0 || float.IsNaN(ratio))
            {
                return;
            }
            if (this.slider.value == ratio) return;
            this.slider.value = ratio;
        }

        public void SetAnchorPosition(Vector3 screenPoint)
        {
            if (!this.slider) return;
            this.rectTransform.anchoredPosition = screenPoint;
        }

        public void SetSiblingIndex(int i)
        {
            if (!this.slider) return;
            rectTransform.SetSiblingIndex(i);
        }

        public void SetText(string text)
        {
            if (!this.slider) return;
            var textGO = this.slider.fillRect.parent.Find("text");
            var textComp = textGO?.GetComponent<TextMeshProUGUI>();
            if (!textComp) return;
            textComp.text = text;
        }
    }
}
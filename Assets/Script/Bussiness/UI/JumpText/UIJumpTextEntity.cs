using GamePlay.Core;
using GamePlay.Infrastructure;
using TMPro;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public class UIJumpTextEntity
    {
        public void SetAnchorPosition(Vector3 screenPoint)
        {
            this.rootNode.GetComponentInChildren<RectTransform>().anchoredPosition = screenPoint;
        }

        public string text
        {
            get { return this._text.text; }
            set { this._text.text = value; }
        }

        private TextMeshProUGUI _text;
        public GameObject rootNode { get; private set; }
        public string prefabName { get; private set; }
        public GameAnimPlayableCom animCom { get; private set; }

        public UIJumpTextEntity(GameObject rootNode, string prefabName)
        {
            this.rootNode = rootNode;
            this.prefabName = prefabName;
            this._text = this.rootNode.GetComponentInChildren<TextMeshProUGUI>();
            var animator = this.rootNode.GetComponentInChildren<Animator>();
            this.animCom = new GameAnimPlayableCom(animator);
        }

        public void Tick(float dt)
        {
            this.animCom.Tick(dt);
        }

        public void Destroy()
        {
            this.animCom.Destroy();
        }

        public void SetActive(bool active)
        {
            this.rootNode.SetActive(active);
        }

        public void SetScale(float scale)
        {
            this.rootNode.transform.localScale = Vector3.one * scale;
        }
    }
}
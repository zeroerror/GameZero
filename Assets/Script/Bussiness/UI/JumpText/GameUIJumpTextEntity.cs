using GamePlay.Core;
using TMPro;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public class GameUIJumpTextEntity
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
        public GamePlayableCom playCom { get; private set; }

        public GameUIJumpTextEntity(GameObject rootNode, string prefabName)
        {
            this.rootNode = rootNode;
            this.prefabName = prefabName;
            this._text = this.rootNode.GetComponentInChildren<TextMeshProUGUI>();
            var animator = this.rootNode.GetComponentInChildren<Animator>();
            this.playCom = new GamePlayableCom(animator);
        }

        public void SetActive(bool active)
        {
            this.rootNode.SetActive(active);
        }

        public void Tick(float dt)
        {
            this.playCom.Tick(dt);
        }

        public void Destroy()
        {
            this.playCom.Destroy();
        }
    }
}
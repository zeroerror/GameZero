using GamePlay.Core;
using TMPro;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public class UIJumpTextEntity
    {
        public void SetAnchorPosition(Vector3 screenPoint)
        {
            this.rootGO.GetComponentInChildren<RectTransform>().anchoredPosition = screenPoint;
        }

        public string text
        {
            get { return this._text.text; }
            set { this._text.text = value; }
        }

        private TextMeshProUGUI _text;
        public GameObject rootGO { get; private set; }
        public string prefabName { get; private set; }
        public GameAnimPlayableCom animCom { get; private set; }

        public UIJumpTextEntity(GameObject rootGO, GameObject bodyGO, string prefabName)
        {
            this.rootGO = rootGO;
            this.prefabName = prefabName;
            this._text = bodyGO.GetComponentInChildren<TextMeshProUGUI>();

            if (bodyGO.TryGetComponent<Animator>(out var animator)) GameObject.DestroyImmediate(animator);
            animator = bodyGO.AddComponent<Animator>();
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
            this.rootGO.SetActive(active);
        }

        public void SetScale(float scale)
        {
            this.rootGO.transform.localScale = Vector3.one * scale;
        }
    }
}
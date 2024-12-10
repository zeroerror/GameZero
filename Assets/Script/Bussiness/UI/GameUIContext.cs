using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public class GameUIContext
    {
        public GameObject uiRoot { get; private set; }
        public Camera uiCamera { get; private set; }

        public GameUIContext(GameObject uiRoot)
        {
            this.uiRoot = uiRoot;
            this.uiCamera = GameObject.Find("UICamera")?.GetComponent<Camera>();
            Debug.Assert(this.uiCamera != null, "UICamera not found");
        }

        public Vector3 WorldToScreenPoint(Vector3 v)
        {
            var pos = RectTransformUtility.WorldToScreenPoint(this.uiCamera, v);
            // 由于canvas的锚点是中心，这里的pos是以左下角为原点的坐标，需要转换为中心为原点的坐标
            pos.x -= Screen.width / 2;
            pos.y -= Screen.height / 2;
            return pos;
        }

        public void AddToUIRoot(Transform transform)
        {
            transform.SetParent(this.uiRoot.transform, false);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
        }
    }
}
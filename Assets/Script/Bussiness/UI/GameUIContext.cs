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

        public void AddToUIRoot(Transform transform)
        {
            transform.SetParent(this.uiRoot.transform, false);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
        }
    }
}
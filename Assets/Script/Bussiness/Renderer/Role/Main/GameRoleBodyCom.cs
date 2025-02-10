using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleBodyCom
    {
        public readonly GameObject tmRoot;
        public readonly GameObject tmFoot;
        public readonly GameObject prefabGO;
        public readonly GameObject shadow;
        public GameRoleAttachmentCom attachmentCom { get; private set; }

        public GameRoleBodyCom(GameObject tmRoot, GameObject tmFoot, GameObject prefabGO, GameRoleAttachmentCom attachmentCom)
        {
            this.tmRoot = tmRoot;
            this.tmFoot = tmFoot;
            this.prefabGO = prefabGO;
            this.shadow = tmFoot.transform.Find("shadow")?.gameObject;
            Debug.Assert(this.shadow != null, "shadow is null");
            this.attachmentCom = attachmentCom;
        }

        public void Destroy()
        {
            GameObject.Destroy(this.tmRoot);
        }

        public void SetActive(bool active)
        {
            this.tmRoot.SetActive(active);
            this.shadow.SetActive(active);
        }
    }

}
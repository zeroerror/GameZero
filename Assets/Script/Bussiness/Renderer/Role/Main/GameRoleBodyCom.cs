using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleBodyCom
    {
        public readonly GameObject root;
        public readonly GameObject foot;
        public readonly GameObject body;
        public readonly GameObject shadow;
        public GameRoleAttachmentCom attachmentCom { get; private set; }

        public GameRoleBodyCom(GameObject root, GameObject foot, GameObject body, GameRoleAttachmentCom attachmentCom)
        {
            this.root = root;
            this.foot = foot;
            this.body = body;
            this.shadow = foot.transform.Find("shadow")?.gameObject;
            Debug.Assert(this.shadow != null, "shadow is null");
            this.attachmentCom = attachmentCom;
        }

        public void Destroy()
        {
            GameObject.Destroy(this.root);
        }

        public void SetActive(bool active)
        {
            this.root.SetActive(active);
            this.shadow.SetActive(active);
        }
    }
}
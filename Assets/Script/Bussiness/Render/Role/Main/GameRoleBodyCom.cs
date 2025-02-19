using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public class GameRoleBodyCom
    {
        public readonly GameObject tmRoot;
        public readonly GameObject tmFoot;
        public readonly GameObject prefabGO;
        public readonly GameObject shadow;
        public GameRoleAttachmentCom attachmentCom { get; private set; }
        public Renderer[] renderers { get; private set; }

        public readonly Animator animator;

        public GameRoleBodyCom(GameObject tmRoot, GameObject tmFoot, GameObject prefabBody, GameRoleAttachmentCom attachmentCom)
        {
            if (prefabBody.TryGetComponent<Animator>(out var animator)) GameObject.DestroyImmediate(animator);
            animator = prefabBody.AddComponent<Animator>();
            this.animator = animator;
            if (prefabBody.TryGetComponent<SpriteRenderer>(out var spriteRenderer)) GameObject.DestroyImmediate(spriteRenderer);
            prefabBody.AddComponent<SpriteRenderer>();

            this.tmRoot = tmRoot;
            this.tmFoot = tmFoot;
            this.prefabGO = prefabBody;
            this.shadow = tmFoot.transform.Find("shadow")?.gameObject;
            Debug.Assert(this.shadow != null, "shadow is null");
            this.attachmentCom = attachmentCom;
            this.renderers = this.prefabGO.GetComponentsInChildren<Renderer>();
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
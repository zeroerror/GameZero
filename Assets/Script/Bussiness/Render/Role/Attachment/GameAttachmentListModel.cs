using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    /// <summary>
    /// 附件列表
    /// </summary>
    [System.Serializable]
    public struct GameAttachmentListModel
    {
        [Header("武器 - 弓箭 - 把手")]
        public GameObject bow_handle;
        [Header("武器 - 弓箭 - 弓臂上部分")]
        public GameObject bow_limb_u;
        [Header("武器 - 弓箭 - 弓臂下部分")]
        public GameObject bow_limb_l;
        [Header("武器 - 弓箭 - 发射点")]
        public GameObject bow_shoot_point;

        public void SetSprite_Visible(bool isVisible)
        {
            if (this.bow_handle) this.bow_handle.GetComponent<SpriteRenderer>().enabled = isVisible;
            if (this.bow_limb_u) this.bow_limb_u.GetComponent<SpriteRenderer>().enabled = isVisible;
            if (this.bow_limb_l) this.bow_limb_l.GetComponent<SpriteRenderer>().enabled = isVisible;
        }
    }
}
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleAttachmentCom : MonoBehaviour
    {
        [Header("左侧附件")]
        public GameAttachmentListModel attachmentList_left;
        [Header("右侧附件")]
        public GameAttachmentListModel attachmentList_right;

        public void SetAttachmentSprite_Bow(Sprite handleSprite, Sprite limbSprite)
        {
            this._SetAttachmentSprite(this.attachmentList_left, handleSprite, limbSprite);
            this._SetAttachmentSprite(this.attachmentList_right, handleSprite, limbSprite);
        }

        private void _SetAttachmentSprite(GameAttachmentListModel list, Sprite handleSprite, Sprite limbSprite)
        {
            var bowHandle_sr = list.bow_handle.GetComponent<SpriteRenderer>();
            bowHandle_sr.sprite = handleSprite;

            var bowLimb_u_sr = list.bow_limb_u.GetComponent<SpriteRenderer>();
            bowLimb_u_sr.sprite = limbSprite;

            var bowLimb_l_sr = list.bow_limb_l.GetComponent<SpriteRenderer>();
            bowLimb_l_sr.sprite = limbSprite;
        }

        public void SetAttachmentSprite_Visible(GameRoleAttachmentDirectionType dirType, bool isVisible)
        {
            switch (dirType)
            {
                case GameRoleAttachmentDirectionType.Left:
                    this.attachmentList_left.SetSprite_Visible(isVisible);
                    break;
                case GameRoleAttachmentDirectionType.Right:
                    this.attachmentList_right.SetSprite_Visible(isVisible);
                    break;
                default:
                    GameLogger.LogError($"设置附件图片可见性失败, 未知的方向类型: {dirType}");
                    break;
            }
        }
    }
}
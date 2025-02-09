using System.Collections.Generic;
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using GamePlay.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Bussiness.UI
{
    public class UIPanelCom
    {
        private UIPanelComBinder _binder;
        private GameEntityBase _entity;

        public UIPanelCom(UIPanelComBinder binder, GameEntityBase entity)
        {
            this._binder = binder;
            this._entity = entity;

            string name = "未知";
            string headUrl = UIPathUtil.GetRoleHead(0);
            if (this._entity is GameRoleEntityR role)
            {
                name = role.model.roleName;
                headUrl = UIPathUtil.GetRoleHead(role.model.typeId);
            }
            // 头像
            this._binder.img_head.GetComponent<Image>().sprite = Resources.Load<Sprite>(headUrl);
            // 名称
            this._binder.txt_name.GetComponent<Text>().text = name;
        }

        public void Refersh(GameAttribute[] attributes, GameAttribute[] baseAttributes)
        {
            attributes?.Remove(attr => attr.type == GameAttributeType.MaxHP || attr.type == GameAttributeType.MaxMP);
            attributes?.Sort((a, b) => a.type.CompareTo(b.type));
            baseAttributes?.Remove(attr => attr.type == GameAttributeType.MaxHP || attr.type == GameAttributeType.MaxMP);
            baseAttributes?.Sort((a, b) => a.type.CompareTo(b.type));
            // 属性列表
            attributes?.Foreach((attr, idx) =>
            {
                var fieldName = "attributeList_Viewport_Content_item" + (idx + 1);
                var item = this._binder.GetField(fieldName) as UIAttributeItemBinder;
                if (item == null)
                {
                    GameLogger.LogError("UIPanelCom: 属性列表项未找到 " + fieldName);
                    return;
                }
                item.gameObject.SetActive(true);
                // 属性图标
                item.group_img_icon.GetComponent<Image>().sprite = Resources.Load<Sprite>(UIPathUtil.GetAttributeIcon(attr.type));
                // 属性值
                item.group_txt_value.GetComponent<TextMeshProUGUI>().text = attr.value.ToString();
                // 加成
                var baseAttr = baseAttributes.Find(b => b.type == attr.type);
                var additionV = baseAttr.type != GameAttributeType.None ? (attr.value - baseAttr.value) : 0;
                var textCom = item.group_txt_addition.GetComponent<TextMeshProUGUI>();
                if (additionV == 0)
                {
                    textCom.text = string.Empty;
                }
                else
                {
                    textCom.text = $"<color=#23FF00>(+{additionV.GetFormatNumStr()})</color>\n<color=#ffffff>{baseAttr.value.GetFormatNumStr()}</color>";
                }
                // <color=#23FF00>(+166)</color>\n<color=#ffffff>500</color>
            });
            var count = attributes?.Length ?? 0;
            for (int i = count; i < 7; i++)
            {
                var fieldName = "attributeList_Viewport_Content_item" + (i + 1);
                var item = this._binder.GetField(fieldName) as UIAttributeItemBinder;
                if (item == null)
                {
                    GameLogger.LogError("UIPanelCom: 属性列表项未找到 " + fieldName);
                    return;
                }
                item.gameObject.SetActive(false);
            }
        }
    }
}
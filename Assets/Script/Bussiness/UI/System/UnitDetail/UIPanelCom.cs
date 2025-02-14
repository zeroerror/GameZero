using System.Collections.Generic;
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Render;
using GamePlay.Core;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Bussiness.UI
{
    public class UIPanelCom
    {
        private UIPanelComBinder _binder;
        private GameDomainApi _logicApi => this._uiApi.logicApi;
        private GameDomainApiR _rendererApi => this._uiApi.rendererApi;
        private UIDomainApi _uiApi;

        public UIPanelCom(UIPanelComBinder binder, UIDomainApi uiApi)
        {
            this._binder = binder;
            this._uiApi = uiApi;
        }

        public void RefreshHead(GameEntityType entityType, int entityId)
        {
            var entity = this._rendererApi.directorApi.FindEntity(entityType, entityId);
            string name = "未知";
            string headUrl = "";
            if (entity is GameRoleEntityR role)
            {
                var model = role.model;
                name = model.roleName;
                headUrl = UIPathUtil.GetRoleHead(model.typeId);
            }
            // 头像
            this._binder.img_head.sprite = Resources.Load<Sprite>(headUrl);
            // 名称
            this._binder.txt_name.text = name;
        }

        public void Refersh(GameEntityType entityType, int entityId)
        {
            var entityl = this._logicApi.directorApi.FindEntity(entityType, entityId);
            var attributes = entityl?.attributeCom.ToArgs().attributes;
            var baseAttributes = entityl?.baseAttributeCom.ToArgs().attributes;

            // 用于展示的属性列表
            var attributes_display = attributes?.Remove(attr => attr.type == GameAttributeType.MaxHP || attr.type == GameAttributeType.MaxMP);
            attributes_display?.Sort((a, b) => a.type.CompareTo(b.type));
            attributes_display?.Foreach((attr, idx) =>
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
                item.group_img_icon.sprite = Resources.Load<Sprite>(UIPathUtil.GetAttributeIcon(attr.type));
                // 属性值
                if (attr.type == GameAttributeType.HP || attr.type == GameAttributeType.MP)
                {
                    var valueStr = attr.value.GetFormatNumStr();
                    var maxValue = attributes.Find(a => a.type == attr.type + 1).value;
                    var maxValueStr = maxValue != 0 ? maxValue.GetFormatNumStr() : valueStr;
                    item.group_txt_value.text = $"{valueStr}/{maxValueStr}".ToDevStr();
                }
                else
                {
                    item.group_txt_value.text = attr.value.ToString();
                }
                // 加成
                var baseAttr = baseAttributes.Find(b => b.type == attr.type);
                var additionV = baseAttr.type != GameAttributeType.None ? (attr.value - baseAttr.value) : 0;
                var textCom = item.group_txt_addition;
                if (additionV == 0)
                {
                    textCom.text = string.Empty;
                }
                else
                {
                    var additionVStr = additionV.GetFormatNumStr();
                    var baseAttrStr = baseAttr.value.GetFormatNumStr();
                    textCom.text = $"<color=#23FF00>(+{additionVStr})</color>\n<color=#ffffff>{baseAttrStr}</color>".ToDevStr();
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
            // 技能列表
            if (entityl is GameRoleEntity role)
            {
                this._binder.skillGroup.gameObject.SetActive(true);
                role.skillCom.ForeachSkills((skill, idx) =>
                {
                    var skillItem = this._binder.GetField("skillGroup_item" + (idx + 1)) as UISkillItemBinder;
                    skillItem.gameObject.SetActive(true);
                    // CD
                    var cdTime = skill.skillModel.conditionModel.cdTime;
                    var cdElapsed = skill.cdElapsed;
                    var textCom = skillItem.cd_txt;
                    textCom.text = cdElapsed.ToFixed(1).ToString();
                    skillItem.cd.gameObject.SetActive(cdElapsed > 0);
                    // 技能图标
                    var url = UIPathUtil.GetSkillIcon(skill.skillModel.typeId);
                    skillItem.img_icon.sprite = Resources.Load<Sprite>(url);
                });
                for (int i = role.skillCom.Count; i < 3; i++)
                {
                    var skillItem = this._binder.GetField("skillGroup_item" + (i + 1)) as UISkillItemBinder;
                    skillItem.gameObject.SetActive(false);
                }
            }
            else
            {
                this._binder.skillGroup.gameObject.SetActive(false);
            }
        }
    }
}
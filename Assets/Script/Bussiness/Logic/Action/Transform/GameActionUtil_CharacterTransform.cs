using System.Collections.Generic;
using GamePlay.Core;
using UnityEngine.SocialPlatforms.Impl;

namespace GamePlay.Bussiness.Logic
{
    public static class GameActionUtil_CharacterTransform
    {
        /// <summary>
        /// 计算变身
        /// <para name="actor"> 行为者 </para>
        /// <para name="target"> 目标 </para>
        /// <para name="transformModel"> 变身模型 </para>
        /// </summary>
        /// <returns></returns>
        public static GameActionRecord_CharacterTransform CalcCharacterTransform(GameEntityBase actor, GameEntityBase target, GameActionModel_CharacterTransform transformModel)
        {
            var attributeList = transformModel.attributeList;
            if (attributeList == null || attributeList.Length == 0)
            {
                return default;
            }

            var record = new GameActionRecord_CharacterTransform();
            record.transAttributes = new GameAttribute[attributeList.Length];
            attributeList.Foreach((attr, index) =>
            {
                var modifyRecord = GameActionUtil_AttributeModify.CalcAttributeModify(
                    actor,
                    target,
                    attr.attributeType,
                    attr.value,
                    attr.valueFormat,
                    attr.refType
                );
                record.transAttributes[index] = new GameAttribute(modifyRecord.modifyType, modifyRecord.modifyValue);
            });
            return record;
        }

        /// <summary>
        /// 执行变身
        /// <para>oldRole 变身前角色</para>
        /// <para>newRole 变身后角色</para>
        /// </summary>
        public static void DoCharacterTransform(GameEntityBase oldRole, GameEntityBase newRole, in GameActionRecord_CharacterTransform record)
        {
            GameLogger.Log($"目标:{oldRole.idCom} 变身 => {newRole.idCom}");
            var oldAttrCom = oldRole.attributeCom;
            var newAttrCom = newRole.attributeCom;
            newAttrCom.CopyFrom(oldAttrCom);

            var transAttributes = record.transAttributes;
            if (transAttributes != null && transAttributes.HasData())
            {
                transAttributes?.Foreach(attr =>
                {
                    newAttrCom.SetAttribute(attr);
                    GameLogger.Log($"特殊属性变化: {attr}");
                });
            }
        }
    }
}
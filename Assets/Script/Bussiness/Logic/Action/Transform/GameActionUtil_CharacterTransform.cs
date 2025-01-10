using System.Collections.Generic;
using GamePlay.Core;

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
        /// <para name="target"> 目标 </para>
        /// <para name="record"> 属性记录 </para>
        /// </summary>
        public static void DoCharacterTransform(GameEntityBase target, GameActionRecord_CharacterTransform record)
        {
            var transAttributes = record.transAttributes;
            if (transAttributes == null || transAttributes.Length == 0)
            {
                return;
            }
            GameLogger.Log($"目标:{target.idCom} 变身");
            transAttributes?.Foreach(attr =>
            {
                var targetAttrCom = target.attributeCom;
                var curValue = targetAttrCom.GetValue(attr.type);
                var afterValue = curValue + attr.value;
                targetAttrCom.SetAttribute(attr.type, afterValue);
                GameLogger.Log($"属性:{attr.type} 当前值:{curValue} 变身后:{afterValue}");
            });
        }
    }
}
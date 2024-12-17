using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public static class GameActionUtil_AttributeModify
    {
        /// <summary>
        /// 计算属性修改
        /// </summary>
        /// <para name="actor"> 行为者 </para>
        /// <para name="target"> 目标 </para>
        /// <para name="modifyModel"> 属性修改模型 </para>
        public static GameActionRecord_AttributeModify CalcAttributeModify(GameEntityBase actor, GameEntityBase target, GameActionModel_Attribute modifyModel)
        {
            // 数值格式化
            var modelValue = modifyModel.valueFormat.FormatValue(modifyModel.value);
            // 参考属性值
            float refAttrValue = modifyModel.refType.GetRefAttributeValue(actor, target);

            // 属性修改数值 增幅/减幅
            var modifyValue = refAttrValue * modelValue;

            // 属性修改结算
            var targetAttrCom = target.attributeCom;
            var modifyType = modifyModel.modifyType;
            var curValue = targetAttrCom.GetValue(modifyType);
            var modifiedValue = curValue - modifyValue;
            var realAttributeModify = modifiedValue < 0 ? curValue : modifyValue;

            // 最大值限制
            if (modifyType == GameAttributeType.HP)
            {
                var maxHP = targetAttrCom.GetValue(GameAttributeType.MaxHP);
                if (modifiedValue > maxHP) realAttributeModify = maxHP - curValue;
            }
            if (modifyType == GameAttributeType.MP)
            {
                var maxMP = targetAttrCom.GetValue(GameAttributeType.MaxHP);
                if (modifiedValue > maxMP) realAttributeModify = maxMP - curValue;
            }

            var record = new GameActionRecord_AttributeModify(
                actor.TryGetLinkEntity<GameEntityBase>()?.idCom.ToArgs() ?? default,
                actor.idCom.ToArgs(),
                target.idCom.ToArgs(),
                modifyModel.modifyType,
                realAttributeModify
            );
            return record;
        }

        public static void DoAttributeModify(GameEntityBase target, GameActionRecord_AttributeModify record)
        {
            var targetAttrCom = target.attributeCom;
            var curValue = targetAttrCom.GetValue(record.modifyType);
            if (curValue <= 0)
            {
                return;
            }

            var afterAttributeModifyHP = curValue + record.modifyValue;
            targetAttrCom.SetAttribute(GameAttributeType.HP, afterAttributeModifyHP);
            GameLogger.Log($"目标:{target.idCom} 受到属性修改:{record.modifyValue}");
        }
    }
}
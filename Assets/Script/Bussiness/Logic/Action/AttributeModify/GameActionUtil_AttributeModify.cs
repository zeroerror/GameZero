using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public static class GameActionUtil_AttributeModify
    {
        /// <summary>
        /// 计算属性修改
        /// <para name="actor"> 行为者 </para>
        /// <para name="target"> 目标 </para>
        /// <para name="modifyType"> 修改类型 </para>
        /// <para name="value"> 修改数值 </para>
        /// <para name="valueFormat"> 数值格式化 </para>
        /// <para name="refType"> 参考属性值类型 </para>
        /// </summary>
        public static GameActionRecord_AttributeModify CalcAttributeModify(
            GameEntityBase actor,
            GameEntityBase target,
            GameAttributeType modifyType,
            int value,
            GameActionValueFormat valueFormat,
            GameActionValueRefType refType,
            int actionId = 0
        )
        {
            // 数值格式化
            var modelValue = valueFormat.FormatValue(value);
            // 参考属性值
            float refAttrValue = refType.GetRefAttributeValue(actor, target);

            // 属性修改数值 增幅/减幅
            var modifyValue = refAttrValue * modelValue;

            // 属性修改结算
            var targetAttrCom = target.attributeCom;
            var curValue = targetAttrCom.GetValue(modifyType);
            var modifiedValue = curValue + modifyValue;
            var realAttributeModify = modifiedValue < 0 ? curValue : modifyValue;

            // 最大值限制
            if (modifyType == GameAttributeType.HP)
            {
                var maxHP = targetAttrCom.GetValue(GameAttributeType.MaxHP);
                if (modifiedValue > maxHP) realAttributeModify = maxHP - curValue;
                else if (modifiedValue < 0) realAttributeModify = -curValue;
            }
            if (modifyType == GameAttributeType.MP)
            {
                var maxMP = targetAttrCom.GetValue(GameAttributeType.MaxMP);
                if (modifiedValue > maxMP) realAttributeModify = maxMP - curValue;
                else if (modifiedValue < 0) realAttributeModify = -curValue;
            }

            // 取2位小数
            realAttributeModify = GameMathF.ToFixed(realAttributeModify, 2);

            var record = new GameActionRecord_AttributeModify(
                actionId,
                actor.GetLinkParent<GameRoleEntity>()?.idCom.ToArgs() ?? default,
                actor.idCom.ToArgs(),
                target.idCom.ToArgs(),
                actor.actionTargeterCom.getCurTargeterAsRecord(),
                modifyType,
                realAttributeModify
            );
            return record;
        }

        /// <summary>
        /// 计算属性修改
        /// <para name="actor"> 行为者 </para>
        /// <para name="target"> 目标 </para>
        /// <para name="modifyModel"> 属性修改模型 </para>
        /// </summary>
        public static GameActionRecord_AttributeModify CalcAttributeModify(GameEntityBase actor, GameEntityBase target, GameActionModel_AttributeModify modifyModel)
        {
            var modifyType = modifyModel.modifyType;
            var value = modifyModel.value;
            var valueFormat = modifyModel.valueFormat;
            var refType = modifyModel.refType;
            return CalcAttributeModify(actor, target, modifyType, value, valueFormat, refType, modifyModel.typeId);
        }

        /// <summary>
        /// 计算属性修改
        /// <para name="actor"> 行为者 </para>
        /// <para name="target"> 目标 </para>
        /// <para name="buffAttributeModel"> buff属性模型 </para>
        /// </summary>
        /// <returns></returns>
        public static GameActionRecord_AttributeModify CalcAttributeModify(GameEntityBase actor, GameEntityBase target, GameBuffAttributeModel buffAttributeModel)
        {
            var modifyType = buffAttributeModel.modifyType;
            var value = buffAttributeModel.value;
            var valueFormat = buffAttributeModel.valueFormat;
            var refType = buffAttributeModel.refType;
            return CalcAttributeModify(actor, target, modifyType, value, valueFormat, refType);
        }

        /// <summary>
        /// 执行属性修改
        /// <para name="target"> 目标 </para>
        /// <para name="record"> 属性修改记录 </para>
        /// </summary>
        public static void DoAttributeModify(GameEntityBase target, GameActionRecord_AttributeModify record)
        {
            var targetAttrCom = target.attributeCom;
            var curValue = targetAttrCom.GetValue(record.modifyType);
            var afterAttributeModify = curValue + record.modifyValue;
            targetAttrCom.SetAttribute(record.modifyType, afterAttributeModify);
            GameLogger.Log($"目标:{target.idCom} 受到属性修改{record.modifyValue} ({curValue}=>{afterAttributeModify})");
            if (record.modifyType == GameAttributeType.AttackSpeed)
            {
                GameLogger.DebugLog($"Buff属性效果: 攻速变化 {record.modifyValue} ({curValue}=>{afterAttributeModify})");
            }
        }

    }
}
using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public static class GameActionHealUtil
    {
        /// <summary>
        /// 计算治疗
        /// </summary>
        /// <para name="actor"> 行为者 </para>
        /// <para name="target"> 目标 </para>
        /// <para name="healModel"> 治疗模型 </para>
        public static GameActionRecord_Heal CalcHeal(GameEntityBase actor, GameEntityBase target, GameActionModel_Heal healModel)
        {
            // 数值格式化
            var modelValue = healModel.valueFormat.FormatValue(healModel.value);
            // 参考属性值
            float refAttrValue = healModel.refType.GetRefAttributeValue(actor, target);

            // 治疗数值 增幅/减幅
            var healValue = refAttrValue * modelValue;
            switch (healModel.healType)
            {
                case GameActionHealType.Real:
                    break;
                default:
                    GameLogger.LogError("未处理的治疗类型");
                    break;
            }

            // 治疗结算
            var curHP = target.attributeCom.GetValue(GameAttributeType.HP);
            var afterHP = curHP + healValue;
            var maxHP = target.attributeCom.GetValue(GameAttributeType.MaxHP);
            var realHeal = afterHP > maxHP ? maxHP - curHP : healValue;
            var actorRoleIdArgs = actor.TryGetLinkEntity<GameEntityBase>()?.idCom.ToArgs() ?? default;
            var record = new GameActionRecord_Heal(
                actorRoleIdArgs,
                actor.idCom.ToArgs(),
                target.idCom.ToArgs(),
                GameActionHealType.Real,
                realHeal
            );
            return record;
        }

        public static void DoHeal(GameEntityBase target, GameActionRecord_Heal record)
        {
            var targetAttrCom = target.attributeCom;
            var curHP = targetAttrCom.GetValue(GameAttributeType.HP);
            if (curHP <= 0)
            {
                return;
            }

            var afterHealHP = curHP + record.value;
            targetAttrCom.SetAttribute(GameAttributeType.HP, afterHealHP);
            GameLogger.Log($"目标:{target.idCom} 受到治疗:{record.value}");
        }
    }
}
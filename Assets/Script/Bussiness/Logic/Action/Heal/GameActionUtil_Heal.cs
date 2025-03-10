using GamePlay.Core;
using GamePlay.Infrastructure;

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

            var healValue = refAttrValue * modelValue;

            // 增疗
            var healAddition = actor.attributeCom.GetValue(GameAttributeType.HealAddition);
            healValue = healValue * (1 + healAddition);

            // 治疗结算
            var curHP = target.attributeCom.GetValue(GameAttributeType.HP);
            var afterHP = curHP + healValue;
            var maxHP = target.attributeCom.GetValue(GameAttributeType.MaxHP);
            var realHeal = afterHP > maxHP ? maxHP - curHP : healValue;

            // 取2位小数
            realHeal = GameMathF.GetFixed(realHeal, 2);

            var actorRoleIdArgs = actor.GetLinkParent<GameRoleEntity>()?.idCom.ToArgs() ?? default;
            var record = new GameActionRecord_Heal(
                healModel.typeId,
                actorRoleIdArgs,
                actor.idCom.ToArgs(),
                target.idCom.ToArgs(),
                actor.actionTargeterCom.getCurTargeterAsRecord(),
                GameActionHealType.Normal,
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
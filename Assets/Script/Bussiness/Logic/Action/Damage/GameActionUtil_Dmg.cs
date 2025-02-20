using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.Logic
{
    public static class GameActionUtil_Dmg
    {
        /// <summary>
        /// 计算伤害
        /// </summary>
        /// <para name="actor"> 行为者 </para>
        /// <para name="target"> 目标 </para>
        /// <para name="dmgModel"> 伤害模型 </para>
        public static GameActionRecord_Dmg CalcDmg(GameEntityBase actor, GameEntityBase target, GameActionModel_Dmg dmgModel)
        {
            // 数值格式化
            var modelValue = (float)dmgModel.value;
            var randomOffset = GameRandomService.GetRandom(dmgModel.randomValueOffset);
            modelValue += randomOffset;
            var formatValue = dmgModel.valueFormat.FormatValue(modelValue);
            // 参考属性值
            float refAttrValue = dmgModel.refType.GetRefAttributeValue(actor, target);

            // 减伤
            var dmgValue = refAttrValue * formatValue;
            switch (dmgModel.dmgType)
            {
                case GameActionDmgType.Real:
                    break;
                case GameActionDmgType.Normal:
                    var dmgResist = target.attributeCom.GetValue(GameAttributeType.NormalDmgResist);
                    var dmgReduce = dmgResist / (dmgResist + 100);
                    dmgValue = dmgValue * (1 - dmgReduce);
                    break;
                default:
                    GameLogger.LogError("未处理的伤害类型");
                    break;
            }

            // 暴击
            var critRate = actor.attributeCom.GetValue(GameAttributeType.CritRate);
            var isCrit = GameRandomService.GetRandom(0f, 1f) < critRate;
            if (isCrit)
            {
                var critDmgAddition = actor.attributeCom.GetValue(GameAttributeType.CritDmgAddition);
                dmgValue = dmgValue * (1.5f + critDmgAddition);
            }

            // 增伤
            var dmgAddition = actor.attributeCom.GetValue(GameAttributeType.DmgAddition);
            dmgValue = dmgValue * (1.5f + dmgAddition);

            // 伤害结算
            var targetAttrCom = target.attributeCom;
            var curHP = targetAttrCom.GetValue(GameAttributeType.HP);
            var afterDmgHP = curHP - dmgValue;
            var realDmg = afterDmgHP < 0 ? curHP : dmgValue;

            var actorRoleIdArgs = actor.GetLinkParent<GameRoleEntity>()?.idCom.ToArgs() ?? default;
            var record = new GameActionRecord_Dmg(
                dmgModel.typeId,
                actorRoleIdArgs,
                actor.idCom.ToArgs(),
                target.idCom.ToArgs(),
                actor.actionTargeterCom.getCurTargeterAsRecord(),
                dmgModel.dmgType,
                realDmg,
                isCrit
            );
            return record;
        }

        public static bool DoDmg(GameEntityBase target, ref GameActionRecord_Dmg record)
        {
            var targetAttrCom = target.attributeCom;
            var curHP = targetAttrCom.GetValue(GameAttributeType.HP);
            if (curHP <= 0)
            {
                return false;
            }

            var dmgValue = record.value;

            // 护盾
            var shield = targetAttrCom.GetValue(GameAttributeType.Shield);
            var shieldValue = dmgValue > shield ? shield : dmgValue;
            var afterShield = shield - shieldValue;
            targetAttrCom.SetAttribute(GameAttributeType.Shield, afterShield);
            dmgValue -= shieldValue;

            var afterDmgHP = curHP - dmgValue;
            targetAttrCom.SetAttribute(GameAttributeType.HP, afterDmgHP);
            if (afterDmgHP <= 0)
            {
                record.isKill = true;
            }
            GameLogger.DebugLog($"目标:{target.idCom} 受到伤害{GameMathF.GetFixed(dmgValue, 4)} ({curHP}=>{afterDmgHP})");
            return true;
        }
    }
}
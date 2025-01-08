using GamePlay.Core;

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
            var randomOffset = GameMathF.RandomRange(dmgModel.randomValueOffset);
            modelValue += randomOffset;
            var formatValue = dmgModel.valueFormat.FormatValue(modelValue);
            // 参考属性值
            float refAttrValue = dmgModel.refType.GetRefAttributeValue(actor, target);

            // 伤害数值 增幅/减幅
            var dmgValue = refAttrValue * formatValue;
            switch (dmgModel.dmgType)
            {
                case GameActionDmgType.Real:
                    break;
                case GameActionDmgType.Physical:
                    // 物理减伤
                    var armor = target.attributeCom.GetValue(GameAttributeType.Armor);
                    var physicalDmgReduce = armor / (armor + 100);
                    dmgValue = dmgValue * (1 - physicalDmgReduce);
                    break;
                case GameActionDmgType.Magic:
                    // 魔法减伤
                    var magicResist = target.attributeCom.GetValue(GameAttributeType.MagicResist);
                    var magicDmgReduce = magicResist / (magicResist + 100);
                    dmgValue = dmgValue * (1 - magicDmgReduce);
                    break;
                default:
                    GameLogger.LogError("未处理的伤害类型");
                    break;
            }

            // 伤害结算
            var targetAttrCom = target.attributeCom;
            var curHP = targetAttrCom.GetValue(GameAttributeType.HP);
            var afterDmgHP = curHP - dmgValue;
            var realDmg = afterDmgHP < 0 ? curHP : dmgValue;

            // 取2位小数
            realDmg = GameMathF.ToFixed(realDmg, 2);

            var actorRoleIdArgs = actor.GetLinkParent<GameRoleEntity>()?.idCom.ToArgs() ?? default;
            var record = new GameActionRecord_Dmg(
                dmgModel.typeId,
                actorRoleIdArgs,
                actor.idCom.ToArgs(),
                target.idCom.ToArgs(),
                actor.actionTargeterCom.getCurTargeterAsRecord(),
                dmgModel.dmgType,
                realDmg
            );
            return record;
        }

        public static bool DoDmg(GameEntityBase target, GameActionRecord_Dmg record)
        {
            var targetAttrCom = target.attributeCom;
            var curHP = targetAttrCom.GetValue(GameAttributeType.HP);
            if (curHP <= 0)
            {
                return false;
            }

            var afterDmgHP = curHP - record.value;
            targetAttrCom.SetAttribute(GameAttributeType.HP, afterDmgHP);
            GameLogger.Log($"目标:{target.idCom} 受到伤害{record.value} ({curHP}=>{afterDmgHP})");
            return true;
        }
    }
}
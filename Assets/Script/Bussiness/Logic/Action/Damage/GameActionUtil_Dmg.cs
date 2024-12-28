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
            var modelValue = dmgModel.valueFormat.FormatValue(dmgModel.value);
            // 参考属性值
            float refAttrValue = dmgModel.refType.GetRefAttributeValue(actor, target);

            // 伤害数值 增幅/减幅
            var dmgValue = refAttrValue * modelValue;
            switch (dmgModel.dmgType)
            {
                case GameActionDmgType.Real:
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

            var actorRoleIdArgs = actor.TryGetLinkEntity<GameEntityBase>()?.idCom.ToArgs() ?? default;
            var record = new GameActionRecord_Dmg(
                dmgModel.typeId,
                actorRoleIdArgs,
                actor.idCom.ToArgs(),
                target.idCom.ToArgs(),
                GameActionDmgType.Real,
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
            GameLogger.DebugLog($"目标:{target.idCom} 受到伤害{record.value} ({curHP}=>{afterDmgHP})");
            return true;
        }
    }
}
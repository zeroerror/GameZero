using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    /// <summary> 数值参考类型 </summary>
    public enum GameActionValueRefType
    {
        None,

        /// <summary> 固定数值 </summary>
        Fixed,

        /// <summary> 行为者攻击力 </summary>
        ActorAttack,
        /// <summary> 行为者基础攻击力 </summary>
        ActorBaseAttack,
        /// <summary> 目标攻击力 </summary>
        TargetAttack,
        /// <summary> 目标基础攻击力 </summary>
        TargetBaseAttack,

        /// <summary> 行为者血量 </summary>
        ActorHP,
        /// <summary> 目标血量 </summary>
        TargetHP,
        /// <summary> 行为者最大血量 </summary>
        ActorMaxHP,
        /// <summary> 目标最大血量 </summary>
        TargetMaxHP,
        /// <summary> 行为者已损失血量 </summary>
        ActorLostHP,
        /// <summary> 目标已损失血量 </summary>
        TargetLostHP,

        /// <summary> 行为者攻速 </summary>
        ActorAttackSpeed,
        /// <summary> 行为者基础攻速 </summary>
        ActorBaseAttackSpeed,
        /// <summary> 目标攻速 </summary>
        TargetAttackSpeed,
        /// <summary> 目标基础攻速 </summary>
        TargetBaseAttackSpeed,

        /// <summary> 行为者魔法值 </summary>
        ActorMP,
        /// <summary> 目标魔法值 </summary>
        TargetMP,

        /// <summary> 行为者护甲 </summary>
        ActorArmor,
        /// <summary> 目标护甲 </summary>
        TargetArmor,

        /// <summary> 行为者魔抗 </summary>
        ActorMagicResis,
        /// <summary> 目标魔抗 </summary>
        TargetMagicResist,
    }

    public static class GameActionValueRefTypeExt
    {
        /// <summary>
        /// 获取参考属性值
        /// <para> actor 行为者 </para>
        /// <para> target 目标 </para>
        /// </summary>
        public static float GetRefAttributeValue(this GameActionValueRefType refType, GameEntityBase actor, GameEntityBase target)
        {
            // 参考属性值
            float refAttrValue = 0;
            var actorAttrCom = actor.attributeCom;
            var targetAttrCom = target.attributeCom;
            var actorAttrCom_base = actor.baseAttributeCom;
            var targetAttrCom_base = target.baseAttributeCom;
            switch (refType)
            {
                case GameActionValueRefType.Fixed:
                    refAttrValue = 1;
                    break;

                case GameActionValueRefType.ActorAttack:
                    refAttrValue = actorAttrCom.GetValue(GameAttributeType.Attack);
                    break;
                case GameActionValueRefType.ActorBaseAttack:
                    refAttrValue = actorAttrCom_base.GetValue(GameAttributeType.Attack);
                    break;
                case GameActionValueRefType.TargetAttack:
                    refAttrValue = targetAttrCom.GetValue(GameAttributeType.Attack);
                    break;
                case GameActionValueRefType.TargetBaseAttack:
                    refAttrValue = targetAttrCom_base.GetValue(GameAttributeType.Attack);
                    break;

                case GameActionValueRefType.ActorHP:
                    refAttrValue = actorAttrCom.GetValue(GameAttributeType.HP);
                    break;
                case GameActionValueRefType.TargetHP:
                    refAttrValue = targetAttrCom.GetValue(GameAttributeType.HP);
                    break;
                case GameActionValueRefType.ActorMaxHP:
                    refAttrValue = actorAttrCom.GetValue(GameAttributeType.MaxHP);
                    break;
                case GameActionValueRefType.TargetMaxHP:
                    refAttrValue = targetAttrCom.GetValue(GameAttributeType.MaxHP);
                    break;
                case GameActionValueRefType.ActorLostHP:
                    refAttrValue = actorAttrCom.GetValue(GameAttributeType.MaxHP) - actorAttrCom.GetValue(GameAttributeType.HP);
                    break;
                case GameActionValueRefType.TargetLostHP:
                    refAttrValue = targetAttrCom.GetValue(GameAttributeType.MaxHP) - targetAttrCom.GetValue(GameAttributeType.HP);
                    break;
                case GameActionValueRefType.ActorAttackSpeed:
                    refAttrValue = actorAttrCom.GetValue(GameAttributeType.AttackSpeed);
                    break;
                case GameActionValueRefType.TargetAttackSpeed:
                    refAttrValue = targetAttrCom.GetValue(GameAttributeType.AttackSpeed);
                    break;
                case GameActionValueRefType.ActorBaseAttackSpeed:
                    refAttrValue = actorAttrCom_base.GetValue(GameAttributeType.AttackSpeed);
                    break;
                case GameActionValueRefType.TargetBaseAttackSpeed:
                    refAttrValue = targetAttrCom_base.GetValue(GameAttributeType.AttackSpeed);
                    break;
                case GameActionValueRefType.ActorMP:
                    refAttrValue = actorAttrCom.GetValue(GameAttributeType.MP);
                    break;
                case GameActionValueRefType.TargetMP:
                    refAttrValue = targetAttrCom.GetValue(GameAttributeType.MP);
                    break;

                case GameActionValueRefType.ActorArmor:
                    refAttrValue = actorAttrCom.GetValue(GameAttributeType.Armor);
                    break;
                case GameActionValueRefType.TargetArmor:
                    refAttrValue = targetAttrCom.GetValue(GameAttributeType.Armor);
                    break;

                case GameActionValueRefType.ActorMagicResis:
                    refAttrValue = actorAttrCom.GetValue(GameAttributeType.MagicResist);
                    break;
                case GameActionValueRefType.TargetMagicResist:
                    refAttrValue = targetAttrCom.GetValue(GameAttributeType.MagicResist);
                    break;
                default:
                    GameLogger.LogError("未处理的数值参考类型: " + refType);
                    break;
            }
            return refAttrValue;
        }

    }
}
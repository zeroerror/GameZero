using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    /// <summary> 数值参考类型 </summary>
    public enum GameActionValueRefType
    {
        None,

        /** 固定数值 */
        Fixed,

        /** 行为者攻击力 */
        ActorAttack,
        /** 目标攻击力 */
        TargetAttack,

        /** 行为者血量 */
        ActorHP,
        /** 目标血量 */
        TargetHP,
        /** 行为者最大血量 */
        ActorMaxHP,
        /** 目标最大血量 */
        TargetMaxHP,
        /** 行为者已损失血量 */
        ActorLostHP,
        /** 目标已损失血量 */
        TargetLostHP,
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
            var actorBaseAttrCom = actor.baseAttributeCom;
            var targetBaseAttrCom = target.baseAttributeCom;
            switch (refType)
            {
                case GameActionValueRefType.Fixed:
                    refAttrValue = 1;
                    break;
                case GameActionValueRefType.ActorAttack:
                    refAttrValue = actorAttrCom.GetValue(GameAttributeType.Attack);
                    break;
                case GameActionValueRefType.TargetAttack:
                    refAttrValue = targetAttrCom.GetValue(GameAttributeType.Attack);
                    break;
                case GameActionValueRefType.ActorHP:
                    refAttrValue = actorAttrCom.GetValue(GameAttributeType.HP);
                    break;
                case GameActionValueRefType.TargetHP:
                    refAttrValue = targetAttrCom.GetValue(GameAttributeType.HP);
                    break;
                case GameActionValueRefType.ActorMaxHP:
                    refAttrValue = actorBaseAttrCom.GetValue(GameAttributeType.HP);
                    break;
                case GameActionValueRefType.TargetMaxHP:
                    refAttrValue = targetBaseAttrCom.GetValue(GameAttributeType.HP);
                    break;
                case GameActionValueRefType.ActorLostHP:
                    refAttrValue = actorBaseAttrCom.GetValue(GameAttributeType.HP) - actorAttrCom.GetValue(GameAttributeType.HP);
                    break;
                case GameActionValueRefType.TargetLostHP:
                    refAttrValue = targetBaseAttrCom.GetValue(GameAttributeType.HP) - targetAttrCom.GetValue(GameAttributeType.HP);
                    break;
                default:
                    GameLogger.LogError("未处理的治疗参考类型" + refType);
                    break;
            }
            return refAttrValue;
        }

    }
}
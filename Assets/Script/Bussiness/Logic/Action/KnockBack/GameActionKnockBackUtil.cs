using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public static class GameActionKnockBackUtil
    {
        /// <summary>
        /// 计算击退
        /// <para> actor: 施放者 </para>
        /// <para> target: 目标 </para>
        /// <para> action: 行为 </para>
        /// </summary>
        public static GameActionRecord_KnockBack CalcKnockBack(GameEntityBase actor, GameEntityBase target, GameActionModel_KnockBack action)
        {
            var dir = GameVec2.zero;
            switch (action.knockBackDirType)
            {
                case GameActionKnockBackDirType.ToTarget:
                    dir = _GetKnockBack_ToTarget(actor, target, action);
                    break;
                case GameActionKnockBackDirType.ToSelf:
                    dir = _GetKnockBack_ToSelf(actor, target, action);
                    break;
                case GameActionKnockBackDirType.SelfForward:
                    dir = _GetKnockBack_SelfForward(actor, target, action);
                    break;
                default:
                    GameLogger.LogError($"未处理的击退方向类型：{action.knockBackDirType}");
                    break;
            }

            // 如果方向为0，则默认使用自身朝向
            if (dir == GameVec2.zero)
            {
                dir = _GetKnockBack_SelfForward(actor, target, action);
            }

            // 击退抗性
            var knockBackResist = target.attributeCom.GetValue(GameAttributeType.KnockbackResist);
            var distance = action.distance * (1 - knockBackResist);

            var actorRoleIdArgs = actor.TryGetLinkEntity<GameEntityBase>()?.idCom.ToArgs() ?? default;
            var record = new GameActionRecord_KnockBack(
                actorRoleIdArgs,
                actor.idCom.ToArgs(),
                target.idCom.ToArgs(),
                dir,
                distance,
                action.duration,
                action.easingType
           );
            return record;
        }

        private static GameVec2 _GetKnockBack_ToTarget(GameEntityBase actor, GameEntityBase target, GameActionModel_KnockBack action)
        {
            var dir = target.transformCom.position - actor.transformCom.position;
            dir.Normalize();
            return dir;
        }

        private static GameVec2 _GetKnockBack_ToSelf(GameEntityBase actor, GameEntityBase target, GameActionModel_KnockBack action)
        {
            var dir = actor.transformCom.position - target.transformCom.position;
            dir.Normalize();
            return dir;
        }

        private static GameVec2 _GetKnockBack_SelfForward(GameEntityBase actor, GameEntityBase target, GameActionModel_KnockBack action)
        {
            var dir = actor.transformCom.forward;
            return dir;
        }

        public static void DoKnockBack(GameEntityBase target, GameActionRecord_KnockBack record, GameTransformDomainApi transformApi)
        {
            var targetTransform = target.transformCom;
            ref var dir = ref record.direction;
            var dis = record.distance;
            var pos = targetTransform.position + new GameVec2(dir.x, dir.y) * dis;
            transformApi.ToPosition(target.transformCom, pos, record.duration, record.easingType);
        }
    }
}
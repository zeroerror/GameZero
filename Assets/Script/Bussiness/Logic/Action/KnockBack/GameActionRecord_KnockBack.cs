using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public struct GameActionRecord_KnockBack
    {
        /// <summary> 行为Id </summary>
        public int actionId;
        /// <summary> 行为者角色Id参数, 不一定存在 </summary>
        public GameIdArgs actorRoleIdArgs;
        /// <summary> 行为者Id参数, 比如技能, 投射物, BUFF等 </summary>
        public GameIdArgs actorIdArgs;

        /// <summary> 目标角色Id参数 </summary>
        public GameIdArgs targetIdArgs;
        /// <summary> 击退方向 </summary>
        public GameVec2 direction;
        /// <summary> 击退距离 </summary>
        public float distance;
        /// <summary> 持续时间 </summary>
        public float duration;
        /// <summary> 缓动曲线 </summary>
        public GameEasingType easingType;

        public GameActionRecord_KnockBack(
            int actionId,
            in GameIdArgs actorRoleIdArgs,
            in GameIdArgs actorIdArgs,
            in GameIdArgs targetRoleIdArgs,
            in GameVec2 direction,
            in float distance,
            float duration,
            GameEasingType easingType
        )
        {
            this.actionId = actionId;
            this.actorRoleIdArgs = actorRoleIdArgs;
            this.actorIdArgs = actorIdArgs;
            this.targetIdArgs = targetRoleIdArgs;
            this.direction = direction;
            this.distance = distance;
            this.duration = duration;
            this.easingType = easingType;
        }
    }
}
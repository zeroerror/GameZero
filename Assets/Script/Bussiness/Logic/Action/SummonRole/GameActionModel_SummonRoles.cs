using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameActionModel_SummonRoles : GameActionModelBase
    {
        /// <summary> 角色类型Id </summary>
        public readonly int roleId;
        /// <summary> 个数 </summary>
        public readonly int count;
        /// <summary> 阵营类型 </summary>
        public readonly GameCampType campType;

        public GameActionModel_SummonRoles(
            int typeId,
            GameEntitySelector selector,
            GameActionPreconditionSetModel preconditionSet,
            in GameVec2 randomValueOffset,
            int roleId,
            int count,
            GameCampType campType
        ) : base(GameActionType.SummonRoles, typeId, selector, preconditionSet, randomValueOffset)
        {
            this.roleId = roleId;
            this.count = count;
            this.campType = campType;
        }

        public override GameActionModelBase GetCustomModel(float customParam)
        {
            // 召唤角色的个数 = 自定义参数 * 原始召唤角色的个数
            var count_custom = GameMath.Floor(customParam * count);
            return new GameActionModel_SummonRoles(
                typeId,
                selector,
                preconditionSet,
                randomValueOffset,
                roleId,
                count_custom,
                campType
            );
        }

        public override string ToString()
        {
            return $"角色类型Id:{roleId}, 个数:{count}, 阵营类型:{campType.ToDesc()}";
        }
    }
}
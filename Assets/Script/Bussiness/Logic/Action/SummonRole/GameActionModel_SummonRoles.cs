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
            int roleId,
            int count,
            GameCampType campType
        ) : base(GameActionType.SummonRoles, typeId, selector, preconditionSet)
        {
            this.roleId = roleId;
            this.count = count;
            this.campType = campType;
        }

        public override string ToString()
        {
            return $"角色类型Id:{roleId}, 个数:{count}, 阵营类型:{campType.ToDesc()}";
        }
    }
}
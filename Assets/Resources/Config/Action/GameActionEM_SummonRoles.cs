using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEM_SummonRoles
    {
        public GameRoleSO roleSO;
        public int count;
        public GameCampType campType;

        public GameEntitySelectorEM selectorEM;
        public GameActionPreconditionSetEM preconditionSetEM;
        public GameVec2 randomValueOffset;

        public GameActionModel_SummonRoles ToModel()
        {
            if (roleSO == null)
            {
                GameLogger.LogError($"召唤角色行为未设置角色");
                return null;
            }
            var model = new GameActionModel_SummonRoles(
                0,
                this.selectorEM.ToModel(),
                this.preconditionSetEM?.ToModel(),
                this.randomValueOffset,
                this.roleSO.typeId,
                this.count,
                this.campType
            );
            return model;
        }
    }
}
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
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

        public GameActionModel_SummonRoles ToModel()
        {
            if (roleSO == null)
            {
                GameLogger.LogError($"召唤角色行为未设置角色");
                return null;
            }
            var model = new GameActionModel_SummonRoles(
                0,
                selectorEM.ToSelector(),
                preconditionSetEM?.ToModel(),
                roleSO.typeId,
                count,
                campType
            );
            return model;
        }
    }
}
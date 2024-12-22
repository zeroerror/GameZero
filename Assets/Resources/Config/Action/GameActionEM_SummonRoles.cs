using GamePlay.Bussiness.Logic;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEM_SummonRoles
    {
        public int roleId;
        public int count;
        public GameCampType campType;

        public GameEntitySelectorEM selectorEM;
        public GameActionPreconditionSetEM preconditionSetEM;

        public GameActionModel_SummonRoles ToModel()
        {
            var model = new GameActionModel_SummonRoles(
                0,
                selectorEM.ToSelector(),
                preconditionSetEM?.ToModel(),
                roleId,
                count,
                campType
            );
            return model;
        }
    }
}
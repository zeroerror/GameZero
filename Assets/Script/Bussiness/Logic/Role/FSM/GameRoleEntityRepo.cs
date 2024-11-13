using System.Collections.Generic;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleEntityRepo : GameEntityRepoBase
    {
        public override Dictionary<int, GameEntity> entityDict => this._entityDict;
        Dictionary<int, GameEntity> _entityDict = new Dictionary<int, GameEntity>();

        public GameRoleEntityRepo()
        {
        }
    }
}
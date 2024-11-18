using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleFactory
    {
        public GameRoleFactory() { }

        public GameRoleEntity Load(int typeId)
        {
            var e = new GameRoleEntity(typeId);
            return e;
        }
    }
}
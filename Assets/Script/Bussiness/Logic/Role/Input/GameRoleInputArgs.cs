using System.Collections.Generic;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public struct GameRoleInputArgs
    {
        public GameVec2 moveDir;
        public GameVec2 chooseDirection;
        public GameVec2 choosePoint;
        public int skillId;
        public List<GameActionTargeterArgs> targeterArgsList;

        public bool HasInput()
        {
            if (moveDir != GameVec2.zero) return true;
            if (chooseDirection != GameVec2.zero) return true;
            if (choosePoint != GameVec2.zero) return true;
            if (skillId > 0) return true;
            return false;
        }

        public void Update(in GameRoleInputArgs inputArgs)
        {
            this.moveDir = inputArgs.moveDir == GameVec2.zero ? this.moveDir : inputArgs.moveDir;
            this.chooseDirection = inputArgs.chooseDirection == GameVec2.zero ? this.chooseDirection : inputArgs.chooseDirection;
            this.choosePoint = inputArgs.choosePoint == GameVec2.zero ? this.choosePoint : inputArgs.choosePoint;
            this.skillId = inputArgs.skillId == 0 ? this.skillId : inputArgs.skillId;
        }
    }
}
using System.Collections.Generic;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public struct GameRoleInputArgs
    {
        public GameVec2 moveDir;
        public GameVec2 faceDir;
        public GameVec2 dstPos;
        public int skillId;
        public List<IGameActionTargeter> targeterList;

        public bool HasInput()
        {
            if (moveDir != GameVec2.zero) return true;
            if (faceDir != GameVec2.zero) return true;
            if (dstPos != GameVec2.zero) return true;
            if (skillId > 0) return true;
            return false;
        }

        public void Update(in GameRoleInputArgs inputArgs)
        {
            this.moveDir = inputArgs.moveDir == GameVec2.zero ? this.moveDir : inputArgs.moveDir;
            this.faceDir = inputArgs.faceDir == GameVec2.zero ? this.faceDir : inputArgs.faceDir;
            this.dstPos = inputArgs.dstPos == GameVec2.zero ? this.dstPos : inputArgs.dstPos;
            this.skillId = inputArgs.skillId == 0 ? this.skillId : inputArgs.skillId;
        }
    }
}
using System.Collections.Generic;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public struct GameRoleInputArgs
    {
        public bool enable;
        public GameVec2 moveDir;
        public GameVec2 faceDir;
        public GameVec2 dstPos;
        public int skillId;
        public List<IGameActionTargeter> targeterList;
    }
}
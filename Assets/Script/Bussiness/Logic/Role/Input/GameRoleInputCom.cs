using System.Collections.Generic;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleInputCom
    {
        public bool enable { get; set; }
        public GameVec2 moveDir { get; set; }
        public GameVec2 faceDir { get; set; }
        public GameVec2 dstPos { get; set; }
        public int skillId { get; set; }
        public List<IGameActionTargeter> targeterList { get; private set; }

        public GameRoleInputCom()
        {
            this.enable = true;
            this.targeterList = new List<IGameActionTargeter>();
        }

        public void Clear()
        {
            this.moveDir = GameVec2.zero;
            this.faceDir = GameVec2.zero;
            this.dstPos = GameVec2.zero;
            this.targeterList.Clear();
        }

        public void SetByArgs(in GameRoleInputArgs args)
        {
            this.moveDir = args.moveDir;
            this.faceDir = args.faceDir;
            this.dstPos = args.dstPos;
            this.targeterList.Clear();
            var targeterList = args.targeterList;
            if (targeterList != null && targeterList.Count > 0) targeterList.AddRange(targeterList);
        }

        public bool TryGetInputArgs(out GameRoleInputArgs inputArgs)
        {
            var hasInput =
            this.moveDir != GameVec2.zero ||
             this.faceDir != GameVec2.zero ||
             this.dstPos != GameVec2.zero ||
             this.skillId != 0 ||
             this.targeterList.Count > 0;
            if (!hasInput)
            {
                inputArgs = default;
                return false;
            }
            inputArgs = new GameRoleInputArgs
            {
                moveDir = this.moveDir,
                faceDir = this.faceDir,
                dstPos = this.dstPos,
                skillId = this.skillId,
                targeterList = this.targeterList,
            };
            return true;
        }
    }
}
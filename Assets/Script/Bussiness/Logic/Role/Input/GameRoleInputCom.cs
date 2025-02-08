using System.Collections.Generic;
using System.Linq;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleInputCom
    {
        public bool enable { get; set; }
        public GameVec2 moveDir { get; set; }
        public GameVec2 moveDst { get; set; }
        public int skillId { get; set; }
        public List<GameActionTargeterArgs> targeterArgsList { get; private set; }

        public GameRoleInputCom()
        {
            this.enable = true;
            this.targeterArgsList = new List<GameActionTargeterArgs>();
        }

        public void Clear()
        {
            this.moveDir = GameVec2.zero;
            this.moveDst = GameVec2.zero;
            this.targeterArgsList.Clear();
            this.skillId = 0;
        }

        public GameRoleInputArgs ToArgs()
        {
            return new GameRoleInputArgs
            {
                moveDir = this.moveDir,
                moveDst = this.moveDst,
                skillId = this.skillId,
                targeterArgsList = this.targeterArgsList.ToList(),
            };
        }

        public void SetByArgs(in GameRoleInputArgs args)
        {
            this.moveDir = args.moveDir;
            this.moveDst = args.moveDst;
            this.targeterArgsList.Clear();
            var targeterList = args.targeterArgsList;
            if (targeterList != null && targeterList.Count > 0) this.targeterArgsList.AddRange(targeterList);
            this.skillId = args.skillId;
        }

        public bool TryGetInputArgs(out GameRoleInputArgs inputArgs)
        {
            var hasInput =
            this.moveDir != GameVec2.zero ||
            this.moveDst != GameVec2.zero ||
            this.skillId != 0 ||
            this.targeterArgsList.Count > 0;
            if (!hasInput)
            {
                inputArgs = default;
                return false;
            }
            inputArgs = new GameRoleInputArgs
            {
                moveDir = this.moveDir,
                moveDst = this.moveDst,
                skillId = this.skillId,
                targeterArgsList = this.targeterArgsList.ToList(),
            };
            return true;
        }

        public bool HasInput()
        {
            return this.moveDir != GameVec2.zero ||
            this.moveDst != GameVec2.zero ||
            this.skillId != 0 ||
            this.targeterArgsList.Count > 0;
        }
    }
}
using System.Collections.Generic;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleInputCom
    {
        public bool enable { get; set; }
        public GameVec2 moveDir { get; set; }
        public GameVec2 chooseDirection { get; set; }
        public GameVec2 choosePosition { get; set; }
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
            this.chooseDirection = GameVec2.zero;
            this.choosePosition = GameVec2.zero;
            this.targeterArgsList.Clear();
            this.skillId = 0;
        }

        public void SetByArgs(in GameRoleInputArgs args)
        {
            this.moveDir = args.moveDir;
            this.chooseDirection = args.chooseDirection;
            this.choosePosition = args.choosePoint;
            this.targeterArgsList.Clear();
            var targeterList = args.targeterArgsList;
            if (targeterList != null && targeterList.Count > 0) this.targeterArgsList.AddRange(targeterList);
            this.skillId = args.skillId;
        }

        public bool TryGetInputArgs(out GameRoleInputArgs inputArgs)
        {
            var hasInput =
            this.moveDir != GameVec2.zero ||
             this.chooseDirection != GameVec2.zero ||
             this.choosePosition != GameVec2.zero ||
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
                chooseDirection = this.chooseDirection,
                choosePoint = this.choosePosition,
                skillId = this.skillId,
                targeterArgsList = this.targeterArgsList,
            };
            return true;
        }
    }
}
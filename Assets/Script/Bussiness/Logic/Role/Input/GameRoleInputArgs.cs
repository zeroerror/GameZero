using System.Collections.Generic;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public struct GameRoleInputArgs
    {
        public GameVec2 moveDir;
        public GameVec2 moveDst;
        public int skillId;
        public List<GameActionTargeterArgs> targeterArgsList;

        public bool HasInput()
        {
            if (this.HasMoveInput()) return true;
            if (this.HasSkillInput()) return true;
            if (targeterArgsList != null && targeterArgsList.Count > 0) return true;
            return false;
        }

        public bool HasSkillInput()
        {
            return skillId > 0;
        }

        public bool HasMoveInput()
        {
            if (!moveDir.Equals(GameVec2.zero)) return true;
            if (!moveDst.Equals(GameVec2.zero)) return true;
            return false;
        }

        public void Update(in GameRoleInputArgs inputArgs)
        {
            if (!inputArgs.moveDir.Equals(GameVec2.zero)) this.moveDir = inputArgs.moveDir;
            if (!inputArgs.moveDst.Equals(GameVec2.zero)) this.moveDst = inputArgs.moveDst;
            this.skillId = inputArgs.skillId == 0 ? this.skillId : inputArgs.skillId;
            this.targeterArgsList = inputArgs.targeterArgsList == null ? this.targeterArgsList : inputArgs.targeterArgsList;
        }

        public bool TryGetTargetEntity(out GameEntityBase targetEntity)
        {
            targetEntity = null;
            if (targeterArgsList == null || targeterArgsList.Count == 0) return false;
            var targeter = targeterArgsList.Find((targeter) => targeter.targetEntity != null);
            targetEntity = targeter.targetEntity;
            return targetEntity != null;
        }

        public bool TryGetTargetPosition(out GameVec2 targetPosition)
        {
            targetPosition = GameVec2.zero;
            if (targeterArgsList == null || targeterArgsList.Count == 0) return false;
            var targeter = targeterArgsList.Find((targeter) => !targeter.targetPosition.Equals(GameVec2.zero));
            targetPosition = targeter.targetPosition;
            return !targetPosition.Equals(GameVec2.zero);
        }

        public bool TryGetTargetDirection(out GameVec2 targetDirection)
        {
            targetDirection = GameVec2.zero;
            if (targeterArgsList == null || targeterArgsList.Count == 0) return false;
            var targeter = targeterArgsList.Find((targeter) => !targeter.targetDirection.Equals(GameVec2.zero));
            targetDirection = targeter.targetDirection;
            return !targetDirection.Equals(GameVec2.zero);
        }
    }
}
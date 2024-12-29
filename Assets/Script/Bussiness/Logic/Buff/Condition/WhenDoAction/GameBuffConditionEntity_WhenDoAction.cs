namespace GamePlay.Bussiness.Logic
{
    /// <summary> buff条件实体 - 当执行行为时 </summary>
    public class GameBuffConditionEntity_WhenDoAction : GameBuffConditionEntityBase
    {
        public GameBuffConditionModel_WhenDoAction model { get; private set; }

        public GameBuffConditionEntity_WhenDoAction(GameBuffEntity buff, GameBuffConditionModel_WhenDoAction model) : base(buff)
        {
            this.model = model;
        }

        protected override bool _Check()
        {
            var isSatisfied = false;

            // 遍历 - 伤害记录
            this.ForEachActionRecord_Dmg((actionRecord) =>
            {
                if (m_check(actionRecord.actorRoleIdArgs.entityId, actionRecord.actionId, GameActionType.Dmg))
                {
                    isSatisfied = true;
                }
            });
            if (isSatisfied) return true;

            // 遍历 - 治疗记录
            this.ForEachActionRecord_Heal((actionRecord) =>
            {
                if (m_check(actionRecord.actorRoleIdArgs.entityId, actionRecord.actionId, GameActionType.Heal))
                {
                    isSatisfied = true;
                }
            });
            if (isSatisfied) return true;

            // 遍历 - 发射投射物记录
            this.ForEachActionRecord_LaunchProjectile((actionRecord) =>
            {
                if (m_check(actionRecord.actorRoleIdArgs.entityId, actionRecord.actionId, GameActionType.LaunchProjectile))
                {
                    isSatisfied = true;
                }
            });
            if (isSatisfied) return true;

            return false;

            bool m_check(int entityId, int actionId, GameActionType actionType)
            {
                // 行为实体必须是Buff作用目标
                var isBuffTargetAct = entityId == _buff.target.idCom.entityId;
                if (!isBuffTargetAct) return false;
                // 指定行为Id
                if (actionId == model.targetActionId) return true;
                // 指定行为类型
                if (actionType == model.targetActionType) return true;
                return false;
            }
        }
    }
}
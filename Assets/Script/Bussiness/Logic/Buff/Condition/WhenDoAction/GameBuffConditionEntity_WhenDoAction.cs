namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// 时间间隔条件
    /// </summary>
    public class GameBuffConditionEntity_WhenDoAction : GameBuffConditionEntityBase
    {
        public GameBuffConditionModel_WhenDoAction model { get; private set; }

        public GameBuffConditionEntity_WhenDoAction(GameBuffEntity buff, GameBuffConditionModel_WhenDoAction model) : base(buff)
        {
            this.model = model;
        }

        protected override void _Tick(float dt)
        {
        }

        protected override bool _Check()
        {
            var isSatisfied = false;

            this.ForEachActionRecord_Dmg((actionRecord) =>
            {
                var actionId = actionRecord.actionId;
                if (actionId == model.targetActionId && actionRecord.actorRoleIdArgs.entityId == _buff.target.idCom.entityId)
                {
                    isSatisfied = true;
                }
            });
            if (isSatisfied) return true;

            this.ForEachActionRecord_Heal((actionRecord) =>
            {
                var actionId = actionRecord.actionId;
                if (actionId == model.targetActionId && actionRecord.actorRoleIdArgs.entityId == _buff.target.idCom.entityId)
                {
                    isSatisfied = true;
                }
            });
            if (isSatisfied) return true;

            this.ForEachActionRecord_LaunchProjectile((actionRecord) =>
            {
                var actionId = actionRecord.actionId;
                if (actionId == model.targetActionId && actionRecord.actorRoleIdArgs.entityId == _buff.target.idCom.entityId)
                {
                    isSatisfied = true;
                }
            });
            if (isSatisfied) return true;

            return false;
        }

        public override void Clear()
        {
        }
    }
}
namespace GamePlay.Bussiness.Logic
{
    /// <summary> buff条件实体 - 当执行行为时 </summary>
    public class GameBuffConditionEntity_WhenDoAction : GameBuffConditionEntityBase
    {
        public GameBuffConditionModel_WhenDoAction model { get; private set; }

        /// <summary> 行为次数 </summary>
        private int _actionCount;
        /// <summary> 已过窗口时间 </summary>
        private float _elapsedWindowTime;

        public GameBuffConditionEntity_WhenDoAction(GameBuffEntity buff, GameBuffConditionModel_WhenDoAction model) : base(buff)
        {
            this.model = model;
        }

        protected override void _Tick(float dt)
        {
            base._Tick(dt);
            this._TickWindowTime(dt);
        }

        private void _TickWindowTime(float dt)
        {
            if (this._actionCount <= 0) return;
            this._elapsedWindowTime += dt;
            if (this._elapsedWindowTime >= model.validWindowTime)
            {
                this._actionCount = 0;
            }
        }

        protected override bool _Check()
        {
            var isSatisfied = false;

            // 遍历 - 伤害记录
            this.ForeachActionRecord_Dmg((actionRecord) =>
            {
                m_check(actionRecord.actorIdArgs, actionRecord.actionTargeter, actionRecord.targetIdArgs, actionRecord.actionId, GameActionType.Dmg);
            });
            if (isSatisfied) return true;

            // 遍历 - 治疗记录
            this.ForeachActionRecord_Heal((actionRecord) =>
            {
                m_check(actionRecord.actorIdArgs, actionRecord.actionTargeter, actionRecord.targetIdArgs, actionRecord.actionId, GameActionType.Heal);
            });
            if (isSatisfied) return true;

            // 遍历 - 发射投射物记录
            this.ForeachActionRecord_LaunchProjectile((actionRecord) =>
            {
                m_check(actionRecord.actorIdArgs, actionRecord.actionTargeter, actionRecord.targetIdArgs, actionRecord.actionId, GameActionType.LaunchProjectile);
            });
            if (isSatisfied) return true;

            return false;

            void m_check(in GameIdArgs actorIdArgs, in GameActionTargeterArgsRecord actionTargeterRecord, in GameIdArgs targetIdArgs, int actionId, GameActionType actionType)
            {
                // 已满足条件, 不再检查
                if (isSatisfied) return;

                var actorEntity = this.FindEntity(actorIdArgs.entityType, actorIdArgs.entityId);
                if (!actorEntity) return;
                var actorRoleEntity = actorEntity.TryGetLinkParent<GameRoleEntity>();
                if (!actorRoleEntity) return;

                // 行为实体必须是Buff作用目标
                var isBuffTargetAct = actorRoleEntity.idCom.entityId == _buff.target.idCom.entityId;
                if (!isBuffTargetAct) return;

                // 指定行为Id
                if (actionId == model.targetActionId)
                {
                    this._actionCount++;
                }
                // 指定行为类型
                if (actionType == model.targetActionType)
                {
                    this._actionCount++;
                }

                // 窗口期时间开始
                if (this._actionCount == 1)
                {
                    this._elapsedWindowTime = 0f;
                }

                // TODO: 应该记录一个list, 因为可能有多个行为满足条件, 都需要对其处理
                // 满足时, 同步目标选取器到buff 
                isSatisfied = this._actionCount >= model.actionCount;
                if (isSatisfied)
                {
                    var targetEntity = this.FindEntity(targetIdArgs.entityType, targetIdArgs.entityId);
                    var targeter = new GameActionTargeterArgs(
                        targetEntity,
                        actionTargeterRecord.targetPosition,
                        actionTargeterRecord.targetDirection
                    );
                    this._buff.actionTargeterCom.SetTargeter(targeter);
                    this._actionCount = 0;
                }
            }
        }

        public override void Clear()
        {
            base.Clear();
            this._actionCount = 0;
            this._elapsedWindowTime = 0;
        }
    }
}
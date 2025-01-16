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
            if (model.validWindowTime > 0 && this._elapsedWindowTime >= model.validWindowTime)
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
                isSatisfied = _MainCheck(actionRecord.actorIdArgs, actionRecord.actionTargeter, actionRecord.targetIdArgs, actionRecord.actionId, GameActionType.Dmg);
                if (!isSatisfied) isSatisfied = this._ExtraCheckDmg(actionRecord.actorIdArgs, actionRecord, actionRecord.targetIdArgs);
            });
            if (isSatisfied) return true;

            // 遍历 - 治疗记录
            this.ForeachActionRecord_Heal((actionRecord) =>
            {
                isSatisfied = _MainCheck(actionRecord.actorIdArgs, actionRecord.actionTargeter, actionRecord.targetIdArgs, actionRecord.actionId, GameActionType.Heal);
            });
            if (isSatisfied) return true;

            // 遍历 - 发射投射物记录
            this.ForeachActionRecord_LaunchProjectile((actionRecord) =>
            {
                isSatisfied = _MainCheck(actionRecord.actorIdArgs, actionRecord.actionTargeter, actionRecord.targetIdArgs, actionRecord.actionId, GameActionType.LaunchProjectile);
            });

            return isSatisfied;
        }

        private bool _BasicCheck(in GameIdArgs actorIdArgs)
        {
            // 行为实体必须是Buff挂载的角色
            var actorEntity = this.FindEntity(actorIdArgs.entityType, actorIdArgs.entityId);
            if (!actorEntity) return false;
            var actorRoleEntity = actorEntity.GetLinkParent<GameRoleEntity>();
            if (!actorRoleEntity) return false;
            var isBuffTargetAct = actorRoleEntity.idCom.entityId == _buff.owner.idCom.entityId;
            if (!isBuffTargetAct) return false;
            // buff自身触发的行为不计数, 防止死循环
            var isBuffAct = this._buff.idCom.IsEquals(actorIdArgs);
            if (isBuffAct) return false;
            return true;
        }

        private bool _MainCheck(in GameIdArgs actorIdArgs, in GameActionTargeterArgsRecord actionTargeterRecord, in GameIdArgs targetIdArgs, int actionId, GameActionType actionType)
        {
            // 基础检查
            if (!_BasicCheck(actorIdArgs)) return false;
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
            return this._CheckSatisfy(actionTargeterRecord, targetIdArgs);
        }

        private bool _ExtraCheckDmg(in GameIdArgs actorIdArgs, GameActionRecord_Dmg record, in GameIdArgs targetIdArgs)
        {
            // 基础检查
            if (!this._BasicCheck(actorIdArgs)) return false;
            // 非击杀行为, 不做判定
            if (this.model.targetActionType != GameActionType.Kill) return false;
            // 跳过非击杀行为
            if (!record.isKill) return false;
            // 判定为击杀行为, 计数
            this._actionCount++;
            // 检查满足条件
            return this._CheckSatisfy(record.actionTargeter, targetIdArgs);
        }

        private bool _CheckSatisfy(in GameActionTargeterArgsRecord actionTargeterRecord, in GameIdArgs targetIdArgs)
        {
            // 窗口期时间开始
            if (this._actionCount == 1)
            {
                this._elapsedWindowTime = 0f;
            }
            // TODO: 应该记录一个list, 因为可能有多个行为满足条件, 都需要对其处理
            // 满足时, 同步目标选取器到buff 
            var isSatisfied = this._actionCount >= model.actionCount;
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

            return isSatisfied;
        }

        public override void Clear()
        {
            base.Clear();
            this._actionCount = 0;
            this._elapsedWindowTime = 0;
        }
    }
}
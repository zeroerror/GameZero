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
                isSatisfied = _NormalCheck(actionRecord.actorIdArgs, actionRecord.actionTargeter, actionRecord.targetIdArgs, actionRecord.actionId, GameActionType.Dmg);

                if (isSatisfied)
                {
                    // 尝试捕获伤害值作为行为参数
                    if (this.model.needCaptureActionParam)
                    {
                        this._buff.conditionActionParam += actionRecord.value;
                    }
                }
                else
                {
                    isSatisfied = this._CheckSpecialActionType(actionRecord.actorIdArgs, actionRecord, actionRecord.targetIdArgs);
                }
            });
            if (isSatisfied) return true;

            // 遍历 - 治疗记录
            this.ForeachActionRecord_Heal((actionRecord) =>
            {
                isSatisfied = _NormalCheck(actionRecord.actorIdArgs, actionRecord.actionTargeter, actionRecord.targetIdArgs, actionRecord.actionId, GameActionType.Heal);
                if (isSatisfied)
                {
                    // 尝试捕获治疗值作为行为参数
                    if (this.model.needCaptureActionParam)
                    {
                        this._buff.conditionActionParam += actionRecord.value;
                    }
                }
            });
            if (isSatisfied) return true;

            // 遍历 - 发射投射物记录
            this.ForeachActionRecord_LaunchProjectile((actionRecord) =>
            {
                isSatisfied = _NormalCheck(actionRecord.actorIdArgs, actionRecord.actionTargeter, actionRecord.targetIdArgs, actionRecord.actionId, GameActionType.LaunchProjectile);
            });

            return isSatisfied;
        }

        /// <summary> 正常检查 </summary>
        private bool _NormalCheck(in GameIdArgs actorIdArgs, in GameActionTargeterArgsRecord actionTargeterRecord, in GameIdArgs targetIdArgs, int actionId, GameActionType actionType)
        {
            // 基础检查
            if (!_BasicCheck(actorIdArgs, targetIdArgs)) return false;
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
            return this._CheckWindowTimeAndActionCount(actionTargeterRecord, targetIdArgs);
        }

        /// <summary> 检查特殊行为如击杀、暴击...... </summary>
        private bool _CheckSpecialActionType(in GameIdArgs actorIdArgs, GameActionRecord_Dmg record, in GameIdArgs targetIdArgs)
        {
            var targetActionType = this.model.targetActionType;
            if (!targetActionType.IsSpecialAction()) return false;
            // 基础检查
            if (!this._BasicCheck(actorIdArgs, targetIdArgs)) return false;
            // 检查是否为特殊行为
            var isSatisfied = false;
            switch (targetActionType)
            {
                case GameActionType.Kill:
                    isSatisfied = record.isKill;
                    break;
                case GameActionType.Crit:
                    isSatisfied = record.isCrit;
                    break;
            }
            if (!isSatisfied) return false;
            this._actionCount++;

            // 检查窗口时间和行为次数
            return this._CheckWindowTimeAndActionCount(record.actionTargeter, targetIdArgs);
        }

        private bool _BasicCheck(in GameIdArgs actorIdArgs, in GameIdArgs targetIdArgs)
        {
            var actor = this.FindEntity(actorIdArgs.entityType, actorIdArgs.entityId);
            if (!actor) return false;
            var actorRole = actor.GetLinkParent<GameRoleEntity>();
            if (!actorRole) return false;

            // 判定buff持有者在本次行为是行为方还是目标方
            if (this.model.isAsActionTarget)
            {
                // 行为目标实体要求是buff持有者
                var targetRole = this.FindEntity(targetIdArgs.entityType, targetIdArgs.entityId).GetLinkParent<GameRoleEntity>();
                var isBuffOwnerTarget = targetRole.idCom.entityId == _buff.owner.idCom.entityId;
                if (!isBuffOwnerTarget) return false;
            }
            else
            {
                // 行为实体要求是buff持有者
                var isBuffOwnerAct = actorRole.idCom.entityId == _buff.owner.idCom.entityId;
                if (!isBuffOwnerAct) return false;
            }

            // buff自身触发的行为不计数, 防止死循环
            var isBuffAct = this._buff.idCom.IsEquals(actorIdArgs);
            if (isBuffAct) return false;
            return true;
        }

        private bool _CheckWindowTimeAndActionCount(in GameActionTargeterArgsRecord actionTargeterRecord, in GameIdArgs targetIdArgs)
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
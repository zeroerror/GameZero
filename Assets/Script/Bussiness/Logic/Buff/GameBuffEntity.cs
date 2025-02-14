namespace GamePlay.Bussiness.Logic
{
    public class GameBuffEntity : GameEntityBase
    {
        public GameBuffModel model { get; private set; }

        /// <summary> 行为触发条件集 </summary>
        public GameBuffConditionSetEntity conditionSetEntity_action { get; private set; }

        /// <summary> buff移除条件集 </summary>
        public GameBuffConditionSetEntity conditionSetEntity_remove { get; private set; }

        /// <summary> buff已挂载时间 </summary>
        public float elapsedTime { get; private set; }
        /// <summary> 行为冷却时间 </summary>
        public float ationCDTime { get; private set; }
        /// <summary> 执行过的行为次数 </summary>
        public int actionedCount { get; private set; }
        /// <summary> buff已挂载层数 </summary>
        public int layer;

        /// <summary> buff持有者 </summary>
        public GameEntityBase owner;
        /// <summary> buff持有效果 </summary>
        public GameAttributeCom ownEffect { get; private set; }

        public GameBuffEntity(GameBuffModel model) : base(model.typeId, GameEntityType.Buff)
        {
            this.model = model;
            this.conditionSetEntity_action = new GameBuffConditionSetEntity(this, model.conditionSetModel_action);
            this.conditionSetEntity_remove = new GameBuffConditionSetEntity(this, model.conditionSetModel_remove);
            this.ownEffect = new GameAttributeCom();
        }

        public override void Destroy()
        {
        }

        public override void Clear()
        {
            this.BindTransformCom(null);
            this.ationCDTime = 0;
            this.actionedCount = 0;
            this.elapsedTime = 0;
            this.layer = 0;
            this.conditionSetEntity_action.Clear();
            this.conditionSetEntity_remove.Clear();
            this.conditionActionParam = 0;
            base.Clear();
        }

        public override void Tick(float dt)
        {
            this.elapsedTime += dt;

            // 行为冷却时间
            if (this.ationCDTime > 0)
            {
                this.ationCDTime -= dt;
            }

            // 行为条件集 - 触发
            var hasActionCD = this.ationCDTime > 0;
            if (!hasActionCD)
            {
                this.conditionSetEntity_action.Tick(dt);
            }

            // 行为条件集 - 移除
            this.conditionSetEntity_remove.Tick(dt);
        }

        /// <summary> 开始行为冷却 </summary>
        public void StartActionCD()
        {
            this.actionedCount++;
            this.ationCDTime = this.model.actionCD;
            this.conditionSetEntity_action.Clear();
            this.conditionActionParam = 0;
        }

        /// <summary> 结束行为冷却 </summary>
        public void EndActionCD()
        {
            this.ationCDTime = 0;
        }

        /// <summary> 被条件捕获的行为参数 </summary>
        public float conditionActionParam;
        public float GetActionParam()
        {
            var modelParam = this.model.actionParam * this.layer;
            var conditionParam = this.conditionActionParam * this.layer;
            var actionParam = modelParam + conditionParam;
            return actionParam;
        }
    }
}
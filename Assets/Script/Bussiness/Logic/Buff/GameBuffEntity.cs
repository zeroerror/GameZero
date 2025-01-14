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
        /** 行为冷却时间 */
        public float ationCDTime { get; private set; }
        /// <summary> buff已挂载层数 </summary>
        public int layer;

        /// <summary> buff目标 </summary>
        public GameEntityBase target;
        /// <summary> 目标效果 </summary>
        public GameAttributeCom targetEffect { get; private set; }

        public GameBuffEntity(GameBuffModel model) : base(model.typeId, GameEntityType.Buff)
        {
            this.model = model;
            this.conditionSetEntity_action = new GameBuffConditionSetEntity(this, model.conditionSetModel_action);
            this.conditionSetEntity_remove = new GameBuffConditionSetEntity(this, model.conditionSetModel_remove);
            this.targetEffect = new GameAttributeCom();
        }

        public override void Destroy()
        {
        }

        public override void Clear()
        {
            this.BindTransformCom(null);
            this.elapsedTime = 0;
            this.layer = 0;
            this.conditionSetEntity_action.Clear();
            this.conditionSetEntity_remove.Clear();
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

        public void StartCD()
        {
            this.ationCDTime = this.model.actionCD;
            this.conditionSetEntity_action.Clear();
        }

        public float GetActionParam()
        {
            return this.model.actionParam * this.layer;
        }
    }
}
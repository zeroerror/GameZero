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
        /// <summary> buff已挂载层数 </summary>
        public int layer;

        /// <summary> buff目标 </summary>
        public GameEntityBase target;

        public GameBuffEntity(GameBuffModel model) : base(model.typeId, GameEntityType.Buff)
        {
            this.model = model;
            this.conditionSetEntity_action = new GameBuffConditionSetEntity(this, model.conditionSetModel_action);
            this.conditionSetEntity_remove = new GameBuffConditionSetEntity(this, model.conditionSetModel_remove);
            this.layer = 0;
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
            this.conditionSetEntity_action.Tick(dt);
            this.conditionSetEntity_remove.Tick(dt);
        }

        public float GetActionParam()
        {
            return this.model.actionParam * this.layer;
        }
    }
}
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
        public int layer { get; private set; }

        public GameBuffEntity(GameBuffModel model) : base(model.typeId, GameEntityType.Buff)
        {
            this.model = model;
            this.conditionSetEntity_action = new GameBuffConditionSetEntity(model.conditionSetModel_action);
            this.conditionSetEntity_remove = new GameBuffConditionSetEntity(model.conditionSetModel_remove);
            this.layer = 1;
        }

        public override void Destroy()
        {
        }

        public override void Clear()
        {
            base.Clear();
            this.elapsedTime = 0;
            this.layer = 1;
        }

        public override void Tick(float dt)
        {
            this.elapsedTime += dt;
            this.conditionSetEntity_action.Tick(dt);
            this.conditionSetEntity_remove.Tick(dt);
        }

        public void AttachLayer(int layer = 1)
        {
            var m = this.model;
            if (m.refreshFlag.HasFlag(GameBuffRefreshFlag.RefreshTime))
            {
                this.conditionSetEntity_remove.RefreshTime();
            }
            if (m.refreshFlag.HasFlag(GameBuffRefreshFlag.StackTime))
            {
                this.conditionSetEntity_remove.StackTime();
            }
            if (m.refreshFlag.HasFlag(GameBuffRefreshFlag.StackLayer))
            {
                this.layer += layer;
            }
        }
    }
}
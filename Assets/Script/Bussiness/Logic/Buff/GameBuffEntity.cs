using GamePlay.Core;

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
            this.BindTransformCom(null);
            this.elapsedTime = 0;
            this.layer = 1;
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

        /// <summary>
        /// 挂载层数
        /// <para>layer: 层数</para>
        /// <returns> 实际挂载层数 </returns>
        /// </summary>
        public int AttachLayer(int layer = 1)
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
                var beforeLayer = this.layer;
                var afterLayer = beforeLayer + layer;
                var maxLayer = m.maxLayer == 0 ? int.MaxValue : m.maxLayer;// 0表示无限层数
                afterLayer = GameMath.Min(afterLayer, maxLayer);
                this.layer = afterLayer;
                return afterLayer - beforeLayer;
            }
            return 0;
        }

        /// <summary>
        /// 移除层数
        /// <para>layer: 层数(0代表全部移除)</para>
        /// <returns> 实际移除层数 </returns>
        /// </summary>
        public int DetachLayer(int layer)
        {
            layer = layer == 0 ? int.MaxValue : layer;

            var beforeLayer = this.layer;
            var afterLayer = beforeLayer - layer;
            afterLayer = GameMath.Max(afterLayer, 0);
            this.layer = afterLayer;
            if (afterLayer <= 0)
            {
                this.SetInvalid();
            }
            return beforeLayer - afterLayer;
        }
    }
}
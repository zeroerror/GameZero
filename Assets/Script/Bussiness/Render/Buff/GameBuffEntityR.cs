using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.Render
{
    public class GameBuffEntityR : GameEntityBase
    {
        public GameBuffModelR model { get; private set; }

        /// <summary> buff已挂载时间 </summary>
        public float elapsedTime { get; private set; }
        /// <summary> buff已挂载层数 </summary>
        public int layer { get; private set; }
        /// <summary> buff特效 </summary>
        public GameVFXEntity vfxEntity;

        public GameBuffEntityR(GameBuffModelR model) : base(model.typeId, GameEntityType.Buff)
        {
            this.model = model;
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
        }


        public void AttachLayer(int layer = 1)
        {
            this.layer += layer;
        }

        public void DetachLayer(int layer)
        {
            layer = layer == 0 ? this.layer : layer;// 0表示全部移除
            this.layer -= layer;
            if (this.layer <= 0)
            {
                this.SetInvalid();
            }
        }
    }
}
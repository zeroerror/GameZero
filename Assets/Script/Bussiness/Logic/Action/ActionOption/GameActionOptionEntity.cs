using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.Logic
{
    public class GameActionOptionEntity : GameEntityBase
    {
        public GameActionOptionModel model { get; private set; }
        public int lv { get; private set; }
        public GameBuffCom buffCom { get; private set; }

        public GameActionOptionEntity(GameActionOptionModel model) : base(model.typeId, GameEntityType.None)
        {
            this.model = model;
            this.buffCom = new GameBuffCom();
            this.lv = 1;
        }

        public override void Clear()
        {
            base.Clear();
            this.lv = 1;
        }

        public override void Tick(float dt)
        {
        }

        public override void Destroy()
        {
        }

        /// <summary>
        /// 添加等级
        /// </summary>
        public bool AddLevel()
        {
            if (this.lv == this.model.maxLv)
            {
                return false;
            }
            this.lv++;
            GameLogger.DebugLog($"行为选项升级至 {this.lv}: {this.model.typeId}{this.model.desc}");
            return true;
        }
    }
}
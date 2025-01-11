using System.Collections.Generic;
using GamePlay.Core;
namespace GamePlay.Bussiness.Renderer
{
    public class GameBuffComR
    {
        public List<GameBuffEntityR> buffList { get; private set; }

        public GameBuffComR()
        {
            this.buffList = new List<GameBuffEntityR>();
        }

        public bool HasBuff(int buffId)
        {
            return this.buffList.Exists(buff => buff.model.typeId == buffId);
        }

        public bool TryGet(int buffId, out GameBuffEntityR buff)
        {
            buff = this.buffList.Find(b => b.model.typeId == buffId);
            return buff != null;
        }

        public void Add(GameBuffEntityR buff)
        {
            if (this.HasBuff(buff.model.typeId))
            {
                GameLogger.LogError("Buff已存在，无法重复添加：" + buff.model.typeId);
                return;
            }
            this.buffList.Add(buff);
        }

        public bool Remove(GameBuffEntityR buff)
        {
            return this.buffList.Remove(buff);
        }
    }
}
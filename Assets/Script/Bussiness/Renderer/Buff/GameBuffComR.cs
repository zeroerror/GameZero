using System.Collections.Generic;
using GamePlay.Core;
namespace GamePlay.Bussiness.Renderer
{
    public class GameBuffComR
    {
        private List<GameBuffEntityR> _buffList;

        public GameBuffComR()
        {
            this._buffList = new List<GameBuffEntityR>();
        }

        public bool HasBuff(int buffId)
        {
            return this._buffList.Exists(buff => buff.model.typeId == buffId);
        }

        public bool TryGet(int buffId, out GameBuffEntityR buff)
        {
            buff = this._buffList.Find(b => b.model.typeId == buffId);
            return buff != null;
        }

        public void Add(GameBuffEntityR buff)
        {
            if (this.HasBuff(buff.model.typeId))
            {
                GameLogger.LogError("Buff已存在，无法重复添加：" + buff.model.typeId);
                return;
            }
            this._buffList.Add(buff);
        }

        public bool Remove(GameBuffEntityR buff)
        {
            return this._buffList.Remove(buff);
        }

        public void ForeachAllBuffs(System.Action<GameBuffEntityR> action)
        {
            this._buffList.ForEach(action);
        }
    }
}
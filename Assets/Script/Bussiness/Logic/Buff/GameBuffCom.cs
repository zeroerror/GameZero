using System.Collections.Generic;
using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameBuffCom
    {
        private List<GameBuffEntity> _buffList;

        public GameBuffCom()
        {
            this._buffList = new List<GameBuffEntity>();
        }

        public bool HasBuff(int buffId)
        {
            return this._buffList.Exists(buff => buff.model.typeId == buffId);
        }

        public bool TryGet(int buffId, out GameBuffEntity buff)
        {
            buff = this._buffList.Find(b => b.model.typeId == buffId);
            return buff != null;
        }

        public GameBuffEntity Get(int buffId)
        {
            return this._buffList.Find(b => b.model.typeId == buffId);
        }

        public void Add(GameBuffEntity buff)
        {
            if (this.HasBuff(buff.model.typeId))
            {
                GameLogger.LogError("Buff已存在，无法重复添加：" + buff.model.typeId);
                return;
            }
            this._buffList.Add(buff);
        }

        public bool Remove(GameBuffEntity buff)
        {
            return this._buffList.Remove(buff);
        }

        public void Foreach(System.Action<GameBuffEntity> action)
        {
            this._buffList.ForEach(action);
        }
    }
}
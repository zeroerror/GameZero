using System.Collections.Generic;
using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameBuffCom
    {
        public List<GameBuffEntity> buffList { get; private set; }

        public GameBuffCom()
        {
            this.buffList = new List<GameBuffEntity>();
        }

        public void Clear()
        {
            this.buffList.Clear();
        }

        public bool HasBuff(int buffId)
        {
            return this.buffList.Exists(buff => buff.model.typeId == buffId);
        }

        public bool TryGet(int buffId, out GameBuffEntity buff)
        {
            buff = this.buffList.Find(b => b.model.typeId == buffId);
            return buff != null;
        }

        public GameBuffEntity Get(int buffId)
        {
            return this.buffList.Find(b => b.model.typeId == buffId);
        }

        public void Add(GameBuffEntity buff)
        {
            if (this.HasBuff(buff.model.typeId))
            {
                GameLogger.LogError("Buff已存在，无法重复添加：" + buff.model.typeId);
                return;
            }
            this.buffList.Add(buff);
        }

        public bool Remove(GameBuffEntity buff)
        {
            return this.buffList.Remove(buff);
        }

        public void Foreach(System.Action<GameBuffEntity> action)
        {
            this.buffList.Foreach(action);
        }

        public List<GameBuffEntity> GetBuffList()
        {
            return this.buffList?.ToList();
        }
    }
}
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

        public bool HasBuff(int buffId)
        {
            return this.buffList.Exists(buff => buff.model.typeId == buffId);
        }

        public bool TryGet(int buffId, out GameBuffEntity buff)
        {
            buff = this.buffList.Find(b => b.model.typeId == buffId);
            return buff != null;
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
    }
}
using System.Collections.Generic;
using GamePlay.Core;
using UnityEngine.Analytics;
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

        public int DetachBuff(int buffId, int layer)
        {
            var buff = this.buffList.Find(b => b.model.typeId == buffId);
            if (!buff)
            {
                GameLogger.LogError("Buff不存在，无法移除：" + buffId);
                return 0;
            }
            if (!buff.isValid) return 0;
            var removeLayer = buff.DetachLayer(layer);
            if (!buff.isValid) this.buffList.Remove(buff);
            return removeLayer;
        }
    }
}
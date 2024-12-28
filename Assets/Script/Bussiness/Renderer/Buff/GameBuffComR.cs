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

        public void DetachBuff(int buffId, int layer)
        {
            var buff = this.buffList.Find(b => b.model.typeId == buffId);
            if (!buff)
            {
                GameLogger.LogError("Buff不存在，无法移除：" + buffId);
                return;
            }
            if (!buff.isValid) return;
            buff.DetachLayer(layer);
            if (!buff.isValid) this.buffList.Remove(buff);
        }
    }
}
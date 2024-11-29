using System.Collections.Generic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameVFXRepoR
    {
        protected Dictionary<int, GameVFXEntityR> _dict { get; } = new Dictionary<int, GameVFXEntityR>();
        protected List<GameVFXEntityR> _pool { get; } = new List<GameVFXEntityR>();

        public GameVFXRepoR()
        {
        }

        public void Clear()
        {
            this._dict.Clear();
        }

        public bool TryAdd(GameVFXEntityR entity)
        {
            GameLogger.Log($"VFX实体仓库添加: {entity.entityId}");
            return this._dict.TryAdd(entity.entityId, entity);
        }

        public void TryRemove(GameVFXEntityR entity)
        {
            this._dict.Remove(entity.entityId);
            this._Recycle(entity);
        }

        private void _Recycle(GameVFXEntityR entity)
        {
            this._pool.Add(entity);
        }

        public virtual bool TryFetch(out GameVFXEntityR entity)
        {
            entity = null;
            var pool = this._pool;
            if (pool.Count == 0) return false;
            var fetchIndex = pool.Count - 1;
            entity = pool[fetchIndex];
            pool.RemoveAt(fetchIndex);
            return true;
        }

        public GameVFXEntityR FindByEntityId(int entityId)
        {
            if (this._dict.TryGetValue(entityId, out GameVFXEntityR entity)) return entity;
            return null;
        }

        public void ForeachEntities(System.Action<GameVFXEntityR> action)
        {
            foreach (var entity in this._dict.Values)
            {
                action(entity);
            }
        }
    }
}
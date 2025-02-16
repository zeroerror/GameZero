using System.Collections.Generic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Render
{
    public class GameVFXRepoR
    {
        protected Dictionary<string, List<GameVFXEntityR>> _dict { get; } = new Dictionary<string, List<GameVFXEntityR>>();
        protected Dictionary<string, List<GameVFXEntityR>> _pool { get; } = new Dictionary<string, List<GameVFXEntityR>>();

        public GameVFXRepoR()
        {
        }

        public void Clear()
        {
            this._dict.Clear();
        }

        public bool TryAdd(GameVFXEntityR entity)
        {
            GameLogger.Log($"视觉特效仓库 添加: {entity.entityId} {entity.prefabUrl}");
            if (!this._dict.TryGetValue(entity.prefabUrl, out var list))
            {
                list = new List<GameVFXEntityR>();
                this._dict.Add(entity.prefabUrl, list);
            }
            list.Add(entity);
            return this._dict.TryAdd(entity.prefabUrl, list);
        }

        public void TryRemove(GameVFXEntityR entity)
        {
            if (!this._dict.TryGetValue(entity.prefabUrl, out var list)) return;
            list.Remove(entity);
            if (list.Count == 0) this._dict.Remove(entity.prefabUrl);
        }

        public void Recycle(GameVFXEntityR entity)
        {
            var list = this._dict[entity.prefabUrl];
            if (!list.Remove(entity)) return;
            GameLogger.Log($"视觉特效仓库 回收: {entity.entityId} {entity.prefabUrl}");
            entity.Stop();
            if (!this._pool.TryGetValue(entity.prefabUrl, out var pool))
            {
                pool = new List<GameVFXEntityR>();
                this._pool.Add(entity.prefabUrl, pool);
            }
            pool.Add(entity);
        }

        public virtual bool TryFetch(string url, out GameVFXEntityR entity)
        {
            if (!this._pool.TryGetValue(url, out var pool) || pool.Count == 0)
            {
                entity = null;
                return false;
            }

            entity = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
            GameLogger.Log($"视觉特效仓库 重用: {entity.entityId} {entity.prefabUrl}");
            return true;
        }

        public GameVFXEntityR FindByEntityId(int entityId)
        {
            foreach (var list in this._dict.Values)
            {
                foreach (var entity in list)
                {
                    if (entity.entityId == entityId) return entity;
                }
            }
            return null;
        }

        public void ForeachEntities(System.Action<GameVFXEntityR> action, bool includePool = false)
        {
            foreach (var list in this._dict.Values)
            {
                foreach (var entity in list)
                {
                    action(entity);
                }
            }

            if (includePool)
            {
                foreach (var list in this._pool.Values)
                {
                    foreach (var entity in list)
                    {
                        action(entity);
                    }
                }
            }
        }

    }
}
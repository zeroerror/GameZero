using System.Collections.Generic;
using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.Render
{
    public class GameVFXRepo
    {
        protected Dictionary<string, List<GameVFXEntity>> _dict { get; } = new Dictionary<string, List<GameVFXEntity>>();
        protected Dictionary<string, List<GameVFXEntity>> _pool { get; } = new Dictionary<string, List<GameVFXEntity>>();

        public GameVFXRepo()
        {
        }

        public void Clear()
        {
            this._dict.Clear();
        }

        public bool TryAdd(GameVFXEntity entity)
        {
            GameLogger.Log($"视觉特效仓库 添加: {entity.entityId} {entity.prefabUrl}");
            if (!this._dict.TryGetValue(entity.prefabUrl, out var list))
            {
                list = new List<GameVFXEntity>();
                this._dict.Add(entity.prefabUrl, list);
            }
            list.Add(entity);
            return this._dict.TryAdd(entity.prefabUrl, list);
        }

        public void TryRemove(GameVFXEntity entity)
        {
            if (!this._dict.TryGetValue(entity.prefabUrl, out var list)) return;
            list.Remove(entity);
            if (list.Count == 0) this._dict.Remove(entity.prefabUrl);
        }

        public void Recycle(GameVFXEntity entity)
        {
            var list = this._dict[entity.prefabUrl];
            if (!list.Remove(entity)) return;
            GameLogger.Log($"视觉特效仓库 回收: {entity.entityId} {entity.prefabUrl}");
            entity.Stop();
            if (!this._pool.TryGetValue(entity.prefabUrl, out var pool))
            {
                pool = new List<GameVFXEntity>();
                this._pool.Add(entity.prefabUrl, pool);
            }
            pool.Add(entity);
        }

        public virtual bool TryFetch(string url, out GameVFXEntity entity)
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

        public GameVFXEntity FindByEntityId(int entityId)
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

        public void ForeachEntities(System.Action<GameVFXEntity> action, bool includePool = false)
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
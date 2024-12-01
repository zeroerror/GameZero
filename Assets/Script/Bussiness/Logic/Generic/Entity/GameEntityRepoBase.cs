using System.Collections.Generic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public abstract class GameEntityRepoBase<T> where T : GameEntityBase
    {
        protected Dictionary<int, T> _dict { get; } = new Dictionary<int, T>();
        protected Dictionary<int, List<T>> _poolDict { get; } = new Dictionary<int, List<T>>();

        public GameEntityRepoBase()
        {
        }

        public virtual void Clear()
        {
            this._dict.Clear();
        }

        public virtual bool TryAdd(T entity)
        {
            var collider = entity.physicsCom.collider;
            if (collider != null) collider.isEnable = true;
            GameLogger.Log($"实体仓库 添加: {entity.idCom}");
            return this._dict.TryAdd(entity.idCom.entityId, entity);
        }

        public virtual void TryRemove(T entity)
        {
            var collider = entity.physicsCom.collider;
            if (collider != null) collider.isEnable = false;
            this._dict.Remove(entity.idCom.entityId, out entity);
            this._Recycle(entity);
        }

        private void _Recycle(T entity)
        {
            var typeId = entity.idCom.typeId;
            if (!this._poolDict.TryGetValue(typeId, out List<T> entityPool))
            {
                entityPool = new List<T>();
            }
            this._poolDict.Add(typeId, entityPool);
            entityPool.Add(entity);
            GameLogger.Log($"实体仓库 回收: {entity.idCom}");
            entity.Reset();
        }

        public virtual bool TryFetch(int typeId, out T entity)
        {
            entity = null;
            if (!this._poolDict.TryGetValue(typeId, out List<T> entityPool)) return false;
            if (entityPool.Count == 0) return false;
            var fetchIndex = entityPool.Count - 1;
            entity = entityPool[fetchIndex];
            entityPool.RemoveAt(fetchIndex);
            return true;
        }

        public virtual T FindByEntityId(int entityId)
        {
            if (this._dict.TryGetValue(entityId, out T entity)) return entity;
            return null;
        }

        public virtual void ForeachEntities(System.Action<T> action, bool isIncludingPool = false)
        {
            foreach (var entity in this._dict.Values)
            {
                action(entity);
            }
            if (isIncludingPool)
            {
                foreach (var entityPool in this._poolDict.Values)
                {
                    foreach (var entity in entityPool)
                    {
                        action(entity);
                    }
                }
            }
        }
    }
}
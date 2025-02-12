using System;
using System.Collections.Generic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public abstract class GameEntityRepoBase<T> where T : GameEntityBase
    {
        protected Dictionary<int, T> _dict { get; } = new Dictionary<int, T>();
        protected List<T> _list { get; } = new List<T>();
        protected Dictionary<int, List<T>> _poolDict { get; } = new Dictionary<int, List<T>>();
        public int Count => this._list.Count;

        public GameEntityRepoBase()
        {
        }

        public virtual void Clear()
        {
            this._dict.Clear();
            this._list.Clear();
            this._poolDict.Clear();
        }

        public virtual bool TryAdd(T entity)
        {
            var collider = entity.physicsCom.collider;
            if (collider != null) collider.isEnable = true;
            entity.SetValid();
            GameLogger.Log($"实体仓库 添加: {entity.idCom}");
            return this._TryAddData(entity);
        }

        private bool _TryAddData(T entity)
        {
            var key = this._GetKey(entity);
            if (this._dict.ContainsKey(key))
            {
                GameLogger.LogError($"实体仓库 添加失败: {entity.idCom} 已存在");
                return false;
            }
            this._dict.Add(key, entity);
            this._list.Add(entity);
            return true;
        }

        protected virtual int _GetKey(T entity)
        {
            return entity.idCom.entityId;
        }

        /// <summary> 将实体从仓库永久移除 </summary>
        public virtual bool TryRemove(T entity)
        {
            var collider = entity.physicsCom.collider;
            if (collider != null) collider.isEnable = false;
            if (this._TryRemoveData(entity))
            {
                entity.Destroy();
                GameLogger.Log($"实体仓库 移除: {entity.idCom}");
                return true;
            }
            return false;
        }

        private bool _TryRemoveData(T entity)
        {
            var key = this._GetKey(entity);
            if (!this._dict.Remove(key, out entity)) return false;
            this._list.Remove(entity);
            return true;
        }

        /// <summary> 回收实体到仓库对象池 </summary>
        public void Recycle(T entity)
        {
            if (!this._TryRemoveData(entity)) return;
            GameLogger.Log($"实体仓库 回收: {entity.idCom}");
            var collider = entity.physicsCom.collider;
            if (collider != null) collider.isEnable = false;

            var typeId = entity.idCom.typeId;
            if (!this._poolDict.TryGetValue(typeId, out List<T> entityPool))
            {
                entityPool = new List<T>();
                this._poolDict.Add(typeId, entityPool);
            }
            entityPool.Add(entity);
            entity.Clear();
            entity.SetInvalid();
        }

        public virtual bool TryFetch(int typeId, out T entity)
        {
            entity = null;
            if (!this._poolDict.TryGetValue(typeId, out List<T> pool)) return false;
            if (pool.Count == 0) return false;
            var fetchIndex = pool.Count - 1;
            entity = pool[fetchIndex];
            pool.RemoveAt(fetchIndex);
            entity.SetValid();
            return true;
        }

        public virtual T FindByEntityId(int entityId)
        {
            if (this._dict.TryGetValue(entityId, out T entity)) return entity;
            return null;
        }

        public virtual List<T> FindAll(Predicate<T> predicate)
        {
            return this._list.FindAll(predicate);
        }

        public virtual T Find(Predicate<T> predicate)
        {
            return this._list.Find(predicate);
        }

        public virtual bool TryFindByEntityId(int entityId, out T entity)
        {
            return this._dict.TryGetValue(entityId, out entity);
        }

        public virtual void ForeachEntities(Action<T> action)
        {
            var count = this._list.Count;
            for (var i = 0; i < count; i++)
            {
                action(this._list[i]);
            }
        }

        public virtual void ForeachEntities(Action<T, int> action)
        {
            var count = this._list.Count;
            for (var i = 0; i < count; i++)
            {
                action(this._list[i], i);
            }
        }

        public virtual void ForeachEntities_IncludePool(Action<T> action)
        {
            for (var i = 0; i < this._list.Count; i++)
            {
                action(this._list[i]);
            }
            foreach (var entityPool in this._poolDict.Values)
            {
                foreach (var entity in entityPool)
                {
                    action(entity);
                }
            }
        }

        public int GetEntityCount(Predicate<T> predicate)
        {
            return this._list.FindAll(predicate).Count;
        }

        public List<T> ToList()
        {
            return new List<T>(this._list);
        }
    }
}
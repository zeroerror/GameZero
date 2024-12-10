using System.Collections.Generic;
using GamePlay.Core;
using UnityEngine.SocialPlatforms.Impl;

namespace GamePlay.Bussiness.Logic
{
    public abstract class GameEntityRepoBase<T> where T : GameEntityBase
    {
        protected Dictionary<int, T> _dict { get; } = new Dictionary<int, T>();
        protected List<T> _list { get; } = new List<T>();

        protected Dictionary<int, List<T>> _poolDict { get; } = new Dictionary<int, List<T>>();

        public GameEntityRepoBase()
        {
        }

        public virtual void Clear()
        {
            this._dict.Clear();
            this._list.Clear();
        }

        public virtual bool TryAdd(T entity)
        {
            var collider = entity.physicsCom.collider;
            if (collider != null) collider.isEnable = true;
            GameLogger.Log($"实体仓库 添加: {entity.idCom}");
            return this._TryAddData(entity);
        }

        private bool _TryAddData(T entity)
        {
            if (this._dict.ContainsKey(entity.idCom.entityId))
            {
                GameLogger.LogError($"实体仓库 添加失败: {entity.idCom}");
                return false;
            }
            this._dict.Add(entity.idCom.entityId, entity);
            this._list.Add(entity);
            return true;
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
            if (!this._dict.Remove(entity.idCom.entityId, out entity)) return false;
            this._list.Remove(entity);
            return true;
        }

        /// <summary> 回收实体到仓库对象池 </summary>
        public void Recycle(T entity)
        {
            if (!this._TryRemoveData(entity)) return;
            var collider = entity.physicsCom.collider;
            if (collider != null) collider.isEnable = false;

            var typeId = entity.idCom.typeId;
            if (!this._poolDict.TryGetValue(typeId, out List<T> entityPool))
            {
                entityPool = new List<T>();
                this._poolDict.Add(typeId, entityPool);
            }
            entityPool.Add(entity);
            GameLogger.Log($"实体仓库 回收: {entity.idCom}");
            entity.Clear();
        }

        public virtual bool TryFetch(int typeId, out T entity)
        {
            entity = null;
            if (!this._poolDict.TryGetValue(typeId, out List<T> pool)) return false;
            if (pool.Count == 0) return false;
            var fetchIndex = pool.Count - 1;
            entity = pool[fetchIndex];
            pool.RemoveAt(fetchIndex);
            return true;
        }

        public virtual T FindByEntityId(int entityId)
        {
            if (this._dict.TryGetValue(entityId, out T entity)) return entity;
            return null;
        }

        public virtual void ForeachEntities(System.Action<T> action)
        {
            var count = this._list.Count;
            for (var i = 0; i < count; i++)
            {
                action(this._list[i]);
            }
        }

        public virtual void ForeachAllEntities(System.Action<T> action)
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
    }
}
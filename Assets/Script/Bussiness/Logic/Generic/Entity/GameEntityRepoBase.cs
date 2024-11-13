using System.Collections.Generic;
namespace GamePlay.Bussiness.Logic
{
    public abstract class GameEntityRepoBase<T> where T : GameEntityBase
    {
        protected Dictionary<int, T> _entityDict { get; } = new Dictionary<int, T>();

        public GameEntityRepoBase()
        {
        }

        public virtual void Clear()
        {
            this._entityDict.Clear();
        }

        public virtual bool TryAdd(T entity)
        {
            return this._entityDict.TryAdd(entity.idCom.entityId, entity);
        }

        public virtual void TryRemove(T entity)
        {
            this._entityDict.Remove(entity.idCom.entityId, out entity);
        }

        public virtual T FindByEntityId(int entityId)
        {
            if (this._entityDict.TryGetValue(entityId, out T entity)) return entity;
            return null;
        }

        public virtual void ForeachEntities(System.Action<T> action)
        {
            foreach (var entity in this._entityDict.Values)
            {
                action(entity);
            }
        }
    }
}
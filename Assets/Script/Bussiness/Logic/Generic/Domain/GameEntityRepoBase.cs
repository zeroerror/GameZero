using System.Collections.Generic;
namespace GamePlay.Bussiness.Logic
{
    public abstract class GameEntityRepoBase
    {
        public abstract Dictionary<int, GameEntity> entityDict { get; }

        public GameEntityRepoBase() { }

        public virtual void Clear()
        {
            this.entityDict.Clear();
        }

        public virtual bool TryAdd(GameEntity entity)
        {
            return this.entityDict.TryAdd(entity.idCom.entityId, entity);
        }

        public virtual void TryRemove(GameEntity entity)
        {
            this.entityDict.Remove(entity.idCom.entityId, out entity);
        }

        public virtual GameEntity FindByEntityId(int entityId)
        {
            if (this.entityDict.TryGetValue(entityId, out GameEntity entity)) return entity;
            return null;
        }
    }
}
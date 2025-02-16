using System.Collections.Generic;

namespace GamePlay.Bussiness.Render
{
    public class GameShaderEffectRepo
    {
        private List<GameShaderEffectEntity> _list = new List<GameShaderEffectEntity>();
        private List<GameShaderEffectEntity> _pool = new List<GameShaderEffectEntity>();

        public GameShaderEffectRepo()
        {
        }

        public virtual void Clear()
        {
            this._list.Clear();
            this._pool.Clear();
        }

        public void Add(GameShaderEffectEntity entity)
        {
            this._list.Add(entity);
        }

        public bool TryFetch(int typeId, out GameShaderEffectEntity entity)
        {
            entity = null;
            for (int i = 0; i < this._pool.Count; i++)
            {
                if (this._pool[i].model.typeId == typeId)
                {
                    entity = this._pool[i];
                    this._pool.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public void Recycle(GameShaderEffectEntity entity)
        {
            entity.Stop();
            this._pool.Add(entity);
            this._list.Remove(entity);
        }

        public void Foreach(System.Action<GameShaderEffectEntity> action)
        {
            for (int i = 0; i < this._list.Count; i++)
            {
                action(this._list[i]);
            }
        }
    }
}
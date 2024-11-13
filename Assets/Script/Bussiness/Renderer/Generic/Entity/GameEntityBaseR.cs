namespace GamePlay.Bussiness.Logic
{
    public abstract class GameEntityBaseR
    {

        public bool isValid() => idCom.entityId > 0;
        public int bodyColliderId { get; private set; }
        public GameIdCom idCom { get; private set; }
        public GameTransformCom transformCom { get; private set; }
        public GameActionTargeterCom actionTargeterCom { get; private set; }

        public GameEntityBaseR(int typeId, GameEntityType entityType)
        {
            idCom = new GameIdCom(typeId, entityType);
            transformCom = new GameTransformCom();
            actionTargeterCom = new GameActionTargeterCom();
        }

        public void Reset()
        {
            idCom.Reset();
            transformCom.Reset();
        }

        public bool IsEquals(GameEntityBaseR other)
        {
            return idCom.IsEquals(other.idCom);
        }

        public T TryGetLinkEntity<T>() where T : GameEntityBaseR
        {
            if (this is T)
            {
                return this as T;
            }

            var p = idCom.parent;
            while (p != null)
            {
                if (p is T)
                {
                    return p as T;
                }
                p = p.idCom.parent;
            }
            return null;
        }

        public virtual void Tick(float dt) { }
        public abstract void Reset(float dt);
    }
}

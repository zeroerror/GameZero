namespace GamePlay.Bussiness.Logic
{
    public abstract class GameEntityBase
    {
        public bool isValid { get; private set; } = true;
        public void SetValid(bool isValid) => this.isValid = isValid;

        public GameIdCom idCom { get; private set; }
        public GameTransformCom transformCom { get; private set; }
        public void BindTransformCom(GameTransformCom transformCom) => this.transformCom = transformCom;
        public GameActionTargeterCom actionTargeterCom { get; private set; }
        public GamePhysicsCom physicsCom { get; private set; }
        public GameAttributeCom attributeCom { get; private set; }

        public GameEntityBase(int typeId, GameEntityType entityType)
        {
            idCom = new GameIdCom(typeId, entityType);
            transformCom = new GameTransformCom();
            actionTargeterCom = new GameActionTargeterCom();
            physicsCom = new GamePhysicsCom();
            attributeCom = new GameAttributeCom();
        }

        public virtual void Clear()
        {
            idCom.Clear();
            transformCom.Clear();
            actionTargeterCom.Clear();
            physicsCom.Clear();
            attributeCom.Clear();
            this.isValid = true;
        }

        public bool IsEquals(GameEntityBase other)
        {
            return idCom.IsEquals(other.idCom);
        }

        public T TryGetLinkEntity<T>() where T : GameEntityBase
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

        public abstract void Tick(float dt);
        public abstract void Dispose();

    }
}
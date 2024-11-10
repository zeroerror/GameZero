namespace Game.Bussiness
{
    public class GameEntity
    {
        public bool IsValid()
        {
            return identityComponent.entityId > 0;
        }

        public GameIdentityComponent identityComponent { get; private set; }
        public GameTransformComponent transformComponent { get; private set; }
        public int bodyColliderId { get; private set; }
        public GameActionTargeterComponent actionTargeterCom { get; private set; }


        public GameEntity()
        {
            identityComponent = new GameIdentityComponent();
            transformComponent = new GameTransformComponent();
            actionTargeterCom = new GameActionTargeterComponent();
        }

        public void Reset()
        {
            identityComponent.Reset();
            transformComponent.Reset();
        }


        public bool IsSelectable()
        {
            return true;
        }

        public bool IsEquals(GameEntity other)
        {
            return identityComponent.IsEquals(other.identityComponent);
        }

        public T TryGetLinkEntity<T>() where T : GameEntity
        {
            if (this is T)
            {
                return this as T;
            }

            var p = identityComponent.parent;
            while (p != null)
            {
                if (p is T)
                {
                    return p as T;
                }
                p = p.identityComponent.parent;
            }
            return null;
        }
    }
}

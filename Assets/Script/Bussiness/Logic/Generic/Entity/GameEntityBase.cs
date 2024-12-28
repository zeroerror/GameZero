using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Logic
{
    public abstract class GameEntityBase
    {
        public static bool operator !(GameEntityBase entity) => entity == null;
        public static bool operator true(GameEntityBase entity) => entity != null;
        public static bool operator false(GameEntityBase entity) => entity == null;

        public bool isValid { get; private set; } = true;
        public void SetValid() => this.isValid = true;
        public void SetInvalid() => this.isValid = false;

        public bool HasReference() => this.idCom.children.Count > 0;

        public GameIdCom idCom { get; private set; }
        public GameTransformCom transformCom { get; private set; }
        public void BindTransformCom(GameTransformCom transformCom) => this.transformCom = transformCom;
        public GameActionTargeterCom actionTargeterCom { get; private set; }
        public GamePhysicsCom physicsCom { get; private set; }
        public GameAttributeCom attributeCom { get; private set; }
        public void BindAttributeCom(GameAttributeCom attributeCom) => this.attributeCom = attributeCom;
        public GameAttributeCom baseAttributeCom { get; private set; }
        public void BindBaseAttributeCom(GameAttributeCom baseAttributeCom) => this.baseAttributeCom = baseAttributeCom;

        public GameEntityBase(int typeId, GameEntityType entityType)
        {
            idCom = new GameIdCom(typeId, entityType);
            transformCom = new GameTransformCom();
            actionTargeterCom = new GameActionTargeterCom();
            physicsCom = new GamePhysicsCom();
            attributeCom = new GameAttributeCom();
            baseAttributeCom = new GameAttributeCom();
        }

        public virtual void Clear()
        {
            idCom?.Clear();
            transformCom?.Clear();
            actionTargeterCom?.Clear();
            physicsCom?.Clear();
            attributeCom?.Clear();
            baseAttributeCom?.Clear();
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

        public bool TryGetLinkEntity<T>(out T entity) where T : GameEntityBase
        {
            entity = TryGetLinkEntity<T>();
            return entity != null;
        }

        public abstract void Tick(float dt);
        public abstract void Destroy();

        public GameVec2 logicCenterPos
        {
            get
            {
                var pos = transformCom.position;
                if (this.physicsCom.collider is GameBoxCollider boxCollider)
                {
                    pos.y += boxCollider.worldHeight * 0.5f;
                }
                return pos;
            }
        }

        public GameVec2 logicBottomPos => this.transformCom.position;

        public bool IsAlive()
        {
            if (!this.isValid) return false;
            if (this.attributeCom.GetValue(GameAttributeType.HP) <= 0) return false;
            return true;
        }

    }
}
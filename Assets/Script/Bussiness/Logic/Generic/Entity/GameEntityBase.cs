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
        public virtual void SetValid() => this.isValid = true;
        public virtual void SetInvalid()
        {
            this.isValid = false;
        }

        public bool HasReference() => this.idCom.children.Count > 0;

        public GameIdCom idCom { get; private set; }
        public GameTransformCom transformCom { get; private set; }
        public virtual GameVec2 GetPosition() => this.transformCom.position;
        public virtual void SetPosition(in GameVec2 pos) => this.transformCom.position = pos;

        public GameActionTargeterCom actionTargeterCom { get; private set; }
        public GamePhysicsCom physicsCom { get; private set; }
        public GameAttributeCom attributeCom { get; private set; }
        public GameAttributeCom baseAttributeCom { get; private set; }

        public GameEntityBase(int typeId, GameEntityType entityType)
        {
            idCom = new GameIdCom(typeId, entityType, this);
            transformCom = new GameTransformCom();
            actionTargeterCom = new GameActionTargeterCom();
            physicsCom = new GamePhysicsCom();
            attributeCom = new GameAttributeCom();
            baseAttributeCom = new GameAttributeCom();
        }

        public virtual void Clear()
        {
            idCom?.Clear();
            actionTargeterCom?.Clear();
            physicsCom?.Clear();

            // 不对绑定组件重复清理
            if (this._bindedTransformCom == null) transformCom?.Clear();
            if (this._bindedAttributeCom == null) attributeCom?.Clear();
            if (this._bindedBaseAttributeCom == null) baseAttributeCom?.Clear();
        }

        public bool IsEquals(GameEntityBase other)
        {
            return idCom.IsEquals(other.idCom);
        }

        /// <summary>
        /// 获取指定类型的父辈实体, 会追溯到根实体
        /// </summary>
        public T GetLinkParent<T>() where T : GameEntityBase
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

        public bool TryGetLinkParent<T>(out T entity) where T : GameEntityBase
        {
            entity = GetLinkParent<T>();
            return entity != null;
        }

        /// <summary>
        /// 获取指定类型的子实体
        /// </summary>
        public T TryGetLinkChild<T>() where T : GameEntityBase
        {
            if (this is T)
            {
                return this as T;
            }

            var c = idCom.children;
            foreach (var child in c)
            {
                if (child is T)
                {
                    return child as T;
                }
            }
            return null;
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

        public virtual bool IsAlive()
        {
            return this.isValid;
        }

        public void BindAttributeCom(GameAttributeCom attributeCom)
        {
            this.attributeCom = attributeCom;
            this._bindedAttributeCom = attributeCom;
        }
        private GameAttributeCom _bindedAttributeCom;

        public void BindBaseAttributeCom(GameAttributeCom baseAttributeCom)
        {
            this.baseAttributeCom = baseAttributeCom;
            this._bindedBaseAttributeCom = baseAttributeCom;
        }
        private GameAttributeCom _bindedBaseAttributeCom;

        public void BindTransformCom(GameTransformCom transformCom)
        {
            this.transformCom = transformCom;
            this._bindedTransformCom = transformCom;
        }
        private GameTransformCom _bindedTransformCom;
    }
}
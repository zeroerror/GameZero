using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public class GameIdCom
    {
        public readonly GameEntityBase entity;

        public int typeId { get; private set; }
        public GameEntityType entityType { get; private set; }

        public int entityId { get; private set; }
        public void SetEntityId(int entityId) => this.entityId = entityId;

        public GameEntityBase parent { get; private set; }
        public List<GameEntityBase> children = new List<GameEntityBase>();

        public int campId;

        public GameIdCom(int typeId, GameEntityType entityType, GameEntityBase entity)
        {
            this.typeId = typeId;
            this.entityType = entityType;
            this.entity = entity;
        }

        public void Clear()
        {
            // 对parent解除引用
            if (parent != null)
            {
                parent.idCom.children.Remove(parent);
                parent = null;
            }

            entityId = 0;
            campId = 0;
            children.Clear();
        }

        public override string ToString()
        {
            return $"实体类型: {GameEntityTypeExtension.ToString(entityType)} 实体Id: {entityId} 阵营ID: {campId}";
        }

        // 判断是否相等
        public bool IsEquals(GameIdCom other)
        {
            return entityType == other.entityType && entityId == other.entityId;
        }

        // 设置父实体
        public void SetParent(GameEntityBase parent)
        {
            if (this.parent == parent)
            {
                return;
            }

            // 对旧的parent解除引用
            var old = this.parent;
            if (old != null)
            {
                old.idCom.children.Remove(old);
            }

            // 对新的parent引用
            this.parent = parent;
            if (parent != null)
            {
                var parentIdCom = parent.idCom;
                campId = parentIdCom.campId;
                parentIdCom.children.Add(this.entity);
            }
        }

        public GameIdArgs ToArgs()
        {
            return new GameIdArgs
            {
                typeId = typeId,
                entityType = entityType,
                entityId = entityId,
                campId = campId
            };
        }

        public void SetByArgs(in GameIdArgs args)
        {
            typeId = args.typeId;
            entityType = args.entityType;
            entityId = args.entityId;
            campId = args.campId;
        }

        public bool CheckCampType(GameIdCom idComB, GameCampType campType)
        {
            switch (campType)
            {
                case GameCampType.None:
                    return true;
                case GameCampType.Enemy:
                    return this.campId != idComB.campId;
                case GameCampType.Ally:
                    return this.campId == idComB.campId;
                case GameCampType.Neutral:
                    return this.campId == 0 || idComB.campId == 0;
                default:
                    return false;
            }
        }
    }
}

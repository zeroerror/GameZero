using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public class GameIdCom
    {
        public int typeId { get; private set; }
        public GameEntityType entityType { get; private set; }
        public int entityId { get; set; }

        public GameEntityBase parent { get; private set; }
        public int campId;

        public GameIdCom(int typeId, GameEntityType entityType)
        {
            this.typeId = typeId;
            this.entityType = entityType;
        }

        public void Clear()
        {
            entityId = 0;
            parent = null;
            campId = 0;
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
            this.parent = parent;
            if (parent != null)
            {
                var parentIdCom = parent.idCom;
                campId = parentIdCom.campId;
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
    }
}

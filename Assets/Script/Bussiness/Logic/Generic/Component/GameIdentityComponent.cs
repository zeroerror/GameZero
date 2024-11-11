using System;

namespace GamePlay.Bussiness.Logic
{
    public class GameIdentityComponent
    {
        public int typeId { get; private set; }
        public GameEntityType entityType { get; private set; }
        public int entityId { get; private set; }
        public GameEntity parent { get; private set; }
        public int campId { get; private set; }

        public GameIdentityComponent()
        {
            Reset();
        }

        public void Reset()
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
        public bool IsEquals(GameIdentityComponent other)
        {
            return entityType == other.entityType && entityId == other.entityId;
        }

        // 设置父实体
        public void SetParent(GameEntity parent)
        {
            this.parent = parent;
            if (parent != null)
            {
                var parentIdCom = parent.identityComponent;
                campId = parentIdCom.campId;
            }
        }

    }
}

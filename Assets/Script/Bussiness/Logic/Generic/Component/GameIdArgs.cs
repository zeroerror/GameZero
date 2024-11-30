using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public struct GameIdArgs
    {
        public int typeId;
        public GameEntityType entityType;
        public int entityId;
        public int campId;

        public override string ToString()
        {
            return $"类型Id:{typeId}, 实体类型:{entityType}, 实体Id:{entityId}, 阵营Id:{campId}";
        }
    }
}

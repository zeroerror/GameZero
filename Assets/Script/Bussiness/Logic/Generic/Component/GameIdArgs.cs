namespace GamePlay.Bussiness.Logic
{
    public struct GameIdArgs
    {
        public int typeId;
        public GameEntityType entityType;
        public int entityId;
        public int campId;

        public GameIdArgs(int typeId, GameEntityType entityType, int entityId, int campId)
        {
            this.typeId = typeId;
            this.entityType = entityType;
            this.entityId = entityId;
            this.campId = campId;
        }

        public override string ToString()
        {
            return $"类型Id:{typeId}, 实体类型:{entityType}, 实体Id:{entityId}, 阵营Id:{campId}";
        }
    }
}

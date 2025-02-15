namespace GamePlay.Bussiness.Logic
{
    public class GameItemUnitModel
    {
        /// <summary> 实体类型 </summary>
        public readonly GameEntityType entityType;
        /// <summary> 类型Id </summary>
        public readonly int typeId;
        /// <summary> 消耗金币 </summary>
        public readonly int costGold;

        public GameItemUnitModel(GameEntityType entityType, int typeId, int costGold)
        {
            this.entityType = entityType;
            this.typeId = typeId;
            this.costGold = costGold;
        }

        public override string ToString()
        {
            return $"实体类型: {this.entityType} 类型Id: {this.typeId} 消耗金币: {this.costGold}";
        }

        public GameItemUnitModel Clone()
        {
            return new GameItemUnitModel(
                this.entityType,
                this.typeId,
                this.costGold
            );
        }
    }
}

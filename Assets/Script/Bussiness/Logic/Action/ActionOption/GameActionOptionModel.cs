namespace GamePlay.Bussiness.Logic
{
    public class GameActionOptionModel
    {
        /// <summary> 类型Id </summary>
        public int typeId;
        /// <summary> 品质 </summary>
        public readonly GameActionOptionQuality quality;
        /// <summary> 最大等级 </summary>
        public readonly int maxLv;
        /// <summary> 行为Id列表 </summary>
        public readonly int[] actionIds;

        public GameActionOptionModel(int typeId, GameActionOptionQuality quality, int maxLv, int[] actionIds)
        {
            this.typeId = typeId;
            this.quality = quality;
            this.maxLv = maxLv;
            this.actionIds = actionIds;
        }
    }
}
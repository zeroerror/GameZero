namespace GamePlay.Bussiness.Logic
{
    public class GameActionOptionModel
    {
        /// <summary> 类型Id </summary>
        public readonly int typeId;
        /// <summary> 描述 </summary>
        public readonly string desc;
        /// <summary> 品质 </summary>
        public readonly GameActionOptionQuality quality;
        /// <summary> 最大等级 </summary>
        public readonly int maxLv;
        /// <summary> 行为Id列表 </summary>
        public readonly int[] actionIds;

        /// <summary> 升级金币消耗列表 </summary>
        public int[] upgradeCosts;

        public GameActionOptionModel(int typeId, string desc, GameActionOptionQuality quality, int maxLv, int[] actionIds, int[] upgradeCosts)
        {
            this.typeId = typeId;
            this.desc = desc;
            this.quality = quality;
            this.maxLv = maxLv;
            this.actionIds = actionIds;
            this.upgradeCosts = upgradeCosts;
        }
    }
}
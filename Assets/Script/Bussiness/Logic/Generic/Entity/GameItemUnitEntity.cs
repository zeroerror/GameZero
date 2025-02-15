using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameItemUnitEntity
    {
        /// <summary> 单位物品模型 </summary>
        public GameItemUnitModel unitModel;
        /// <summary> 实体Id </summary>
        public int entityId;
        /// <summary> 属性参数 </summary>
        public GameAttributeArgs attributeArgs;
        /// <summary> 基础属性参数 </summary>
        public GameAttributeArgs baseAttributeArgs;

        public GameItemUnitEntity()
        {
            this.itemid = ++_autoItemId;
        }

        public int itemid { get; private set; }
        private static int _autoItemId;

    }
}
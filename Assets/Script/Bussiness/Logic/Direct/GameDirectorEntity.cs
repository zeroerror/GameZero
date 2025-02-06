using System.Collections.Generic;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorEntity
    {
        public GameDirectorFSMCom fsmCom { get; private set; }
        public GameDirectorTimelineComponent timeScaleCom { get; private set; }

        /// <summary> 单位牌库 </summary>
        public List<GamePlayUnitModel> unitPool { get; private set; }
        public void SetUnitPool(List<GamePlayUnitModel> unitPool)
        {
            this.unitPool = unitPool;
        }

        /// <summary> 拥有的金币 </summary>
        public int coins;
        /// <summary> 已选择的行动选项 </summary>
        public List<GameActionOptionModel> actionOptions;
        /// <summary> 拥有的单位实体 </summary>
        public List<GamePlayUnitEntity> unitEntitys;
        /// <summary> 当前购买栏单位列表 </summary>
        public List<GamePlayUnitModel> buyableUnits;

        public GameDirectorEntity()
        {
            this.fsmCom = new GameDirectorFSMCom();
            this.timeScaleCom = new GameDirectorTimelineComponent();
            this.coins = 0;
            this.actionOptions = new List<GameActionOptionModel>();
            this.unitEntitys = new List<GamePlayUnitEntity>();
            this.buyableUnits = new List<GamePlayUnitModel>();
        }

        public int Tick(float dt)
        {
            var tickCount = this.timeScaleCom.Tick(dt);
            return tickCount;
        }
    }

    public class GamePlayUnitEntity
    {
        /// <summary> 模型 </summary>
        public GamePlayUnitModel model;
        /// <summary> 实体Id </summary>
        public int entityId;
        /// <summary> 属性参数 </summary>
        public GameAttributeArgs attributeArgs;
        /// <summary> 基础属性参数 </summary>
        public GameAttributeArgs baseAttributeArgs;
        /// <summary> 位置 </summary>
        public GameVec2 position;
    }

    public class GamePlayUnitModel
    {
        /// <summary> 实体类型 </summary>
        public GameEntityType entityType;
        /// <summary> 类型Id </summary>
        public int typeId;
        /// <summary> 消耗金币 </summary>
        public int costCoins;

        public override string ToString()
        {
            return $"实体类型: {this.entityType} 类型Id: {this.typeId} 消耗金币: {this.costCoins}";
        }
    }
}
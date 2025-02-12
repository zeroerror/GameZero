using System.Collections.Generic;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorEntity
    {
        public GameDirectorFSMCom fsmCom { get; private set; }
        public GameDirectorTimelineComponent timeScaleCom { get; private set; }

        /// <summary> 单位牌库 </summary>
        public List<GameItemUnitModel> unitPool { get; private set; }
        public void SetUnitPool(List<GameItemUnitModel> unitPool)
        {
            this.unitPool = unitPool;
        }

        /// <summary> 拥有的金币 </summary>
        public int gold
        {
            get => this._gold;
            set
            {
                if (value == this._gold) return;
                this._gold = value;
                this.goldDirty = true;
            }
        }
        private int _gold;
        public bool goldDirty;

        /// <summary> 已选择的行动选项 </summary>
        public List<GameActionOptionModel> actionOptions;
        /// <summary> 拥有的单位实体 </summary>
        public List<GameItemUnitEntity> itemUnitEntitys;
        /// <summary> 当前购买栏单位列表 </summary>
        public List<GameItemUnitModel> buyableUnits;

        /// <summary> 当前回合 </summary>
        public int curRound;

        public GameDirectorEntity()
        {
            this.fsmCom = new GameDirectorFSMCom();
            this.timeScaleCom = new GameDirectorTimelineComponent();
            this.gold = 10000;//初始金币
            this.actionOptions = new List<GameActionOptionModel>();
            this.itemUnitEntitys = new List<GameItemUnitEntity>();
            this.buyableUnits = new List<GameItemUnitModel>();
        }

        public int Tick(float dt)
        {
            var tickCount = this.timeScaleCom.Tick(dt);
            return tickCount;
        }
    }

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
        /// <summary> 站位 </summary>
        public GameVec2 standPos
        {
            get => this._standPos;
            set
            {
                if (value == this._standPos) return;
                this._standPos = value;
                this.needPlaceBack = true;
            }
        }
        private GameVec2 _standPos;
        /// <summary> 是否需要归位 </summary>
        public bool needPlaceBack;

        public GameItemUnitEntity()
        {
            this.itemid = ++_autoItemId;
        }

        public int itemid { get; private set; }
        private static int _autoItemId;

    }

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
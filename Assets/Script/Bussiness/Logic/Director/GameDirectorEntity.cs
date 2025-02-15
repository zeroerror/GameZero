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
}
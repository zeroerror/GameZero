using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    [System.Serializable]
    public class GameEntitySelector
    {
        /// <summary> 选择锚点类型 </summary>
        public GameEntitySelectAnchorType selectAnchorType;
        /// <summary> 阵营类型 </summary>
        public GameCampType campType;
        /// <summary> 实体类型 </summary>
        public GameEntityType entityType;
        /// <summary> 是否仅选择死亡单位 TODO 改成角色状态条件</summary>
        public bool onlySelectDead;

        /// <summary> 范围选取模型 </summary>
        public GameColliderModelBase rangeSelectModel;
        /// <summary> 范围选取限制数量(0代表不限制) </summary>
        public int rangeSelectLimitCount;
        /// <summary> 范围选取排序方式 </summary>
        public GameEntitySelectSortType rangeSelectSortType;

        /// <summary>
        /// 判定单个实体是否满足选择
        /// entityA: 实体A，主动选择方
        /// entityB: 实体B，被选择方
        /// </summary>
        public bool CheckSelect(GameEntityBase entityA, GameEntityBase entityB)
        {
            if (!entityA || !entityB) return false;
            // 判定阵营
            if (!entityA.idCom.CheckCampType(entityB.idCom, campType)) return false;
            // 判定实体类型 ps: none默认通过
            if (this.entityType != GameEntityType.None && entityB.idCom.entityType != this.entityType) return false;
            // 判定锚点类型
            if (this.rangeSelectModel == null)
            {
                var isSelf = entityA.idCom.IsEquals(entityB.idCom);
                if (isSelf && !this.selectAnchorType.HasFlag(GameEntitySelectAnchorType.Actor)) return false;
                if (!isSelf && !this.selectAnchorType.HasFlag(GameEntitySelectAnchorType.ActTarget)) return false;
            }
            // 判断是否仅选择死亡单位
            if (this.onlySelectDead && entityB.IsAlive()) return false;
            return true;
        }

        /// <summary> 是否是范围选择 </summary>
        public bool IsRangeSelect() => this.rangeSelectModel != null;
    }
}
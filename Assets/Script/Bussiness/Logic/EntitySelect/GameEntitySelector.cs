using System.Runtime.Serialization.Formatters;
using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    [System.Serializable]
    public class GameEntitySelector
    {
        // 选择锚点类型
        public GameEntitySelectAnchorType selectAnchorType;
        // 阵营类型
        public GameCampType campType;
        // 实体类型
        public GameEntityType entityType;
        // 碰撞模型
        public GameColliderModelBase colliderModel;

        public bool isRangeSelect => this.colliderModel != null;

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
            // 判定实体类型
            if (this.entityType != GameEntityType.None && entityB.idCom.entityType != this.entityType) return false;
            // 判定锚点类型
            if (this.colliderModel == null)
            {
                var isSelf = entityA.idCom.IsEquals(entityB.idCom);
                if (isSelf && !this.selectAnchorType.HasFlag(GameEntitySelectAnchorType.Actor)) return false;
                if (!isSelf && !this.selectAnchorType.HasFlag(GameEntitySelectAnchorType.ActTarget)) return false;
            }
            return true;
        }


    }
}
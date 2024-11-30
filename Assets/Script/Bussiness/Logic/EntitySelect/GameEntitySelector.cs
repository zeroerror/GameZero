using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    [System.Serializable]
    public struct GameEntitySelector
    {
        // 选择锚点类型
        public GameEntitySelectAnchorType selectAnchorType;
        // 阵营类型
        public GameCampType campType;
        // 实体类型
        public GameEntityType entityType;
        // 碰撞模型
        public GameColliderModelBase colliderModel;

        /// <summary>
        /// 判定单个实体是否满足选择
        /// entityA: 实体A，主动选择方
        /// entityB: 实体B，被选择方
        /// </summary>
        public bool CheckSelect(GameEntityBase entityA, GameEntityBase entityB)
        {
            // 判定阵营
            if (!this.CheckCampType(entityA, entityB)) return false;
            // 判定实体类型
            if (this.entityType != GameEntityType.None && entityB.idCom.entityType != this.entityType) return false;
            // 判定锚点类型
            if (this.colliderModel == null)
            {
                var isSelf = entityA.idCom.entityId == entityB.idCom.entityId;
                if (isSelf && !this.selectAnchorType.HasFlag(GameEntitySelectAnchorType.Self)) return false;
                if (!isSelf && !this.selectAnchorType.HasFlag(GameEntitySelectAnchorType.Target)) return false;
            }
            return true;
        }

        public bool CheckCampType(GameEntityBase entityA, GameEntityBase entityB)
        {
            switch (this.campType)
            {
                case GameCampType.None:
                    return true;
                case GameCampType.Enemy:
                    return entityA.idCom.campId != entityB.idCom.campId;
                case GameCampType.Ally:
                    return entityA.idCom.campId == entityB.idCom.campId;
                case GameCampType.Neutral:
                    return entityA.idCom.campId == 0 || entityB.idCom.campId == 0;
                default:
                    return false;
            }
        }
    }
}
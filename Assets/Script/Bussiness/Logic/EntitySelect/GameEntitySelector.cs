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
        /// <para> actor: 行为者 </para>
        /// <para> target: 目标 </para>
        /// </summary>
        public bool CheckSelect(GameEntityBase actor, GameEntityBase target)
        {
            if (!actor || !target) return false;
            // 判定阵营
            if (!actor.idCom.CheckCampType(target.idCom, campType)) return false;
            // 判定实体类型 ps: none默认通过
            if (this.entityType != GameEntityType.None && target.idCom.entityType != this.entityType) return false;
            // 判定锚点类型
            var isSingleSelect = this.rangeSelectModel == null;
            if (isSingleSelect)
            {
                switch (this.selectAnchorType)
                {
                    case GameEntitySelectAnchorType.Actor:
                        if (!actor.idCom.IsEquals(target.idCom)) return false;
                        break;
                    case GameEntitySelectAnchorType.ActTarget:
                        var actTarget = actor.actionTargeterCom.targetEntity;
                        if (!actTarget || !actTarget.idCom.IsEquals(target.idCom)) return false;
                        break;
                    case GameEntitySelectAnchorType.ActorRole:
                        var actorRole = actor.GetLinkParent<GameRoleEntity>();
                        if (!actorRole || !actorRole.idCom.IsEquals(target.idCom)) return false;
                        break;
                }
            }
            // 判断是否仅选择死亡单位
            if (this.onlySelectDead && target.IsAlive()) return false;
            return true;
        }

    }
}
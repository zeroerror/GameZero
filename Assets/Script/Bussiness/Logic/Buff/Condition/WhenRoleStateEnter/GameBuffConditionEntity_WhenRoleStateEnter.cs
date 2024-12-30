using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// buff条件实体 - 当死亡时(即亡语)
    /// <para>当满足以下任意条件时，都算作满足条件</para>
    /// </summary>
    public class GameBuffConditionEntity_WhenRoleStateEnter : GameBuffConditionEntityBase
    {
        public GameBuffConditionModel_WhenRoleStateEnter model { get; private set; }

        public GameBuffConditionEntity_WhenRoleStateEnter(GameBuffEntity buff, GameBuffConditionModel_WhenRoleStateEnter model) : base(buff)
        {
            this.model = model;
        }

        protected override bool _Check()
        {
            var conditionCheck = false;
            List<GameActionTargeterArgs> targeterList = null;
            this.ForeachRoleStateRecord((stateRecord) =>
            {
                // buff目标不是角色, 跳过
                var target = this._buff.target;
                if (target.idCom.entityType != GameEntityType.Role) return;
                // 非存活状态跳过
                var role = this.FindEntity(GameEntityType.Role, stateRecord.entityId) as GameRoleEntity;
                if (!role || !role.IsAlive()) return;
                // 检查是否满足条件
                var campType = this.model.campType;
                var isTargetSelf = campType == GameCampType.None;
                if (isTargetSelf)
                {
                    if (stateRecord.entityId == target.idCom.entityId) m_check();
                }
                else
                {
                    if (role.idCom.CheckCampType(target.idCom, campType)) m_check();
                }

                void m_check()
                {
                    conditionCheck = true;
                    if (targeterList == null) targeterList = new List<GameActionTargeterArgs>();
                    var targeter = new GameActionTargeterArgs(
                        role,
                        role.transformCom.position,
                        (role.transformCom.position - target.transformCom.position).normalized
                    );
                    targeterList.Add(targeter);
                    return;
                }
            });

            // 满足时, 同步目标选取器列表到buff 
            if (targeterList != null)
            {
                this._buff.actionTargeterCom.SetTargeterList(targeterList);
                this._buff.actionTargeterCom.foreachType = GameForeachType.Sequential;
            }

            return conditionCheck;
        }
    }
}
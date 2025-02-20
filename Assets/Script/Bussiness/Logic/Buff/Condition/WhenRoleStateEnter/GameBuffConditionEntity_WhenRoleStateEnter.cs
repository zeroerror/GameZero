using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// buff条件实体 - 当角色状态进入时
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
            List<GameActionTargeterArgs> targeterList = null;
            this.ForeachRoleStateRecord((stateRecord) =>
            {
                var role = this.FindEntity(GameEntityType.Role, stateRecord.entityId) as GameRoleEntity;
                if (!role) return;
                // 检查 状态类型
                if (stateRecord.stateType != this.model.stateType) return;
                // 检查 施法状态下的技能类型
                if (stateRecord.stateType == GameRoleStateType.Cast && stateRecord.skillType != this.model.skillType) return;
                // 检查 阵营
                var campType = this.model.campType;
                var isTargetSelf = campType == GameCampType.None;
                var target = this._buff.owner;
                if (isTargetSelf)
                {
                    if (stateRecord.entityId == target.idCom.entityId) _setSatisfied();
                }
                else
                {
                    if (role.idCom.CheckCampType(target.idCom, campType)) _setSatisfied();
                }

                void _setSatisfied()
                {
                    if (targeterList == null) targeterList = new List<GameActionTargeterArgs>();
                    var targeter = new GameActionTargeterArgs(
                        role,
                        role.transformCom.position,
                        (target.transformCom.position - role.transformCom.position).normalized
                    );
                    targeterList.Add(targeter);
                    return;
                }
            });

            // 不满足时, 返回false
            if (targeterList == null)
            {
                return false;
            }

            // 满足时, 同步目标选取器列表到buff 
            this._buff.actionTargeterCom.SetTargeterList(targeterList);
            this._buff.actionTargeterCom.foreachType = GameForeachType.Sequential;
            return true;
        }
    }
}
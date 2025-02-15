using System.Collections.Generic;
using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// 通用的攻击AI逻辑
    /// </summary>
    public class GameRoleAIDomain_Attack
    {
        GameContext _context;
        GameRoleContext _roleContext => this._context.roleContext;

        public GameRoleAIDomain_Attack()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public void Destroy()
        {
        }

        public void Tick(GameRoleEntity role, float dt)
        {
            var attackState = role.aiCom.attackState;
            var inputCom = role.inputCom;

            // 已有输入, 重置攻击状态
            if (inputCom.HasInput())
            {
                attackState.Clear();
                return;
            }

            // 有激活跟随AI时, 远离时切换至跟随AI状态
            if (role.aiCom.followState.isFarAway())
            {
                this._context.domainApi.roleAIApi.TryEnter(role, GameRoleAIStateType.Follow);
                return;
            }

            // 选择施法目标和技能
            this._ChooseCastTargetAndSkill(role);

            // 没有施法目标或技能, 切换至Idle状态
            var castTarget = attackState.castTarget;
            var castSkill = attackState.castSkill;
            if (castTarget == null || castSkill == null)
            {
                return;
            }

            // 抵达了技能选取的范围, 设置施法输入
            var conditionModel = castSkill.skillModel.conditionModel;
            var selector = conditionModel.selector;
            if (
                conditionModel.targeterType == GameSkillTargterType.Actor ||
                GamePhysicsResolvingUtil.CheckOverlap(selector.rangeSelectModel, role.transformCom.ToArgs(), castTarget.transformCom.position)
            )
            {
                this._SetInput_Cast(role, castSkill, castTarget);
                return;
            }

            // 未抵达施法距离, 设置移动输入
            this._SetInput_Move(role, castSkill, castTarget);
            return;
        }

        private void _ChooseCastTargetAndSkill(GameRoleEntity role)
        {
            // 先记录先前的施法目标
            var attackState = role.aiCom.attackState;
            var oldCastTarget = attackState.castTarget;

            // 清空状态参数
            attackState.castTarget = null;
            attackState.castSkill = null;

            // 如果当前目标有效, 判定对当前目标是否存在可施法技能, 如果存在则直接返回
            if (!!oldCastTarget && this._IsValidTarget(role, oldCastTarget))
            {
                var castSkill = this._context.domainApi.skillApi.FindCastableSkill(role, oldCastTarget);
                if (castSkill)
                {
                    attackState.castTarget = oldCastTarget;
                    attackState.castSkill = castSkill;
                    return;
                }
            }

            // 重新选择施法目标和技能
            var repo = this._roleContext.repo;
            var minDisSqr = float.MaxValue;
            repo.ForeachEntities((castTarget) =>
            {
                if (!this._IsValidTarget(role, castTarget)) return;

                // 对当前目标不存在可施法技能, 跳过
                var castSkill = this._context.domainApi.skillApi.FindCastableSkill(role, castTarget);
                if (castSkill == null) return;

                // 距离越近, 优先级越高
                var curDisSqr = castTarget.logicBottomPos.GetDisSqr(role.logicBottomPos);
                if (curDisSqr < minDisSqr)
                {
                    minDisSqr = curDisSqr;
                    attackState.castTarget = castTarget;
                    attackState.castSkill = castSkill;
                }
            });
        }

        private bool _IsValidTarget(GameEntityBase self, GameEntityBase target)
        {
            // 目标为隐身状态的敌对阵营角色
            if (target is GameRoleEntity targetRole)
            {
                var isStealthEnemy = targetRole.idCom.CheckCampType(self.idCom, GameCampType.Enemy) && targetRole.fsmCom.stateType == GameRoleStateType.Stealth;
                if (isStealthEnemy) return false;
            }
            // 目标已死亡
            if (!target.IsAlive()) return false;
            return true;
        }

        private void _SetInput_Cast(GameRoleEntity role, GameSkillEntity castSkill, GameEntityBase castTarget)
        {
            var args = new GameRoleInputArgs
            {
                skillId = castSkill.skillModel.typeId,
                targeterArgsList = new List<GameActionTargeterArgs>
                    {
                        new GameActionTargeterArgs
                        {
                            targetEntity = castTarget,
                            targetDirection = ( castTarget.logicCenterPos - role.logicCenterPos).normalized,
                            targetPosition = castTarget.logicBottomPos,
                        }
                    }
            };
            role.inputCom.SetByArgs(args);
        }

        private void _SetInput_Move(GameRoleEntity role, GameSkillEntity castSkill, GameEntityBase castTarget)
        {
            var colliderModel = castSkill.skillModel.conditionModel.selector.rangeSelectModel;
            var contactMTV = GamePhysicsResolvingUtil.GetContactMTV(colliderModel, castSkill.transformCom.ToArgs(), castTarget.transformCom.position);
            contactMTV += contactMTV.normalized * 0.01f;
            var moveDst = role.logicBottomPos - contactMTV;
            role.inputCom.moveDst = moveDst;
        }

    }
}
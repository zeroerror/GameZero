using System.Collections.Generic;
using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
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
                this._context.domainApi.roleApi.aiApi.TryEnter(role, GameRoleAIStateType.Follow);
                return;
            }

            // 选择施法目标和技能
            this._ChooseCastTargetAndSkill(role);

            // 没有施法目标或技能, 切换至Idle状态
            var castTarget = attackState.castTarget;
            var castSkill = attackState.castSkill;
            if (castTarget == null || castSkill == null)
            {
                this._context.domainApi.roleApi.fsmApi.TryEnter(role, GameRoleStateType.Idle);
                return;
            }

            // 抵达施法距离, 设置施法输入 ps: 类似操控角色到达施法距离后输入施法指令
            var colliderModel = castSkill.skillModel.conditionModel.selector.colliderModel;
            var mtv = GamePhysicsResolvingUtil.GetResolvingMTV(colliderModel, role.transformCom.ToArgs(), castTarget.transformCom.position);
            var hasReached = mtv != GameVec2.zero;
            if (hasReached)
            {
                this._SetCastInput(role, castSkill, castTarget);
                return;
            }

            // 没有抵达有效施法距离, 输入移动
            var args = new GameRoleInputArgs
            {
                skillId = castSkill.skillModel.typeId,
                targeterArgsList = new List<GameActionTargeterArgs>
                    {
                        new GameActionTargeterArgs
                        {
                            targetEntity = castTarget,
                            targetDirection = ( castTarget.logicCenterPos - role.transformCom.position).normalized,
                            targetPosition = castTarget.logicBottomPos,
                        }
                    }
            };
            inputCom.SetByArgs(args);
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
            if (!!oldCastTarget && oldCastTarget.IsAlive())
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
                if (!castTarget.IsAlive()) return;

                // 对当前目标不存在可施法技能, 跳过
                var castSkill = this._context.domainApi.skillApi.FindCastableSkill(role, castTarget);
                if (castSkill == null) return;

                // 检查施法条件
                var inputArgs = new GameRoleInputArgs
                {
                    skillId = castSkill.skillModel.typeId,
                    targeterArgsList = new List<GameActionTargeterArgs>
                    {
                            new GameActionTargeterArgs
                            {
                                targetEntity = castTarget,
                                targetDirection = (castTarget.logicCenterPos - role.transformCom.position).normalized,
                                targetPosition = castTarget.logicBottomPos,
                            }
                    }
                };
                var isConditionSatisfied = this._context.domainApi.skillApi.CheckCastCondition(role, castSkill, inputArgs);
                if (!isConditionSatisfied) return;

                var curDisSqr = castTarget.transformCom.position.GetDisSqr(role.transformCom.position);
                if (curDisSqr < minDisSqr)
                {
                    minDisSqr = curDisSqr;
                    attackState.castTarget = castTarget;
                    attackState.castSkill = castSkill;
                }
            });
        }

        private void _SetCastInput(GameRoleEntity role, GameSkillEntity castSkill, GameEntityBase castTarget)
        {
            var colliderModel = castSkill.skillModel.conditionModel.selector.colliderModel;
            var contactMTV = GamePhysicsResolvingUtil.GetContactMTV(colliderModel, castSkill.transformCom.ToArgs(), castTarget.transformCom.position);
            var moveDir = contactMTV.normalized.Neg();
            role.inputCom.moveDir = moveDir;
        }

    }
}
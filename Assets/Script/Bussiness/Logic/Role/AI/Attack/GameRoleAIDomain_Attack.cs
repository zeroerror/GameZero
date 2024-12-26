using System.Collections.Generic;
using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
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
            if (inputCom.HasInput())
            {
                // 已有输入, 重置攻击状态
                attackState.Clear();
                return;
            }

            if (role.aiCom.followState.isFarAway())
            {
                // 有激活跟随AI时, 远离时切换至跟随AI状态
                this._context.domainApi.roleApi.aiApi.TryEnter(role, GameRoleAIStateType.Follow);
                return;
            }

            var castTarget = attackState.targetEntity;
            var skillCom = role.skillCom;
            GameSkillEntity castSkill = null;
            // 目标存活, 不寻找新的目标
            if (castTarget == null || !castTarget.isValid || castTarget.attributeCom.GetValue(GameAttributeType.HP) <= 0)
            {
                castTarget = null;

                var repo = this._roleContext.repo;
                var minDisSqr = float.MaxValue;
                repo.ForeachEntities((curTar) =>
                {
                    if (curTar == role)
                    {
                        return;
                    }
                    if (curTar.attributeCom.GetValue(GameAttributeType.HP) <= 0)
                    {
                        return;
                    }

                    // 判定角色是否可被选中
                    var curSkill = skillCom.FindWithPriority((skill) =>
                        {
                            return this._context.domainApi.skillApi.CheckSkillCondition(role, skill, curTar);
                        });
                    if (curSkill == null) return;

                    // 判定技能条件
                    var inputArgs = new GameRoleInputArgs
                    {
                        skillId = curSkill.skillModel.typeId,
                        targeterArgsList = new List<GameActionTargeterArgs>
                        {
                            new GameActionTargeterArgs
                            {
                                targetEntity = curTar,
                                targetDirection = (curTar.logicCenterPos - role.transformCom.position).normalized,
                                targetPosition = curTar.logicBottomPos,
                            }
                        }
                    };
                    var isConditionSatisfied = this._context.domainApi.skillApi.CheckCastCondition(role, curSkill, inputArgs);
                    if (!isConditionSatisfied) return;

                    var curDisSqr = curTar.transformCom.position.GetDisSqr(role.transformCom.position);
                    if (curDisSqr < minDisSqr)
                    {
                        minDisSqr = curDisSqr;
                        castTarget = curTar;
                        castSkill = curSkill;
                    }
                });
                attackState.targetEntity = castTarget;
            }

            if (castTarget == null)
            {
                this._context.domainApi.roleApi.fsmApi.TryEnter(role, GameRoleStateType.Idle);
                return;
            }

            if (castSkill == null)
            {
                castSkill = skillCom.Find((skill) =>
                      {
                          var skillModel = skill.skillModel;
                          var sel = skillModel.conditionModel.selector;
                          return sel.CheckSelect(skill, castTarget);
                      });
            }
            if (castSkill == null)
            {
                return;
            }

            // 判定追击所需距离
            var selector = castSkill.skillModel.conditionModel.selector;
            var colliderModel = selector.colliderModel;
            var mtv = GamePhysicsResolvingUtil.GetResolvingMTV(colliderModel, role.transformCom.ToArgs(), castTarget.transformCom.position);
            var hasReached = mtv != GameVec2.zero;
            if (hasReached)
            {
                // 攻击
                var tarPos = castTarget.logicCenterPos;
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

            var contactMTV = GamePhysicsResolvingUtil.GetContactMTV(colliderModel, castSkill.transformCom.ToArgs(), castTarget.transformCom.position);
            var moveDir = contactMTV.normalized.Neg();
            inputCom.moveDir = moveDir;
        }

    }
}
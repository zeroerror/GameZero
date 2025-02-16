using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Logic
{
    public class GameSkillDomain : GameSkillDomainApi
    {
        GameContext _context;
        GameSkillContext _skillContext => this._context.skillContext;

        public GameSkillDomain()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public void Destroy()
        {
        }

        public void Tick(float dt)
        {
            var skillCom = this._skillContext.repo;
            skillCom.ForeachEntities((skill) =>
            {
                skill.Tick(dt);
            });
        }

        public bool TryGetModel(int typeId, out GameSkillModel model)
        {
            return this._skillContext.factory.template.TryGet(typeId, out model);
        }

        public GameSkillEntity CreateSkill(GameRoleEntity role, int typeId)
        {
            var skillCom = role.skillCom;
            var skill = this._CreateSkill(role, typeId, skillCom);
            // 提交RC
            this._context.SubmitRC(GameSkillRCCollection.RC_GAMES_SKILL_CREATE,
                new GameSkillRCArgs_Create { roleIdArgs = role.idCom.ToArgs(), skillIdArgs = skill.idCom.ToArgs() }
            );
            return skill;
        }

        private GameSkillEntity _CreateSkill(GameRoleEntity role, int typeId, GameSkillCom skillCom)
        {
            if (skillCom.TryGet(typeId, out var _))
            {
                GameLogger.LogError("技能创建失败，技能已存在：" + typeId);
                return null;
            }

            var repo = this._skillContext.repo;
            if (!repo.TryFetch(typeId, out var skill)) skill = this._skillContext.factory.Load(typeId);
            if (skill == null)
            {
                GameLogger.LogError("技能创建失败，技能ID不存在：" + typeId);
                return null;
            }

            skill.idCom.entityId = this._skillContext.idService.FetchId();
            // 绑定父子关系
            skill.idCom.SetParent(role);
            // 组件绑定
            skill.BindTransformCom(role.transformCom);
            skill.BindAttributeCom(role.attributeCom);
            skill.BindBaseAttributeCom(role.baseAttributeCom);
            // 为主动技能添加时间轴事件
            var skillModel = skill.skillModel;
            if (skillModel.skillType != GameSkillType.Passive)
            {
                var timelineEvModels = skillModel.timelineEvModels;
                var actionApi = this._context.domainApi.actionApi;
                timelineEvModels?.Foreach((evModel, index) =>
                {
                    skill.timelineCom.AddEventByFrame(evModel.frame, () =>
                    {
                        skill.physicsCom.ClearCollided();
                        evModel.actionIds?.Foreach((actionId) =>
                        {
                            actionApi.DoAction(actionId, skill);
                        });
                    });
                });
            }
            skillCom.Add(skill);
            repo.TryAdd(skill);
            return skill;
        }

        public bool CheckRoleCastCondition(GameRoleEntity role, GameSkillEntity skill, in GameRoleInputArgs inputArgs, bool ignoreDistanceCondition = false)
        {
            var errCode = this._CheckRoleCastCondition(role, skill, in inputArgs, ignoreDistanceCondition);
            return errCode == 0;
        }

        private int _CheckRoleCastCondition(GameRoleEntity role, GameSkillEntity skill, in GameRoleInputArgs inputArgs, bool ignoreDistanceCondition = false)
        {
            var fsmCom = role.fsmCom;
            var stateType = fsmCom.stateType;
            if (stateType == GameRoleStateType.Cast) return 1;

            // 输入检测
            var skillModel = skill.skillModel;
            switch (skillModel.conditionModel.targeterType)
            {
                case GameSkillTargterType.Enemy:
                    var index = inputArgs.targeterArgsList.FindIndex((args) => args.targetEntity != null && args.targetEntity.idCom.campId != role.idCom.campId);
                    if (index == -1)
                    {
                        return 2;
                    }
                    break;
                case GameSkillTargterType.Direction:
                    var index_d = inputArgs.targeterArgsList.FindIndex((args) => !args.targetDirection.Equals(GameVec2.zero));
                    if (index_d == -1)
                    {
                        return 3;
                    }
                    break;
                case GameSkillTargterType.Position:
                    var index_p = inputArgs.targeterArgsList.FindIndex((args) => !args.targetPosition.Equals(GameVec2.zero));
                    if (index_p == -1)
                    {
                        return 4;
                    }
                    break;
                case GameSkillTargterType.Actor:
                    break;
                default:
                    GameLogger.LogError("未知的技能目标类型");
                    return 5;
            }

            // 技能条件检测
            var firstTarget = inputArgs.targeterArgsList?[0].targetEntity;
            if (!this.CheckSkillCondition(role, skill, firstTarget, ignoreDistanceCondition)) return 6;

            // 目标为隐身状态的敌对阵营角色
            if (firstTarget is GameRoleEntity targetRole)
            {
                if (targetRole.idCom.CheckCampType(role.idCom, GameCampType.Enemy) && targetRole.fsmCom.stateType == GameRoleStateType.Stealth) return 7;
            }

            return 0;
        }

        public bool CheckCastCondition(GameRoleEntity role, GameSkillEntity skill)
        {
            var inputArgs = role.inputCom.ToArgs();
            var errCode = this._CheckRoleCastCondition(role, skill, in inputArgs);
            if (errCode != 0)
            {
                GameLogger.LogWarning($"技能施法条件检测失败：{skill.idCom} 错误码：{errCode}");
                return false;
            }
            return true;
        }

        public bool CheckSkillCondition(GameRoleEntity role, GameSkillEntity skill, GameEntityBase target, bool ignoreDistanceCondition = false)
        {
            var conditionModel = skill.skillModel.conditionModel;
            if (conditionModel == null) return true;
            // 检查 - CD
            if (skill.cdElapsed > 0) return false;
            // 检查 - 属性消耗
            if (role.attributeCom.GetValue(GameAttributeType.MP) < conditionModel.mpCost) return false;
            // 检查 - 选择器
            var selector = conditionModel.selector;
            if (!selector.CheckSelect(skill, target)) return false;
            // 检查 - 范围
            if (
                !ignoreDistanceCondition &&
                conditionModel.targeterType != GameSkillTargterType.Actor &&
                !GamePhysicsResolvingUtil.CheckOverlap(selector.rangeSelectModel, skill.transformCom.ToArgs(), target.logicBottomPos)
            )
            {
                return false;
            }
            return true;
        }

        public void CastSkill(GameRoleEntity role, GameSkillEntity skill)
        {
            this._calcSkillCost(role, skill);
            skill.timelineCom.Play();
            skill.actionTargeterCom.SetTargeterList(role.inputCom.targeterArgsList);
        }

        private void _calcSkillCost(GameRoleEntity role, GameSkillEntity skill)
        {
            var conditionModel = skill.skillModel.conditionModel;
            // CD
            skill.cdElapsed = conditionModel.cdTime;
            // 蓝量
            var mp = role.attributeCom.GetValue(GameAttributeType.MP);
            mp -= conditionModel.mpCost;
            mp = mp < 0 ? 0 : mp;
            var attr = new GameAttribute() { type = GameAttributeType.MP, value = mp };
            role.attributeCom.SetAttribute(attr);
        }

        public GameSkillEntity FindCastableSkill(GameRoleEntity role, GameEntityBase target, bool ignoreDistanceCondition = true)
        {
            if (!target.IsAlive()) return null;// 非存活目标, 不可施法 TODO： 除非是复活类技能
            return role.skillCom.FindWithPriority((skill) =>
            {
                if (skill.skillModel.skillType == GameSkillType.Passive) return false;
                return this.CheckSkillCondition(role, skill, target, ignoreDistanceCondition);
            });
        }

        public void DoPassiveSkill(GameRoleEntity role)
        {
            var skillIds = role.model.skillIds;
            if (skillIds == null) return;
            skillIds.Foreach((skillId) =>
            {
                if (!this._skillContext.factory.template.TryGet(skillId, out var skillModel))
                {
                    GameLogger.LogError("被动技能执行失败，技能ID不存在：" + skillId);
                    return;
                }
                if (skillModel.skillType != GameSkillType.Passive) return;
                var passiveSkill = role.skillCom.Find((s) => s.skillModel.typeId == skillId);
                if (!passiveSkill)
                {
                    GameLogger.LogError("被动技能执行失败，技能未找到：" + skillId);
                    return;
                }
                // 直接执行被动技能的所有action
                skillModel.timelineEvModels?.Foreach((evModel, index) =>
                {
                    evModel.actionIds?.Foreach((actionId) =>
                    {
                        this._context.domainApi.actionApi.DoAction(actionId, passiveSkill);
                    });
                });
            });
        }
    }
}

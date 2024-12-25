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
        }

        public GameSkillEntity CreateSkill(GameRoleEntity role, int typeId)
        {
            var skillCom = role.skillCom;
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

            skill.idCom.SetEntityId(this._skillContext.idService.FetchId());
            // 绑定父子关系
            skill.idCom.SetParent(role);
            // 组件绑定
            skill.BindTransformCom(role.transformCom);
            skill.BindAttributeCom(role.attributeCom);
            skill.BindBaseAttributeCom(role.baseAttributeCom);
            // 添加时间轴事件
            var skillModel = skill.skillModel;
            var timelineEvModels = skillModel.timelineEvModels;
            var actionApi = this._context.domainApi.actionApi;
            timelineEvModels?.Foreach((evModel, index) =>
            {
                skill.timelineCom.AddEventByFrame(evModel.frame, () =>
                {
                    evModel.actionIds?.Foreach((actionId) =>
                    {
                        actionApi.DoAction(actionId, skill);
                    });
                });
            });
            skillCom.Add(skill);
            repo.TryAdd(skill);

            // 提交RC
            this._context.SubmitRC(GameSkillRCCollection.RC_GAMES_SKILL_CREATE,
                new GameSkillRCArgs_Create { roleIdArgs = role.idCom.ToArgs(), skillIdArgs = skill.idCom.ToArgs() }
            );
            return skill;
        }

        public GameSkillModel GetSkillModel(int typeId)
        {
            var factory = this._skillContext.factory;
            if (!factory.template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("技能模板不存在：" + typeId);
                return null;
            }
            return model;
        }

        public bool CheckCastCondition(GameRoleEntity role, GameSkillEntity skill)
        {
            var inputArgs = role.inputCom.ToArgs();
            return this.CheckCastCondition(role, skill, in inputArgs);
        }

        public bool CheckCastCondition(GameRoleEntity role, GameSkillEntity skill, in GameRoleInputArgs inputArgs)
        {
            var fsmCom = role.fsmCom;
            var stateType = fsmCom.stateType;
            if (stateType == GameRoleStateType.Cast) return false;

            // 输入检测
            var skillModel = skill.skillModel;
            switch (skillModel.conditionModel.targeterType)
            {
                case GameSkillTargterType.Enemy:
                    var index = inputArgs.targeterArgsList.FindIndex((args) => args.targetEntity != null && args.targetEntity.idCom.campId != role.idCom.campId);
                    if (index == -1)
                    {
                        GameLogger.LogWarning($"[{skillModel.typeId}]技能释放条件无法满足！ 需要存在目标敌人");
                        return false;
                    }
                    break;
                case GameSkillTargterType.Direction:
                    var index_d = inputArgs.targeterArgsList.FindIndex((args) => args.targetDirection != GameVec2.zero);
                    if (index_d == -1)
                    {
                        GameLogger.LogWarning($"[{skillModel.typeId}]技能释放条件无法满足！ 需要存在目标方向");
                        return false;
                    }
                    break;
                case GameSkillTargterType.Position:
                    var index_p = inputArgs.targeterArgsList.FindIndex((args) => args.targetPosition != GameVec2.zero);
                    if (index_p == -1)
                    {
                        GameLogger.LogWarning($"[{skillModel.typeId}]技能释放条件无法满足！ 需要存在目标位置");
                        return false;
                    }
                    break;
                case GameSkillTargterType.Actor:
                    break;
                default:
                    GameLogger.LogError("未知的技能目标类型");
                    return false;
            }
            // 消耗检测
            // CD
            if (skill.cdElapsed > 0) return false;
            // 蓝量
            var mp = role.attributeCom.GetValue(GameAttributeType.MP);
            if (mp < skill.skillModel.conditionModel.mpCost) return false;
            // 人物状态，比如眩晕，沉默等
            return true;
        }

        public void CastSkill(GameRoleEntity role, GameSkillEntity skill)
        {
            this._calcSkillCost(role, skill);
            skill.timelineCom.Play();
            skill.actionTargeterCom.SetTargeterList(role.inputCom.targeterArgsList);
            skill.physicsCom.ClearCollided();
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

        public bool TryGetModel(int typeId, out GameSkillModel model)
        {
            return this._skillContext.factory.template.TryGet(typeId, out model);
        }
    }
}

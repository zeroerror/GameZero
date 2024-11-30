using GamePlay.Core;

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

        public void Dispose()
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

            skill.idCom.entityId = this._skillContext.entityIdService.FetchId();
            // 绑定父子关系
            skill.idCom.SetParent(role);
            // 绑定TransformCom为角色TransformCom
            skill.BindTransformCom(role.transformCom);
            // 添加时间轴事件
            var skillModel = skill.skillModel;
            var timelineEvModels = skillModel.timelineEvModels;
            var actionApi = this._context.domainApi.actionApi;
            timelineEvModels?.Foreach((evModel, index) =>
            {
                skill.timelineCom.AddEventByFrame(evModel.frame, () =>
                {
                    actionApi.DoAction(evModel.actionId, skill);
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
    }
}

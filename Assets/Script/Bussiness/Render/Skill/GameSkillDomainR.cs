using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public class GameSkillDomainR : GameSkillDomainApiR
    {
        GameContextR _context;
        GameSkillContextR _skillContext => this._context.skillContext;

        public GameSkillDomainR()
        {
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
        }

        public void Destroy()
        {
        }

        public void BindEvents()
        {
            this._context.BindRC(GameSkillRCCollection.RC_GAMES_SKILL_CREATE, this._OnSkillCreate);
        }

        public void UnbindEvents()
        {
            this._context.UnbindRC(GameSkillRCCollection.RC_GAMES_SKILL_CREATE, this._OnSkillCreate);
        }

        public void Tick(float dt) { }

        private void _OnSkillCreate(object args)
        {
            var rcArgs = (GameSkillRCArgs_Create)args;
            var roleIdArgs = rcArgs.roleIdArgs;
            // 检查角色异步
            var role = this._context.roleContext.repo.FindByEntityId(roleIdArgs.entityId);
            if (role == null)
            {
                this._context.DelayRC(GameSkillRCCollection.RC_GAMES_SKILL_CREATE, args);
                return;
            }
            var skillIdArgs = rcArgs.skillIdArgs;
            this.CreateSkill(role, skillIdArgs);
        }

        private GameSkillEntityR _CreateSkill(GameRoleEntityR role, in GameIdArgs skillIdArgs, GameSkillComR skillCom)
        {
            var typeId = skillIdArgs.typeId;
            if (skillCom.TryGet(typeId, out var _))
            {
                GameLogger.LogError("技能创建失败，技能已存在：" + typeId);
                return null;
            }

            var repo = this._skillContext.repo;
            if (!repo.TryFetch(skillIdArgs.entityId, out var skill)) skill = this._skillContext.factory.Load(typeId);
            if (skill == null)
            {
                GameLogger.LogError("技能创建失败，技能ID不存在：" + typeId);
                return null;
            }

            skill.idCom.SetByArgs(skillIdArgs);
            skillCom.Add(skill);

            // 绑定TransformCom为角色TransformCom
            skill.BindTransformCom(role.transformCom);

            repo.TryAdd(skill);
            return skill;
        }

        public GameSkillEntityR CreateSkill(GameRoleEntityR role, in GameIdArgs skillIdArgs)
        {
            var skill = this._CreateSkill(role, skillIdArgs, role.skillCom);
            return skill;
        }
    }
}

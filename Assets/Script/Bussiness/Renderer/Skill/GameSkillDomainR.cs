using System.Runtime.CompilerServices;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
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
            this._BindEvent();
        }

        public void Destroy()
        {
            this._UnbindEvents();
        }

        private void _BindEvent()
        {
            this._context.BindRC(GameSkillRCCollection.RC_GAMES_SKILL_CREATE, this._OnSkillCreate);
            this._context.BindRC(GameSkillRCCollection.RC_GAMES_SKILL_TRANSFORM, this._OnSkillTransform);
        }

        private void _UnbindEvents()
        {
            this._context.UnbindRC(GameSkillRCCollection.RC_GAMES_SKILL_CREATE, this._OnSkillCreate);
            this._context.UnbindRC(GameSkillRCCollection.RC_GAMES_SKILL_TRANSFORM, this._OnSkillTransform);
        }

        public void Tick(float dt) { }

        private void _OnSkillCreate(object args)
        {
            var evArgs = (GameSkillRCArgs_Create)args;
            var roleIdArgs = evArgs.roleIdArgs;
            // 检查角色异步
            var role = this._context.roleContext.repo.FindByEntityId(roleIdArgs.entityId);
            if (role == null)
            {
                this._context.DelayRC(GameSkillRCCollection.RC_GAMES_SKILL_CREATE, args);
                return;
            }
            var skillIdArgs = evArgs.skillIdArgs;
            this.CreateSkill(role, skillIdArgs);
        }

        private GameSkillEntityR _CreateSkill(GameRoleEntityR role, in GameIdArgs skillIdArgs, GameSkillComR skillCom)
        {
            var typeId = skillIdArgs.typeId;
            if (skillCom.TryGet(typeId, out var skill))
            {
                GameLogger.LogError("技能创建失败，技能已存在：" + typeId);
                return null;
            }

            var repo = this._skillContext.repo;
            if (!repo.TryFetch(skillIdArgs.entityId, out skill)) skill = this._skillContext.factory.Load(typeId);
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

        public void _OnSkillTransform(object args)
        {
            var evArgs = (GameSkillRCArgs_CharacterTransform)args;
            var roleIdArgs = evArgs.roleIdArgs;
            // 检查角色异步
            var role = this._context.roleContext.repo.FindByEntityId(roleIdArgs.entityId);
            if (role == null)
            {
                this._context.DelayRC(GameSkillRCCollection.RC_GAMES_SKILL_TRANSFORM, args);
                return;
            }
            var skillIdArgsList = evArgs.skillIdArgsList;
            this._CharacterTransformSkill(role, skillIdArgsList);
        }

        private void _CharacterTransformSkill(GameRoleEntityR role, GameIdArgs[] skillIdArgsList)
        {
            var roleTransformCom = role.roleTransformCom;
            var skillCom = roleTransformCom.skillCom;
            skillIdArgsList?.Foreach((skillIdArgs, index) =>
            {
                this._CreateSkill(role, skillIdArgs, skillCom);
            });
        }
    }
}

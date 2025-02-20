using GamePlay.Core;
using GamePlay.Infrastructure;
namespace GamePlay.Bussiness.Logic
{
    public class GameContext
    {
        public GameDirectorEntity director { get; private set; }

        #region [轻量级服务]
        public GameEventService eventService { get; private set; }
        public GameEventService rcEventService { get; private set; }
        public GameCmdBufferService cmdBufferService { get; private set; }
        #endregion

        #region [领域API及上下文]
        public GameDomainApi domainApi { get; private set; }
        public GameRoleContext roleContext { get; private set; }
        public GameSkillContext skillContext { get; private set; }
        public GamePhysicsContext physicsContext { get; private set; }
        public GameActionContext actionContext { get; private set; }
        public GameProjectileContext projectileContext { get; private set; }
        public GameFieldContext fieldContext { get; private set; }
        public GameTransformContext transformContext { get; private set; }
        public GameBuffContext buffContext { get; private set; }
        #endregion

        public GameContext()
        {
            this.director = new GameDirectorEntity();
            this.eventService = new GameEventService();
            this.rcEventService = new GameEventService();
            this.cmdBufferService = new GameCmdBufferService();

            this.domainApi = new GameDomainApi();
            this.roleContext = new GameRoleContext();
            this.skillContext = new GameSkillContext();
            this.physicsContext = new GamePhysicsContext();
            this.actionContext = new GameActionContext();
            this.projectileContext = new GameProjectileContext();
            this.fieldContext = new GameFieldContext();
            this.transformContext = new GameTransformContext();
            this.buffContext = new GameBuffContext();
        }

        public void SubmitRC(string name, object args)
        {
            this.rcEventService.Submit(name, args);
        }

        public void BindRC(string rcName, System.Action<object> callback)
        {
            this.rcEventService.Bind(rcName, callback);
        }

        public void UnbindRC(string rcName, System.Action<object> callback)
        {
            this.rcEventService.Unbind(rcName, callback);
        }

        public GameEntityBase FindEntity(GameEntityType entityType, int entityId)
        {
            switch (entityType)
            {
                case GameEntityType.None:
                    return null;
                case GameEntityType.Role:
                    return this.roleContext.repo.FindByEntityId(entityId);
                case GameEntityType.Skill:
                    return this.skillContext.repo.FindByEntityId(entityId);
                case GameEntityType.Projectile:
                    return this.projectileContext.repo.FindByEntityId(entityId);
                case GameEntityType.Buff:
                    return this.buffContext.repo.FindByEntityId(entityId);
                case GameEntityType.Field:
                    return this.fieldContext.repo.FindByEntityId(entityId);
                default:
                    GameLogger.LogWarning($"查找实体: 未处理的实体类型 {entityType}");
                    return null;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using GamePlay.Bussiness.Core;
using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleDomain : GameRoleDomainApi
    {
        GameContext _context;
        GameRoleContext _roleContext => this._context.roleContext;

        public GameRoleFSMDomainApi fsmApi => this.fsmDomain;

        public GameRoleFSMDomain fsmDomain { get; private set; }
        public GameRoleDomain()
        {
            this.fsmDomain = new GameRoleFSMDomain();
        }

        public void Inject(GameContext context)
        {
            this._context = context;
            this.fsmDomain.Inject(context);
        }

        public void Destroy()
        {
            this.fsmDomain.Destroy();
        }

        public GameRoleTemplate GetRoleTemplate()
        {
            return this._roleContext.factory.template;
        }

        public GameRoleEntity FindByEntityId(int entityId)
        {
            return this._roleContext.repo.FindByEntityId(entityId);
        }

        public void ForeachAllRoles(Action<GameRoleEntity> action)
        {
            this._roleContext.repo.ForeachEntities(action);
        }

        public GameRoleEntity GetUserRole()
        {
            return this._roleContext.userRole;
        }

        public List<GameRoleEntity> GetCampRoles(int campId)
        {
            return this._roleContext.repo.FindAll((entity) =>
            {
                return entity.idCom.campId == campId;
            });
        }

        public bool TryGetPlayerInput(int entityId, out GameRoleInputArgs inputArgs)
        {
            return this._roleContext.playerInputArgs.TryGetValue(entityId, out inputArgs);
        }

        public void SetRoleInput(int entityId, in GameRoleInputArgs inputArgs)
        {
            if (!this._roleContext.playerInputArgs.TryGetValue(entityId, out var oldInputArgs))
            {
                this._roleContext.playerInputArgs[entityId] = inputArgs;
                return;
            }
            oldInputArgs.Update(inputArgs);
            this._roleContext.playerInputArgs[entityId] = oldInputArgs;
        }

        public void SetUserRoleInput(in GameRoleInputArgs inputArgs)
        {
            var entityId = this._roleContext.userRole.idCom.entityId;
            this.SetRoleInput(entityId, inputArgs);
        }

        public GameRoleEntity CreatePlayerRole(int typeId, in GameTransformArgs transArgs, bool isUser)
        {
            var role = this.CreateRole(typeId, GameCampCollection.PLAYER_CAMP_ID, transArgs, isUser);
            if (isUser && this._roleContext.userRole == null) this._roleContext.userRole = role;
            return role;
        }

        public GameRoleEntity CreateEnemyRole(int typeId, in GameTransformArgs transArgs)
        {
            var role = this.CreateRole(typeId, GameCampCollection.ENEMY_CAMP_ID, transArgs, false);
            return role;
        }

        public GameRoleEntity CreateRole(int typeId, int campId, in GameTransformArgs transArgs, bool isUser)
        {
            var role = this._LoadRole(typeId, campId, transArgs);
            this._roleContext.repo.TryAdd(role);

            // 提交RC事件
            this._context.SubmitRC(GameRoleRCCollection.RC_GAME_ROLE_CREATE, new GameRoleRCArgs_Create
            {
                idArgs = role.idCom.ToArgs(),
                transArgs = role.transformCom.ToArgs(),
                isUser = isUser,
                isEnemy = campId != GameCampCollection.PLAYER_CAMP_ID
            });

            // 其余领域的初始化逻辑
            this._InitByOtherDomains(role);
            return role;
        }

        private GameRoleEntity _LoadRole(int typeId, int campId, in GameTransformArgs transArgs)
        {
            var repo = this._roleContext.repo;
            if (!repo.TryFetch(typeId, out var role))
            {
                role = this._roleContext.factory.Load(typeId);
            }
            if (role == null)
            {
                GameLogger.LogError("角色创建失败, 角色ID不存在：" + typeId);
                return null;
            }

            // 变换组件
            role.transformCom.SetByArgs(transArgs);
            if (transArgs.scale == GameVec2.zero)
            {
                GameLogger.LogWarning("角色创建时, scale为0, 自动修正为1");
                role.transformCom.scale = GameVec2.one;
            }
            if (transArgs.angle != 0)
            {
                GameLogger.LogWarning("角色创建时, angle不为0, 自动修正为0");
                role.transformCom.angle = 0;
            }
            // ID组件
            role.idCom.entityId = this._roleContext.idService.FetchId();
            role.idCom.campId = campId;
            return role;
        }

        private void _InitByOtherDomains(GameRoleEntity role)
        {
            // 物理组件
            var logicSize = GameRoleCollection.ROLE_LOGIC_SIZE;
            var logicModel = new GameBoxColliderModel(new GameVec2(0, logicSize.y / 2), 0, logicSize.x, logicSize.y);
            this._context.domainApi.physicsApi.CreatePhysics(role, logicModel, true);
            var colliderRadius = GameRoleCollection.ROLE_COLLIDER_RADIUS;
            var colliderModel = new GameCircleColliderModel(GameVec2.zero, 0, colliderRadius);
            this._context.domainApi.physicsApi.CreatePhysics(role, role.colliderPhysicsCom, colliderModel, false);
            // 技能组件
            role.model.skillIds?.Foreach((skillId, index) =>
            {
                this._context.domainApi.skillApi.CreateSkill(role, skillId);
            });
            // 被动技能在角色生成时的自动释放
            this._context.domainApi.skillApi.DoPassiveSkill(role);
            // 默认进入待机
            this.fsmDomain.TryEnter(role, GameRoleStateType.Idle);
            this._context.domainApi.roleAIApi.TryEnter(role, GameRoleAIStateType.Idle);
        }

        public void Tick(float dt)
        {
            this._roleContext.ClearRoleStateRecords();
            var repo = this._roleContext.repo;
            repo.ForeachEntities((entity) =>
            {
                entity.Tick(dt);
                this.fsmDomain.Tick(entity, dt);
            });
        }

        public GameRoleEntity GetNearestEnemy(GameEntityBase entity)
        {
            var nearestEnemy = default(GameRoleEntity);
            var nearestDistance = float.MaxValue;
            this._roleContext.repo.ForeachEntities((e) =>
            {
                if (e == entity) return;
                if (e.idCom.campId == entity.idCom.campId) return;
                var disSqr = (e.transformCom.position - entity.transformCom.position).sqrMagnitude;
                if (disSqr < nearestDistance)
                {
                    nearestDistance = disSqr;
                    nearestEnemy = e;
                }
            });
            return nearestEnemy;
        }

        public GameRoleEntity[] SummonRoles(
            GameEntityBase summoner,
            int typeId,
            GameCampType campType,
            int count,
            in GameTransformArgs transArgs
        )
        {
            var roles = new GameRoleEntity[count];
            var pos = summoner.transformCom.position;

            // 判定阵营, 暂时适配玩家和怪物, 小于怪物阵营id的视为玩家角色的阵营Id
            var summonerCampId = summoner.idCom.campId;
            var campId = campType == GameCampType.Ally ?
                summonerCampId : summonerCampId < GameCampCollection.ENEMY_CAMP_ID ?
                GameCampCollection.ENEMY_CAMP_ID : GameCampCollection.PLAYER_CAMP_ID;

            for (int i = 0; i < count; i++)
            {
                var role = this.CreateRole(typeId, campId, transArgs, false);
                var randomDir = GameRandomService.GetRandomDir2D();
                role.transformCom.position = pos + randomDir * 1f;
                role.idCom.SetParent(summoner);
                role.aiCom.followState.followEntity = summoner;// 设定AI跟随目标
                roles[i] = role;
            }
            return roles;
        }

        public GameRoleEntity[] SummonRoles(GameActionModel_SummonRoles model, GameEntityBase summoner, in GameTransformArgs transArgs)
        {
            return this.SummonRoles(summoner, model.roleId, model.campType, model.count, transArgs);
        }

        public void TransformRole(GameRoleEntity role, int transToRoleId, in GameActionRecord_CharacterTransform record)
        {
            if (!this._roleContext.factory.template.TryGet(transToRoleId, out var model))
            {
                GameLogger.LogError("变身失败, 模板不存在: " + transToRoleId);
            }

            // 创建变身角色
            var newRole = this._LoadRole(transToRoleId, role.idCom.campId, role.transformCom.ToArgs());
            newRole.idCom.entityId = role.idCom.entityId;
            var isUser = role == this._roleContext.userRole;
            if (isUser) this._roleContext.userRole = newRole;

            // 将新角色替换就旧的角色, 若未处于变身状态, 将oldRole记录在变身角色仓库
            var oldRole = this._roleContext.repo.Replace(newRole);
            if (!this._roleContext.transformRepo.TryFindByEntityId(role.idCom.entityId, out var _))
            {
                this._roleContext.transformRepo.TryAdd(oldRole);
            }

            // 提交RC事件
            this._context.SubmitRC(GameRoleRCCollection.RC_GAME_ROLE_TRANSFORM, new GameRoleRCArgs_CharacterTransform
            {
                idArgs = newRole.idCom.ToArgs(),
            });

            // 其余领域的初始化逻辑
            this._InitByOtherDomains(newRole);

            // 转移buff
            this._context.domainApi.buffApi.TranserBuffCom(oldRole.buffCom, newRole);

            // 转移父子关系
            var children = oldRole.idCom.children;
            children.Foreach((child) =>
            {
                child.idCom.SetParent(newRole);
            });
            children.Clear();

            // 变身后的属性变化
            GameActionUtil_CharacterTransform.DoCharacterTransform(oldRole, newRole, record);

            // 新旧角色的状态变化
            oldRole.SetInvalid();
            var collider = oldRole.physicsCom.collider;
            if (collider != null) collider.isEnable = false;
        }

        public void RemoveAllRoles()
        {
            var repo = this._roleContext.repo;
            var list = repo.ToList();
            list.Foreach((entity) =>
            {
                this.fsmDomain.TryEnter(entity, GameRoleStateType.Destroyed);
            });
            this._roleContext.userRole = null;
        }

        public void DetachAllRolesBuffs()
        {
            var repo = this._roleContext.repo;
            repo.ForeachEntities((entity) =>
            {
                this._context.domainApi.buffApi.DetachAllBuffs(entity);
            });
        }
    }
}

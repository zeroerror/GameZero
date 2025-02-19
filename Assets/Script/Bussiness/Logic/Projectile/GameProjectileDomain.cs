using System;
using GamePlay.Core;
using GamePlay.Infrastructure;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileDomain : GameProjectileDomainApi
    {
        GameContext _context;
        GameProjectileContext _projectileContext => this._context.projectileContext;

        public GameProjectileFSMDomain fsmDomain { get; private set; }
        public GameProjectileFSMDomainApi fsmApi => this.fsmDomain;

        public GameProjectileDomain()
        {
            this.fsmDomain = new GameProjectileFSMDomain();
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

        public void Tick(float dt)
        {
            var repo = this._projectileContext.repo;
            repo.ForeachEntities((e) =>
            {
                e.Tick(dt);
                this.fsmDomain.Tick(e, dt);
            });
        }

        public GameProjectileEntity CreateProjectile(int typeId, GameEntityBase creator, GameTransformArgs transArgs, in GameActionTargeterArgs targeter)
        {
            var repo = this._projectileContext.repo;
            if (!repo.TryFetch(typeId, out var projectile)) projectile = this._projectileContext.factory.Load(typeId);
            if (projectile == null)
            {
                GameLogger.LogError("弹道创建失败，弹道ID不存在：" + typeId);
                return null;
            }
            // 保持缩放为正
            var scale = transArgs.scale;
            scale.x = GameMathF.Abs(scale.x);
            scale.y = GameMathF.Abs(scale.y);
            transArgs.scale = scale;
            // 初始化组件
            if (projectile.model.isLockRotation) transArgs.forward = GameVec2.up;
            projectile.transformCom.SetByArgs(transArgs);
            projectile.transformCom.isLockRotation = projectile.model.isLockRotation;
            projectile.idCom.entityId = this._projectileContext.idService.FetchId();
            projectile.idCom.SetParent(creator);
            projectile.actionTargeterCom.SetTargeter(targeter);
            this._InitTimelineCom(projectile);
            // 绑定组件
            projectile.BindAttributeCom(creator.attributeCom);
            // 添加到仓库
            repo.TryAdd(projectile);
            // 提交RC
            var args = new GameProjectileRCArgs_Create()
            {
                idArgs = projectile.idCom.ToArgs(),
                creatorIdArgs = creator.idCom.ToArgs(),
                transArgs = projectile.transformCom.ToArgs(),
            };
            this._context.SubmitRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_CREATE, args);

            // 初始化FSM
            this.fsmDomain.InitFSM(projectile);

            return projectile;
        }

        private void _InitTimelineCom(GameProjectileEntity projectile)
        {
            var timelineCom = projectile.timelineCom;
            var model = projectile.model;
            timelineCom.SetLength(model.lifeTime);
            var timelineEvModels = model.timelineEvModels;
            timelineEvModels?.Foreach((evModel, index) =>
            {
                timelineCom.AddEventByFrame(evModel.frame, () =>
                {
                    evModel.actionIds?.Foreach((actionId) =>
                    {
                        this._context.domainApi.actionApi.DoAction(actionId, projectile);
                    });
                });
            });
            timelineCom.Play();
        }

        public GameProjectileEntity[] CreateBarrage(int typeId, GameEntityBase creator, in GameTransformArgs transArgs, GameActionTargeterArgs targeter, in GameProjectileBarrageModel_Spread barrageModel)
        {
            var count = barrageModel.count;
            if (count <= 0) return null;
            var spreadAngle = barrageModel.spreadAngle;
            var stepAngle = count == 1 ? 0 : spreadAngle / (count - 1);
            var projectiles = new GameProjectileEntity[count];
            var originTarDir = targeter.targetDirection;
            for (var i = 0; i < count; i++)
            {
                float angle;
                if (count % 2 == 1 && i == 0) angle = 0;// 奇数个时，第一个是0
                else angle = -spreadAngle * 0.5f + i * stepAngle;

                var tarDir = originTarDir.Rotate(angle);
                targeter.targetDirection = tarDir;
                var p = this.CreateProjectile(typeId, creator, transArgs, targeter);
                projectiles[i] = p;
            }
            return projectiles;
        }

        public GameProjectileEntity[] CreateBarrage(int typeId, GameEntityBase creator, GameTransformArgs transArgs, in GameActionTargeterArgs targeter, in GameProjectileBarrageModel_CustomLaunchOffset barrageModel)
        {
            var count = barrageModel.count;
            if (count <= 0) return null;
            var projectiles = new GameProjectileEntity[count];
            var launchOffsets = barrageModel.launchOffsets;
            var angle = GameVec2.SignedAngle(GameVec2.right, targeter.targetDirection);
            var originPos = transArgs.position;
            for (var i = 0; i < count; i++)
            {
                var offset = launchOffsets[i];
                offset = offset.Rotate(angle);
                transArgs.position = originPos + offset;
                var p = this.CreateProjectile(typeId, creator, transArgs, targeter);
                projectiles[i] = p;
            }
            return projectiles;
        }

        public void RemoveAllProjectiles()
        {
            var repo = this._projectileContext.repo;
            var list = repo.ToList();
            list.Foreach((entity) =>
            {
                this.fsmDomain.TryEnter(entity, GameProjectileStateType.Destroyed);
            });
        }

        public void ForeachAllProjectiles(Action<GameProjectileEntity> action)
        {
            var repo = this._projectileContext.repo;
            repo.ForeachEntities(action);
        }
    }
}
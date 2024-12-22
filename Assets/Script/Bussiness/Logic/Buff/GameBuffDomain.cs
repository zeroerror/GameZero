using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameBuffDomain : GameBuffDomainApi
    {
        GameContext _context;
        GameBuffContext _buffContext => this._context.buffContext;

        public GameBuffDomain()
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
            this._context.roleContext.repo.ForeachAllEntities(role =>
            {
                this._TickBuff(role, dt);
            });
        }

        private void _TickBuff(GameRoleEntity role, float dt)
        {
            var buffCom = role.buffCom;
            for (int i = 0; i < buffCom.buffList.Count; i++)
            {
                var buff = buffCom.buffList[i];
                buff.Tick(dt);
                // 行为触发
                var isSatisfied_action = buff.conditionSetEntity_action.CheckSatisfied();
                if (isSatisfied_action)
                {
                    buff.model.actionIds?.Foreach(actionId =>
                    {
                        buff.physicsCom.ClearCollided();
                        this._context.domainApi.actionApi.DoAction(actionId, buff);
                    });
                }
                // 移除触发
                var isSatisfied_remove = buff.conditionSetEntity_remove.CheckSatisfied();
                if (isSatisfied_remove)
                {
                    this._context.cmdBufferService.AddDelayCmd(0, () =>
                    {
                        this.DetachBuff(buff.model.typeId, role, 0);
                    });
                }
            }
        }

        public bool AttachBuff(int typeId, GameEntityBase target, int layer, out int realAttachLayer)
        {
            realAttachLayer = 0;

            if (!target.TryGetLinkEntity<GameRoleEntity>(out var targetRole))
            {
                GameLogger.LogError("目标不是角色, 暂不支持挂载Buff");
                return false;
            }

            var buffCom = targetRole.buffCom;
            if (buffCom.TryGet(typeId, out var existBuff))
            {
                realAttachLayer = existBuff.AttachLayer(layer);
                _Attach(existBuff);
                return true;
            }

            var repo = this._buffContext.repo;
            if (!repo.TryFetch(typeId, out var newBuff))
            {
                newBuff = this._buffContext.factory.Load(typeId);
            }
            if (newBuff == null)
            {
                GameLogger.LogError("Buff创建失败, BuffID不存在：" + typeId);
                return false;
            }

            // 绑定父子关系
            newBuff.idCom.SetParent(targetRole);
            // 组件绑定
            newBuff.BindTransformCom(targetRole.transformCom);

            buffCom.Add(newBuff);
            this._buffContext.repo.TryAdd(newBuff);
            _Attach(newBuff);
            return true;

            // 内置方法
            void _Attach(GameBuffEntity buff)
            {
                // 设置行为目标
                buff.actionTargeterCom.SetTargeter(new GameActionTargeterArgs
                {
                    targetEntity = targetRole,
                    targetPosition = targetRole.transformCom.position,
                    targetDirection = targetRole.forward
                });
                // 提交RC事件
                this._context.SubmitRC(GameBuffRCCollection.RC_GAME_BUFF_ATTACH, new GameBuffRCArgs_Attach
                {
                    buffIdArgs = buff.idCom.ToArgs(),
                    targetIdArgs = targetRole.idCom.ToArgs(),
                    layer = layer
                });
            }
        }

        public int DetachBuff(int buffId, GameEntityBase target, int layer)
        {
            if (!target.TryGetLinkEntity<GameRoleEntity>(out var targetRole))
            {
                GameLogger.LogError("目标不是角色, 暂不支持移除Buff");
                return 0;
            }
            var buffCom = targetRole.buffCom;
            var detachLayer = buffCom.DetachBuff(buffId, layer);
            if (detachLayer > 0)
            {
                this._context.SubmitRC(GameBuffRCCollection.RC_GAME_BUFF_REMOVE, new GameBuffRCArgs_Remove
                {
                    buffId = buffId,
                    targetIdArgs = targetRole.idCom.ToArgs(),
                    detachLayer = detachLayer
                });
            }
            return detachLayer;
        }
    }
}
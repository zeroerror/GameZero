using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameBuffDomainR : GameBuffDomainApiR
    {
        GameContextR _context;
        GameBuffContextR _buffContext => this._context.buffContext;

        public GameBuffDomainR()
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
            this._context.BindRC(GameBuffRCCollection.RC_GAME_BUFF_ATTACH, this._OnBuffAttach);
            this._context.BindRC(GameBuffRCCollection.RC_GAME_BUFF_DETACH, this._OnBuffDeAttach);
        }

        private void _UnbindEvents()
        {
            this._context.UnbindRC(GameBuffRCCollection.RC_GAME_BUFF_ATTACH, this._OnBuffAttach);
            this._context.UnbindRC(GameBuffRCCollection.RC_GAME_BUFF_DETACH, this._OnBuffDeAttach);
        }

        private void _OnBuffAttach(object args)
        {
            var evArgs = (GameBuffRCArgs_Attach)args;
            var targetIdArgs = evArgs.targetIdArgs;
            // 检查角色异步
            var target = this._context.roleContext.repo.FindByEntityId(targetIdArgs.entityId);
            if (target == null)
            {
                this._context.DelayRC(GameBuffRCCollection.RC_GAME_BUFF_ATTACH, args);
                return;
            }

            this.AttachBuff(evArgs.buffIdArgs, target, evArgs.layer);
        }

        private void _OnBuffDeAttach(object args)
        {
            var evArgs = (GameBuffRCArgs_Detach)args;
            var targetIdArgs = evArgs.targetIdArgs;
            // 检查角色异步
            var target = this._context.roleContext.repo.FindByEntityId(targetIdArgs.entityId);
            if (target == null)
            {
                this._context.DelayRC(GameBuffRCCollection.RC_GAME_BUFF_DETACH, args);
                return;
            }

            this.DetachBuff(evArgs.buffId, target, evArgs.detachLayer);
        }

        public void Tick(float dt)
        {
        }

        public void AttachBuff(GameIdArgs buffIdArgs, GameEntityBase target, int layer)
        {
            if (!target.TryGetLinkEntity<GameRoleEntityR>(out var targetRole))
            {
                GameLogger.LogError("目标不是角色, 暂不支持挂载Buff");
                return;
            }

            var typeId = buffIdArgs.typeId;
            var buffCom = targetRole.buffCom;
            if (buffCom.TryGet(typeId, out var existBuff))
            {
                existBuff.AttachLayer(layer);
                return;
            }

            var repo = this._buffContext.repo;
            if (!repo.TryFetch(typeId, out var newBuff))
            {
                newBuff = this._buffContext.factory.Load(typeId);
            }
            if (newBuff == null)
            {
                GameLogger.LogError("Buff创建失败, BuffID不存在：" + typeId);
                return;
            }

            newBuff.idCom.SetByArgs(buffIdArgs);
            newBuff.idCom.SetParent(targetRole);
            newBuff.BindTransformCom(targetRole.transformCom);

            buffCom.Add(newBuff);
            this._buffContext.repo.TryAdd(newBuff);

            // buff特效
            var vfxUrl = newBuff.model.vfxUrl;
            if (!string.IsNullOrEmpty(vfxUrl))
            {
                var args = new GameVFXPlayArgs
                {
                    url = vfxUrl,
                    attachNode = targetRole.root,
                };
                this._context.domainApi.vfxApi.Play(args);
            }

            return;
        }

        public void DetachBuff(int buffId, GameEntityBase target, int layer)
        {
            if (!target.TryGetLinkEntity<GameRoleEntityR>(out var targetRole))
            {
                GameLogger.LogError("目标不是角色, 暂不支持移除Buff");
                return;
            }
            var buffCom = targetRole.buffCom;
            buffCom.DetachBuff(buffId, layer);
        }
    }
}
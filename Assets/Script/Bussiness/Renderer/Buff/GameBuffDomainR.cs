using System.Collections.Generic;
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
            this._BindEvents();
        }

        public void Destroy()
        {
            this._UnbindEvents();
        }

        private void _BindEvents()
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
            var rcArgs = (GameBuffRCArgs_Attach)args;
            var targetIdArgs = rcArgs.targetIdArgs;
            // 检查角色异步
            var target = this._context.roleContext.repo.FindByEntityId(targetIdArgs.entityId);
            if (target == null)
            {
                this._context.DelayRC(GameBuffRCCollection.RC_GAME_BUFF_ATTACH, args);
                return;
            }

            this.AttachBuff(rcArgs.buffIdArgs, target, rcArgs.layer);
        }

        private void _OnBuffDeAttach(object args)
        {
            var rcArgs = (GameBuffRCArgs_Detach)args;
            var targetIdArgs = rcArgs.targetIdArgs;
            // 检查角色异步
            var target = this._context.roleContext.repo.FindByEntityId(targetIdArgs.entityId);
            if (target == null)
            {
                this._context.DelayRC(GameBuffRCCollection.RC_GAME_BUFF_DETACH, args);
                return;
            }

            this.DetachBuff(rcArgs.buffId, target, rcArgs.detachLayer);
        }

        public void Tick(float dt)
        {
        }

        public void AttachBuff(GameIdArgs buffIdArgs, GameEntityBase target, int layer)
        {
            if (!target.TryGetLinkParent<GameRoleEntityR>(out var targetRole))
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
            var model = newBuff.model;
            var vfxUrl = model.vfxUrl;
            if (!string.IsNullOrEmpty(vfxUrl))
            {
                var args = new GameVFXPlayArgs
                {
                    url = vfxUrl,
                    attachNode = targetRole.bodyCom.root,
                    loopDuration = -1,
                    layerType = model.vfxLayerType,
                    scale = model.vfxScale,
                    attachOffset = model.vfxOffset,
                    orderOffset = model.vfxOrderOffset,
                };
                newBuff.vfxEntity = this._context.domainApi.vfxApi.Play(args);
            }

            return;
        }

        public void DetachBuff(int buffId, GameEntityBase target, int layer)
        {
            if (!target.TryGetLinkParent<GameRoleEntityR>(out var targetRole))
            {
                GameLogger.LogError("目标不是角色, 暂不支持移除Buff");
                return;
            }
            var buffCom = targetRole.buffCom;
            if (!buffCom.TryGet(buffId, out var buff))
            {
                GameLogger.LogError("Buff不存在，无法移除：" + buffId);
                return;
            }
            if (!buff.isValid) return;
            buff.DetachLayer(layer);
            if (!buff.isValid)
            {
                buffCom.Remove(buff);
                this._context.domainApi.vfxApi.Stop(buff.vfxEntity);
            }
        }

        public bool TryGetBuffModel(int buffId, out GameBuffModelR buffModel)
        {
            return this._buffContext.factory.template.TryGet(buffId, out buffModel);
        }

        public List<GameBuffModelR> GetBuffModelList()
        {
            return this._buffContext.factory.template.GetBuffModelList();
        }

        public void TranserBuffCom(GameBuffComR refBuffCom, GameRoleEntityR targetRole)
        {
            var buffList = targetRole.buffCom.buffList;
            buffList.Clear();
            buffList.AddRange(refBuffCom.buffList);
            refBuffCom.buffList.Clear();
            // 转移buff特效的挂载节点
            buffList.Foreach((buff) =>
            {
                var vfxEntity = buff.vfxEntity;
                if (vfxEntity == null) return;
                var newPlayArgs = vfxEntity.playArgs;
                if (newPlayArgs.attachNode)
                {
                    newPlayArgs.attachNode = targetRole.bodyCom.root;
                }
                vfxEntity.SetPlayArgs(newPlayArgs);
            });
        }
    }
}
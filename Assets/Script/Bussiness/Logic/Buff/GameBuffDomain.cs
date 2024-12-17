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

        public void Dispose()
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
                if (!buff.isValid)
                {
                    this._context.cmdBufferService.Add(0, () =>
                    {
                        this.RemoveBuff(buff.model.typeId, role);
                    });
                }
            }
        }

        public bool AttachBuff(int typeId, GameRoleEntity targetRole)
        {
            var buffCom = targetRole.buffCom;
            if (buffCom.TryGet(typeId, out var existBuff))
            {
                existBuff.AttachLayer();
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
            buffCom.Add(newBuff);
            return true;
        }

        public bool RemoveBuff(int buffId, GameRoleEntity targetRole)
        {
            var buffCom = targetRole.buffCom;
            return buffCom.Remove(buffId);
        }
    }
}
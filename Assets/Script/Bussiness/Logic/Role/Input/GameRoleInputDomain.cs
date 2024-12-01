using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleInputDomain
    {
        GameContext _context;
        GameRoleContext _roleContext => this._context.roleContext;

        public GameRoleInputDomain()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public void Dispose()
        {
        }

        public void Tick()
        {
            this._roleContext.repo.ForeachEntities((entity) =>
            {
                entity.inputCom.Clear();
                if (this._roleContext.TryGetPlayerInputArgs(entity.idCom.entityId, out var inputArgs))
                {
                    this._DoAutoLockTarget(entity, ref inputArgs);
                    entity.inputCom.SetByArgs(inputArgs);
                }
            });
            this._roleContext.ClearPlayerInputArgs();
        }

        private void _DoAutoLockTarget(GameEntityBase entity, ref GameRoleInputArgs inputArgs)
        {
            var skillId = inputArgs.skillId;
            if (skillId == 0) return;
            if (!this._context.domainApi.skillApi.TryGetModel(skillId, out var skillModel)) return;
            var targeterType = skillModel.conditionModel.targeterType;
            var roleApi = this._context.domainApi.roleApi;
            var nearestEnemy = roleApi.GetNearestEnemy(entity);
            var targeterList = new List<GameActionTargeterArgs>();
            inputArgs.targeterArgsList = targeterList;
            switch (targeterType)
            {
                case GameSkillTargterType.Enemy:
                    targeterList.Add(new GameActionTargeterArgs { targetEntity = nearestEnemy });
                    break;
                case GameSkillTargterType.Direction:
                    var dir = nearestEnemy.transformCom.position - entity.transformCom.position;
                    dir.Normalize();
                    targeterList.Add(new GameActionTargeterArgs { targetDirection = dir });
                    break;
                case GameSkillTargterType.Position:
                    targeterList.Add(new GameActionTargeterArgs { targetPosition = nearestEnemy.transformCom.position });
                    break;
            }
        }
    }
}
using System.Collections.Generic;
using GamePlay.Core;

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

        public void Destroy()
        {
        }

        public void Tick()
        {
            this._roleContext.repo.ForeachEntities((entity) =>
            {
                entity.inputCom.Clear();
                if (this._context.domainApi.roleApi.TryGetPlayerInput(entity.idCom.entityId, out var inputArgs))
                {
                    this._ProcessAutoInput(entity, ref inputArgs);
                    entity.inputCom.SetByArgs(inputArgs);
                }
            });
            this._roleContext.ClearPlayerInputArgs();
        }

        /// <summary>
        /// 处理自动输入, 包括自动锁定目标、方向、位置等
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="inputArgs"></param>
        private void _ProcessAutoInput(GameEntityBase actor, ref GameRoleInputArgs inputArgs)
        {
            var skillId = inputArgs.skillId;
            if (skillId == 0) return;
            if (!this._context.domainApi.skillApi.TryGetModel(skillId, out var skillModel)) return;
            // 玩家主动的目标选取
            var targeterList = inputArgs.targeterArgsList;
            if (targeterList?.Count > 0) return;
            // 自动目标选取
            targeterList = new List<GameActionTargeterArgs>();
            var roleApi = this._context.domainApi.roleApi;
            var nearestEnemy = roleApi.GetNearestEnemy(actor);
            inputArgs.targeterArgsList = targeterList;
            var targeterType = skillModel.conditionModel.targeterType;
            GameActionTargeterArgs targeter = new GameActionTargeterArgs();
            targeter.targetEntity = targeterType == GameSkillTargterType.Actor ? actor : nearestEnemy;
            targeter.targetPosition = targeter.targetEntity != null ? targeter.targetEntity.transformCom.position : actor.transformCom.position;
            targeter.targetDirection =
            targeterType == GameSkillTargterType.Direction ?
                actor.transformCom.forward : targeter.targetEntity != null ?
                (targeter.targetEntity.transformCom.position - actor.transformCom.position).normalized :
                actor.transformCom.forward;
            targeterList.Add(targeter);
        }
    }
}
using System.Collections.Generic;
using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{

    public class GameRoleFSMDomain : GameRoleFSMDomainApi
    {
        private GameContext _context;
        private GameRoleContext _roleContext => this._context.roleContext;

        private GameRoleStateDomainBase _anyStateDomain;
        private Dictionary<GameRoleStateType, GameRoleStateDomainBase> _stateDomainDict;

        public GameRoleFSMDomain()
        {
            this._anyStateDomain = new GameRoleStateDomain_Any();

            this._stateDomainDict = new Dictionary<GameRoleStateType, GameRoleStateDomainBase>
            {
                { GameRoleStateType.Idle, new GameRoleStateDomain_Idle() },
                { GameRoleStateType.Move, new GameRoleStateDomain_Move() },
                { GameRoleStateType.Cast, new GameRoleStateDomain_Cast() },
                { GameRoleStateType.Dead, new GameRoleStateDomain_Dead() },
                { GameRoleStateType.Stealth, new GameRoleStateDomain_Stealth() },
                { GameRoleStateType.Destroyed, new GameRoleStateDomain_Destroyed() }
            };
        }

        public void Inject(GameContext context)
        {
            this._context = context;
            this._anyStateDomain.Inject(context);
            foreach (var stateDomain in this._stateDomainDict.Values)
            {
                stateDomain.Inject(context);
            }
        }

        public void Destroy()
        {
        }

        public void Tick(GameRoleEntity role, float dt)
        {
            var fsmCom = role.fsmCom;
            var stateType = fsmCom.stateType;
            if (fsmCom.isInvalid) return;

            this._anyStateDomain.Tick(role, dt);

            if (!this._stateDomainDict.TryGetValue(stateType, out var stateDomain)) return;
            stateDomain.Tick(role, dt);
        }

        public bool TryEnter(GameRoleEntity role, GameRoleStateType toState, params object[] args)
        {
            if (!this._stateDomainDict.TryGetValue(toState, out var stateDomain))
            {
                GameLogger.LogError($"TryEnter: 无法找到状态域{toState}");
                return false;
            }
            var check = stateDomain.CheckEnter(role, args);
            if (check)
            {
                this._ExitToState(role, toState);
                stateDomain.Enter(role, args);

                var stateType = role.fsmCom.stateType;
                var skillType = stateType == GameRoleStateType.Cast ? role.fsmCom.castState.skill.skillModel.skillType : GameSkillType.None;
                this._roleContext.roleStateRecords.Add(new GameRoleStateRecord(
                    role.idCom.entityId,
                    stateType,
                    skillType
                ));
            }
            return check;
        }

        private void _ExitToState(GameRoleEntity role, GameRoleStateType toState)
        {
            var fsmCom = role.fsmCom;
            var stateType = fsmCom.stateType;
            if (!this._stateDomainDict.TryGetValue(stateType, out var stateDomain)) return;
            stateDomain.ExitTo(role, toState);
            // 提交RC
            this._context.SubmitRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_EXIT, new GameRoleRCArgs_StateExit
            {
                exitStateType = stateType,
                idArgs = role.idCom.ToArgs(),
            });
        }
    }

}
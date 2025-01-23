using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorStateDomain_Settling : GameDirectorStateDomainBase
    {
        public GameDirectorStateDomain_Settling(GameDirectorDomain directorDomain) : base(directorDomain)
        {
        }

        public override bool CheckEnter(GameDirectorEntity director, object args = null)
        {
            return true;
        }

        public override void Enter(GameDirectorEntity director, object args = null)
        {
            var playerCampId = GameRoleCollection.PLAYER_ROLE_CAMP_ID;
            var enemyCampId = GameRoleCollection.ENEMY_ROLE_CAMP_ID;
            var playerCount = this._context.roleContext.repo.GetEntityCount((role) => role.idCom.campId == playerCampId);
            var enemyCount = this._context.roleContext.repo.GetEntityCount((role) => role.idCom.campId == enemyCampId);
            var isWin = playerCount > enemyCount;
            var fsmCom = director.fsmCom;
            fsmCom.EnterSettling(playerCount, enemyCount, isWin);

            this._context.domainApi.roleApi.DetachAllRolesBuffs();

            GameLogger.DebugLog("导演 - 进入结算状态");
            GameLogger.DebugLog($"导演 - 结算结果: 玩家{playerCount} - 敌人{enemyCount} - 结果{(isWin ? "胜利" : "失败")}");

            // 提交RC
            this._context.SubmitRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_SETTLING, new GameDirectorRCArgs_StateEnterSettling
            {
                fromStateType = fsmCom.lastStateType,
                playerCount = playerCount,
                enemyCount = enemyCount,
                isWin = isWin,
            });
        }

        protected override void _Tick(GameDirectorEntity director, float frameTime)
        {
            var fsmCom = director.fsmCom;
            fsmCom.settlingState.stateTime += frameTime;
            this._directorDomain.entityCollectDomain.Tick();
        }

        protected override (GameDirectorStateType, object) _CheckExit(GameDirectorEntity director)
        {
            var settlingState = director.fsmCom.settlingState;
            if (settlingState.stateTime >= 0.5 && settlingState.isWin)
            {
                return (GameDirectorStateType.Loading, 1);
            }
            return (GameDirectorStateType.None, null);
        }

        public override void ExitTo(GameDirectorEntity director, GameDirectorStateType toState)
        {
            var isWin = director.fsmCom.settlingState.isWin;
            if (isWin)
            {
                this._context.director.coins += 100;
            }
        }
    }
}
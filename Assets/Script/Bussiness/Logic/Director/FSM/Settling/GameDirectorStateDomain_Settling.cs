using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorStateDomain_Settling : GameDirectorStateDomainBase
    {
        public GameDirectorStateDomain_Settling(GameDirectorDomain directorDomain) : base(directorDomain)
        {
        }

        protected override void _BindEvents()
        {
            this._context.eventService.Bind(GameLCCollection.LC_GAME_SETTLING_CONFIRM_EXIT, this._onSettlingConfirmExit);
        }

        protected override void _UnbindEvents()
        {
            this._context.eventService.Unbind(GameLCCollection.LC_GAME_SETTLING_CONFIRM_EXIT, this._onSettlingConfirmExit);
        }

        private void _onSettlingConfirmExit(object args)
        {
            var fsmCom = this._context.director.fsmCom;
            fsmCom.settlingState.stateFinished = true;
        }

        public override bool CheckEnter(GameDirectorEntity director, object args = null)
        {
            return true;
        }

        public override void Enter(GameDirectorEntity director, object args = null)
        {
            var playerCampId = GameCampCollection.PLAYER_CAMP_ID;
            var enemyCampId = GameCampCollection.ENEMY_CAMP_ID;
            var playerCount = this._context.roleContext.repo.GetEntityCount((role) => role.idCom.campId == playerCampId);
            var enemyCount = this._context.roleContext.repo.GetEntityCount((role) => role.idCom.campId == enemyCampId);
            var isWin = playerCount > enemyCount;
            var fsmCom = director.fsmCom;
            fsmCom.EnterSettling(playerCount, enemyCount, isWin);

            this._context.domainApi.roleApi.DetachAllRolesBuffs();

            GameLogger.DebugLog("导演 - 进入结算状态");
            GameLogger.DebugLog($"导演 - 结算结果: 玩家{playerCount} - 敌人{enemyCount} - 结果{(isWin ? "胜利" : "失败")}");

            // 提交RC - 进入结算
            var gainCoins = this._SettleCoins(director);
            GameDirectorRCArgs_StateEnterSettling rcArgs;
            rcArgs.fromStateType = fsmCom.lastStateType;
            rcArgs.playerCount = playerCount;
            rcArgs.enemyCount = enemyCount;
            rcArgs.isWin = isWin;
            rcArgs.ganiCoins = gainCoins;
            this._context.SubmitRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_SETTLING, rcArgs);

            // 提交RC - 金币变更
            this._context.SubmitRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_COINS_CHANGE, args);
        }

        protected override void _Tick(GameDirectorEntity director, float frameTime)
        {
            var fsmCom = director.fsmCom;
            fsmCom.settlingState.stateTime += frameTime;
            this._directorDomain.entityCollectDomain.Tick();
        }

        protected override GameDirectorExitStateArgs _CheckExit(GameDirectorEntity director)
        {
            var settlingState = director.fsmCom.settlingState;
            if (!settlingState.stateFinished)
            {
                return new GameDirectorExitStateArgs(GameDirectorStateType.None);
            }

            if (settlingState.isWin)
            {
                return new GameDirectorExitStateArgs(GameDirectorStateType.Loading, 1);
            }
            return new GameDirectorExitStateArgs(GameDirectorStateType.None);
        }

        public override void ExitTo(GameDirectorEntity director, GameDirectorStateType toState)
        {
        }

        /// <summary> 金币结算 </summary>
        private int _SettleCoins(GameDirectorEntity director)
        {
            // 战斗时间 金币 = 100 - 战斗时间(s)[0-40]
            var fightingState = director.fsmCom.fightingState;
            var fightingTime = fightingState.stateTime;
            var timeGainCoins = 100 - GameMath.Clamp((int)(fightingTime), 0, 40);
            // 剩余主单位 金币 = 20 * 剩余主单位数量
            var initEntityIdArgsList = fightingState.initEntityIdArgsList;
            var remainUnitCount = this._context.roleContext.repo.GetEntityCount((role) => initEntityIdArgsList.FindIndex((args) => args.entityId == role.idCom.entityId) >= 0);
            var unitGainCoins = 20 * remainUnitCount;
            // 总金币
            var totalGainCoins = timeGainCoins + unitGainCoins;
            GameLogger.DebugLog($"导演 - 结算金币: 时间{timeGainCoins} - 单位{unitGainCoins} - 总计{totalGainCoins}");
            return totalGainCoins;
        }
    }
}
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

        protected override GameDirectorExitStateArgs _CheckExit(GameDirectorEntity director)
        {
            var settlingState = director.fsmCom.settlingState;
            if (settlingState.stateTime >= 0.5 && settlingState.isWin)
            {
                return new GameDirectorExitStateArgs(GameDirectorStateType.Loading, 1);
            }
            return new GameDirectorExitStateArgs(GameDirectorStateType.None);
        }

        public override void ExitTo(GameDirectorEntity director, GameDirectorStateType toState)
        {
            var isWin = director.fsmCom.settlingState.isWin;
            if (isWin)
            {
                var gainCoins = this._SettleCoins(director);
                this._context.director.coins += gainCoins;
                var args = new GameDirectorRCArgs_CoinsChange
                {
                    coins = this._context.director.coins,
                };
                this._context.SubmitRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_COINS_CHANGE, args);
            }
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
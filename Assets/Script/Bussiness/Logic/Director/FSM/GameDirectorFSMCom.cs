using System.Collections.Generic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorFSMCom
    {
        /// <summary> 当前状态 </summary>
        public GameDirectorStateType stateType { get; private set; }
        /// <summary> 上一个状态 </summary>
        public GameDirectorStateType lastStateType { get; private set; }

        public GameDirectorState_Loading loadingState { get; private set; }
        public GameDirectorState_FightPreparing fightPreparingState { get; private set; }
        public GameDirectorState_Fight fightingState { get; private set; }
        public GameDirectorState_Settling settlingState { get; private set; }

        public GameDirectorFSMCom()
        {
            this.loadingState = new GameDirectorState_Loading();
            this.fightPreparingState = new GameDirectorState_FightPreparing();
            this.fightingState = new GameDirectorState_Fight();
            this.settlingState = new GameDirectorState_Settling();
        }

        public void EnterLoading(int loadFieldId)
        {
            this.SwitchToState(GameDirectorStateType.Loading);
            this.loadingState.loadFieldId = loadFieldId;
        }

        public void EnterFightPreparing(List<GameActionOptionModel> options)
        {
            this.SwitchToState(GameDirectorStateType.FightPreparing);
            this.fightPreparingState.options = options;
        }

        public void EnterFighting(List<GameIdArgs> initEntityIdArgsList)
        {
            this.SwitchToState(GameDirectorStateType.Fighting);
            this.fightingState.initEntityIdArgsList = initEntityIdArgsList;
        }

        public void EnterSettling(int playerCount, int enemyCount, bool isWin)
        {
            this.SwitchToState(GameDirectorStateType.Settling);
            this.settlingState.playerCount = playerCount;
            this.settlingState.enemyCount = enemyCount;
            this.settlingState.isWin = isWin;
        }

        public void SwitchToState(GameDirectorStateType nextState)
        {
            this.lastStateType = this.stateType;
            this.stateType = nextState;
            switch (nextState)
            {
                case GameDirectorStateType.Loading:
                    loadingState.Clear();
                    break;
                case GameDirectorStateType.FightPreparing:
                    fightPreparingState.Clear();
                    break;
                case GameDirectorStateType.Fighting:
                    fightingState.Clear();
                    break;
                case GameDirectorStateType.Settling:
                    settlingState.Clear();
                    break;
                default:
                    GameLogger.LogError("GameDirectorFSMCom.SwitchToState: 未处理的状态");
                    break;
            }
        }
    }
}


using System.Collections.Generic;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.Render
{
    public class GameDirectorFSMCom
    {
        /// <summary> 当前状态 </summary>
        public GameDirectorStateType stateType { get; private set; }
        /// <summary> 上一个状态 </summary>
        public GameDirectorStateType lastStateType { get; private set; }

        public GameDirectorState_LoadingR loadingState { get; private set; }
        public GameDirectorState_FightPreparingR fightPreparingState { get; private set; }
        public GameDirectorState_FightR fightingState { get; private set; }
        public GameDirectorState_SettlingR settlingState { get; private set; }

        public GameDirectorFSMCom()
        {
            this.loadingState = new GameDirectorState_LoadingR();
            this.fightPreparingState = new GameDirectorState_FightPreparingR();
            this.fightingState = new GameDirectorState_FightR();
            this.settlingState = new GameDirectorState_SettlingR();
        }

        public void EnterLoading(int loadFieldId)
        {
            this.SwitchToState(GameDirectorStateType.Loading);
            this.loadingState.loadFieldId = loadFieldId;
        }

        public void EnterFightPreparing()
        {
            this.SwitchToState(GameDirectorStateType.FightPreparing);
        }

        public void EnterFighting()
        {
            this.SwitchToState(GameDirectorStateType.Fighting);
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


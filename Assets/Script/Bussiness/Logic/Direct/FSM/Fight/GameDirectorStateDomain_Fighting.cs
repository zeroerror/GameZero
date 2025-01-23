using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorStateDomain_Fighting : GameDirectorStateDomainBase
    {
        public GameDirectorStateDomain_Fighting(GameDirectorDomain directorDomain) : base(directorDomain)
        {
        }

        public override bool CheckEnter(GameDirectorEntity director, object args = null)
        {
            return true;
        }

        public override void Enter(GameDirectorEntity director, object args = null)
        {
            var fsmCom = director.fsmCom;
            fsmCom.EnterFighting();
            GameLogger.DebugLog("导演 - 进入战斗状态");
            // 提交RC
            this._context.SubmitRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHTING, new GameDirectorRCArgs_StateEnterFighting
            {
                fromStateType = fsmCom.lastStateType,
            });
        }

        protected override (GameDirectorStateType, object) _CheckExit(GameDirectorEntity director)
        {
            var playerCampId = GameRoleCollection.PLAYER_ROLE_CAMP_ID;
            var playerCount = this._context.roleContext.repo.GetEntityCount((role) => role.idCom.campId == playerCampId);
            if (playerCount == 0) return (GameDirectorStateType.Settling, null);
            var enemyCampId = GameRoleCollection.ENEMY_ROLE_CAMP_ID;
            var enemyCount = this._context.roleContext.repo.GetEntityCount((role) => role.idCom.campId == enemyCampId);
            if (enemyCount == 0) return (GameDirectorStateType.Settling, null);
            return (GameDirectorStateType.None, null);
        }

        protected override void _Tick(GameDirectorEntity director, float frameTime)
        {
            if (director.timeScaleCom.timeScaleDirty)
            {
                director.timeScaleCom.ClearTimeScaleDirty();
                this._context.SubmitRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_TIME_SCALE_CHANGE, director.timeScaleCom.timeScale);
            }
            this._directorDomain.TickDomain(frameTime);
        }

    }
}
namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorStateDomain_Fighting : GameDirectorStateDomainBase
    {
        public GameDirectorStateDomain_Fighting(GameDirectorDomain directorDomain) : base(directorDomain)
        {
        }

        public override bool CheckEnter(GameDirectorEntity director)
        {
            return true;
        }

        public void Destory()
        {
        }

        public override void Enter(GameDirectorEntity director)
        {
            var fsmCom = director.fsmCom;
            fsmCom.EnterFighting();
            // 提交RC
            this._context.SubmitRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHTING, new GameDirectorRCArgs_StateEnterFighting
            {
                fromStateType = fsmCom.lastStateType,
            });
        }

        protected override GameDirectorStateType _CheckExit(GameDirectorEntity director)
        {
            var playerCampId = GameRoleCollection.PLAYER_ROLE_CAMP_ID;
            var playerCount = this._context.roleContext.repo.GetEntityCount((role) => role.idCom.campId == playerCampId);
            if (playerCount == 0) return GameDirectorStateType.Settling;
            var enemyCampId = GameRoleCollection.ENEMY_ROLE_CAMP_ID;
            var enemyCount = this._context.roleContext.repo.GetEntityCount((role) => role.idCom.campId == enemyCampId);
            if (enemyCount == 0) return GameDirectorStateType.Settling;
            return GameDirectorStateType.None;
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
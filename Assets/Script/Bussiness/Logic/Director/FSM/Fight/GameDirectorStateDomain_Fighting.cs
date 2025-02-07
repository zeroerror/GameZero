using System.Collections.Generic;
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
            var initEntityIdArgsList = new List<GameIdArgs>();
            var playerCampId = GameCampCollection.PLAYER_CAMP_ID;
            this._context.roleContext.repo.ForeachEntities((role) =>
            {
                if (role.idCom.campId == playerCampId)
                {
                    initEntityIdArgsList.Add(new GameIdArgs(
                        role.idCom.typeId,
                        role.idCom.entityType,
                        role.idCom.entityId,
                        role.idCom.campId
                    ));
                }
            });
            fsmCom.EnterFighting(initEntityIdArgsList);
            GameLogger.DebugLog("导演 - 进入战斗状态");

            // 记录站位
            var unitEntitys = director.unitEntitys;
            unitEntitys?.ForEach((unitEntity) =>
            {
                var unit = this._context.domainApi.directorApi.FindUnit(unitEntity);
                if (unit == null) return;
                unitEntity.standPos = unit.transformCom.position;
            });

            // 提交RC
            GameDirectorRCArgs_StateEnterFighting rcArgs;
            rcArgs.fromStateType = fsmCom.lastStateType;
            this._context.SubmitRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHTING, rcArgs);
        }

        protected override GameDirectorExitStateArgs _CheckExit(GameDirectorEntity director)
        {
            var playerCampId = GameCampCollection.PLAYER_CAMP_ID;
            var playerCount = this._context.roleContext.repo.GetEntityCount((role) => role.idCom.campId == playerCampId);
            if (playerCount == 0) return new GameDirectorExitStateArgs(GameDirectorStateType.Settling);
            var enemyCampId = GameCampCollection.ENEMY_CAMP_ID;
            var enemyCount = this._context.roleContext.repo.GetEntityCount((role) => role.idCom.campId == enemyCampId);
            if (enemyCount == 0) return new GameDirectorExitStateArgs(GameDirectorStateType.Settling);
            return new GameDirectorExitStateArgs(GameDirectorStateType.None);
        }

        protected override void _Tick(GameDirectorEntity director, float frameTime)
        {
            director.fsmCom.fightingState.stateTime += frameTime;
            if (director.timeScaleCom.timeScaleDirty)
            {
                director.timeScaleCom.ClearTimeScaleDirty();
                this._context.SubmitRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_TIME_SCALE_CHANGE, director.timeScaleCom.timeScale);
            }
            this._directorDomain.TickDomain(frameTime);
        }

    }
}
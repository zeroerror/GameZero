using System.Collections.Generic;
using GamePlay.Core;
using UnityEngine.UIElements;
namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorStateDomain_Fighting : GameDirectorStateDomainBase
    {
        public GameDirectorStateDomain_Fighting(GameDirectorDomain directorDomain) : base(directorDomain)
        {
        }

        protected override void _BindEvents()
        {
        }

        protected override void _UnbindEvents()
        {
        }

        public override bool CheckEnter(GameDirectorEntity director, params object[] args)
        {
            return true;
        }

        public override void Enter(GameDirectorEntity director, params object[] args)
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

            // 提交RC
            GameDirectorRCArgs_StateEnterFighting rcArgs;
            rcArgs.fromStateType = fsmCom.lastStateType;
            this._context.SubmitRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHTING, rcArgs);
        }

        protected override GameDirectorExitStateArgs _CheckExit(GameDirectorEntity director)
        {
            var enemyCampId = GameCampCollection.ENEMY_CAMP_ID;
            var enemyCount = this._context.roleContext.repo.GetEntityCount((role) => role.idCom.campId == enemyCampId);
            var playerCampId = GameCampCollection.PLAYER_CAMP_ID;
            var playerCount = this._context.roleContext.repo.GetEntityCount((role) => role.idCom.campId == playerCampId);
            // 胜利条件: 敌人全部生成完毕且死亡
            var hasSpawnedAllEnemyUnits = this._context.domainApi.fieldApi.HasSpawnedAllEnemyUnits();
            if (hasSpawnedAllEnemyUnits && enemyCount == 0)
            {
                return new GameDirectorExitStateArgs(GameDirectorStateType.Settling, playerCount, enemyCount, true);
            }

            // 失败条件: 金币为0 且 玩家角色全部死亡
            var gold = this._context.director.gold;
            if (gold <= 0 && playerCount == 0)
            {
                return new GameDirectorExitStateArgs(GameDirectorStateType.Settling, playerCount, enemyCount, false);
            }

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
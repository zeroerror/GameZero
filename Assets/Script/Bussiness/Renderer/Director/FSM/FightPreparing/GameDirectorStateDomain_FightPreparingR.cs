using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Render
{
    public class GameDirectorStateDomain_FightPreparingR : GameDirectorStateDomainBaseR
    {
        public GameDirectorStateDomain_FightPreparingR(GameDirectorDomainR directorDomain) : base(directorDomain)
        {
        }

        public override void BindEvents()
        {
            this._context.BindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING, this._OnStateEnter);
            this._context.BindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING_POSITIONED, this._OnFightPreparingPositioned);
            this._context.uiApi.directorApi.BindKeyAction(KeyCode.Mouse0, this._OnClickUnit);
        }

        public override void UnbindEvents()
        {
            this._context.UnbindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING, this._OnStateEnter);
            this._context.UnbindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING_POSITIONED, this._OnFightPreparingPositioned);
            this._context.uiApi.directorApi.UnbindKeyAction(KeyCode.Mouse0, this._OnClickUnit);
        }

        private void _OnStateEnter(object args)
        {
            this.Enter(this._context.director, args);
        }

        private void _OnClickUnit()
        {
            var pointerPos = this._context.uiApi.directorApi.GetPointerPosition();
            var clickWorldPos = this._context.cameraEntity.GetWorldPoint(pointerPos);

            // 存在选择的单位, 再次点击将单位放置到当前点击位置
            var fightingState = this._context.director.fsmCom.fightingState;
            var chooseEntity = fightingState.chooseEntity;
            if (chooseEntity)
            {
                // 清空选择
                fightingState.chooseEntity = null;
                // 提交LC
                var ldirectorApi = this._context.logicApi.directorApi;
                ldirectorApi.SubmitEvent(GameLCCollection.LC_GAME_UNIT_POSITION_CHANGED, new GameLCArgs_UnitPositionChanged
                {
                    entityType = chooseEntity.idCom.entityType,
                    entityId = chooseEntity.idCom.entityId,
                    newPosition = clickWorldPos
                });
                return;
            }

            var clickUnit = this._context.domainApi.directorApi.GetClickEntity(clickWorldPos);
            if (!clickUnit) return;

            // 检查阵营
            var campId = clickUnit.idCom.campId;
            if (campId != GameCampCollection.PLAYER_CAMP_ID) return;

            fightingState.chooseEntity = clickUnit;
        }

        private void _OnFightPreparingPositioned(object args)
        {
        }

        public override void Enter(GameDirectorEntityR director, object args = null)
        {
            director.fsmCom.EnterFightPreparing();
            GameLogger.DebugLog("R 导演 - 进入战斗准备状态");
        }

        protected override void _Tick(GameDirectorEntityR director, float frameTime)
        {
        }

        public override void ExitTo(GameDirectorEntityR director, GameDirectorStateType toState)
        {
            base.ExitTo(director, toState);
            this._context.uiApi.directorApi.UnbindKeyAction(KeyCode.Mouse0, this._OnClickUnit);
        }

    }
}
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.UI;
using GamePlay.Core;
using GamePlay.Infrastructure;
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public class GameDirectorStateDomain_FightingR : GameDirectorStateDomainBaseR
    {
        public GameDirectorStateDomain_FightingR(GameDirectorDomainR directorDomain) : base(directorDomain)
        {
        }

        public override void BindEvents()
        {
            this._context.BindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHTING, this._OnStateEnter);
            this._context.uiApi.directorApi.BindKeyAction(KeyCode.Mouse0, this._OnClickUnit);
        }

        public override void UnbindEvents()
        {
            this._context.UnbindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHTING, this._OnStateEnter);
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
            var clickEntity = this._context.domainApi.directorApi.GetClickEntity(clickWorldPos);
            if (!clickEntity)
            {
                return;
            }
            var idCom = clickEntity.idCom;
            if (idCom.entityId > 0)
            {
                var input = new UIUnitDetailMainViewInput();
                input.entityType = idCom.entityType;
                input.entityId = idCom.entityId;
                this._context.uiApi.directorApi.OpenUI<UIUnitDetailMainView>(new UIViewInput(input));
            }
        }

        public override void Enter(GameDirectorEntityR director, object args = null)
        {
            director.fsmCom.EnterFighting();
            GameLogger.DebugLog("R 导演 - 进入战斗状态");
        }

        protected override void _Tick(GameDirectorEntityR director, float frameTime)
        {
        }

    }
}
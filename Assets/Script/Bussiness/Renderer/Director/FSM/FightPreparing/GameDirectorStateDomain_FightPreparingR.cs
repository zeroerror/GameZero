using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Renderer
{
    public class GameDirectorStateDomain_FightPreparingR : GameDirectorStateDomainBaseR
    {
        public GameDirectorStateDomain_FightPreparingR(GameDirectorDomainR directorDomain) : base(directorDomain)
        {
        }

        protected override void _BindEvents()
        {
            this._context.uiApi.directorApi.BindKeyAction(KeyCode.Mouse0, this._OnClickUnit);
        }

        protected override void _UnbindEvents()
        {
            this._context.uiApi.directorApi.UnbindKeyAction(KeyCode.Mouse0, this._OnClickUnit);
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

            // 角色
            {
                var repo = this._context.roleContext.repo;
                var clickRole = _GetClickUnit(clickWorldPos, repo);
                if (clickRole != null)
                {
                    fightingState.chooseEntity = clickRole;
                    return;
                }
            }
            // 其它
        }

        private static GameEntityBase _GetClickUnit(GameVec2 clickWorldPos, GameRoleRepoR repo)
        {
            const float width = 1.0f;
            const float height = 2.0f;
            var entityColliderModel = new GameBoxColliderModel(
                GameVec2.zero,
                0,
                width,
                height
            );
            var boxCollider = new GameBoxCollider(null, entityColliderModel, -1);
            var clickUnit = repo.Find((role) =>
            {
                boxCollider.UpdateTRS(role.transformCom.ToArgs());
                var mtv = GamePhysicsResolvingUtil.GetResolvingMTV(entityColliderModel, role.transformCom.ToArgs(), clickWorldPos);
                var isHit = mtv != GameVec2.zero;
                return isHit;
            });
            return clickUnit;
        }

        public override void Enter(GameDirectorEntityR director, object args = null)
        {
        }

        protected override void _Tick(GameDirectorEntityR director, float frameTime)
        {
        }

    }
}
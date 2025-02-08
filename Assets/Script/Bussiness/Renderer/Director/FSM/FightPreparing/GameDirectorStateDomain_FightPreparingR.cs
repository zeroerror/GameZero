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

        private void _OnFightPreparingPositioned(object args)
        {
        }

        private static GameEntityBase _GetClickUnit(GameVec2 clickWorldPos, GameRoleRepoR repo)
        {
            const float width = 1.0f;
            const float height = 2.0f;
            var entityColliderModel = new GameBoxColliderModel(
                new GameVec2(0, height / 2),
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
            if (!clickUnit) return null;

            // 检查阵营
            var campId = clickUnit.idCom.campId;
            if (campId != GameCampCollection.PLAYER_CAMP_ID)
            {
                GameLogger.LogWarning("点击单位不是玩家阵营");
                return null;
            }

            return clickUnit;
        }

        public override void Enter(GameDirectorEntityR director, object args = null)
        {
            // 镜头看向当前回合的区域位置
            this._context.domainApi.directorApi.LookAtRoundArea();

            director.fsmCom.EnterFightPreparing();
            GameLogger.DebugLog("R 导演 - 进入战斗准备状态");
        }

        protected override void _Tick(GameDirectorEntityR director, float frameTime)
        {
        }

    }
}
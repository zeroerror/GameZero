using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.UI;
using GamePlay.Core;
using UnityEngine;
namespace GamePlay.Bussiness.Render
{
    public class GameDirectorDomainR : GameDirectorDomainApiR
    {
        public GameDirectorEntityR director => this.context.director;

        public GameDirectorFSMDomainR directorFSMDomain { get; private set; }
        public GameDirectorFSMDomainApiR directorFSMApi => this.directorFSMDomain;

        public GameContextR context { get; private set; }
        public GameRoleDomainR roleDomain { get; private set; }
        public GameSkillDomainR skillDomain { get; private set; }
        public GameTransformDomainR transformDomain { get; private set; }
        public GameAttributeDomainR attributeDomain { get; private set; }
        public GameVFXDomain vfxDomain { get; private set; }
        public GameActionDomainR actionDomain { get; private set; }
        public GameDrawDomainR drawDomain { get; private set; }
        public GameProjectileDomainR projectileDomain { get; private set; }
        public GameFieldDomainR fieldDomain { get; private set; }
        public GameEntityCollectDomainR entityCollectDomain { get; private set; }
        public GameBuffDomainR buffDomain { get; private set; }
        public GameShaderEffectDomain shaderEffectDomain { get; private set; }

        public GameDirectorDomainR()
        {
            this.context = new GameContextR();
            this._InitDomain();
        }

        public void Inject(GameObject sceneRoot, GameDomainApi logicApi, UIDomainApi uiApi)
        {
            this._InitContext(sceneRoot, logicApi, uiApi);
            this._InjectContext();
        }

        private void _InitDomain()
        {
            this.directorFSMDomain = new GameDirectorFSMDomainR(this);
            this.roleDomain = new GameRoleDomainR();
            this.skillDomain = new GameSkillDomainR();
            this.transformDomain = new GameTransformDomainR();
            this.attributeDomain = new GameAttributeDomainR();
            this.vfxDomain = new GameVFXDomain();
            this.actionDomain = new GameActionDomainR();
            this.drawDomain = new GameDrawDomainR();
            this.projectileDomain = new GameProjectileDomainR();
            this.fieldDomain = new GameFieldDomainR();
            this.entityCollectDomain = new GameEntityCollectDomainR();
            this.buffDomain = new GameBuffDomainR();
            this.shaderEffectDomain = new GameShaderEffectDomain();
        }

        private void _InitContext(GameObject sceneRoot, GameDomainApi logicApi, UIDomainApi uiApi)
        {
            this.context.Inject(sceneRoot, logicApi, uiApi);
            this.context.domainApi.SetDirectorApi(this);
            this.context.domainApi.SetRoleApi(this.roleDomain);
            this.context.domainApi.SetSkillApi(this.skillDomain);
            this.context.domainApi.SetTransformApi(this.transformDomain);
            this.context.domainApi.SetAttributeApi(this.attributeDomain);
            this.context.domainApi.SetVFXApi(this.vfxDomain);
            this.context.domainApi.SetActionApi(this.actionDomain);
            this.context.domainApi.SetDrawApi(this.drawDomain);
            this.context.domainApi.SetProjectileApi(this.projectileDomain);
            this.context.domainApi.SetFieldApi(this.fieldDomain);
            this.context.domainApi.SetEntityCollectApi(this.entityCollectDomain);
            this.context.domainApi.SetBuffApi(this.buffDomain);
            this.context.domainApi.SetShaderEffectApi(this.shaderEffectDomain);
        }

        private void _InjectContext()
        {
            this.directorFSMDomain.Inject(this.context);
            this.roleDomain.Inject(this.context);
            this.skillDomain.Inject(this.context);
            this.transformDomain.Inject(this.context);
            this.attributeDomain.Inject(this.context);
            this.vfxDomain.Inject(this.context);
            this.actionDomain.Inject(this.context);
            this.drawDomain.Inject(this.context);
            this.projectileDomain.Inject(this.context);
            this.fieldDomain.Inject(this.context);
            this.entityCollectDomain.Inject(this.context);
            this.buffDomain.Inject(this.context);
            this.shaderEffectDomain.Inject(this.context);
        }

        public void Destroy()
        {
            this.UnbindEvents();
            this.directorFSMDomain.Destroy();
            this.roleDomain.Destroy();
            this.skillDomain.Destroy();
            this.transformDomain.Destroy();
            this.attributeDomain.Destroy();
            this.vfxDomain.Destroy();
            this.actionDomain.Destroy();
            this.drawDomain.Destroy();
            this.projectileDomain.Destroy();
            this.fieldDomain.Destroy();
            this.entityCollectDomain.Destroy();
            this.buffDomain.Destroy();
            this.shaderEffectDomain.Destroy();
        }

        public void BindEvents()
        {
            this.context.BindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_TIME_SCALE_CHANGE, this._OnTimeScaleChange);
            this.directorFSMDomain.BindEvents();
            this.roleDomain.BindEvents();
            this.skillDomain.BindEvents();
            this.transformDomain.BindEvents();
            this.attributeDomain.BindEvents();
            this.vfxDomain.BindEvents();
            this.actionDomain.BindEvents();
            this.drawDomain.BindEvents();
            this.projectileDomain.BindEvents();
            this.fieldDomain.BindEvents();
            this.entityCollectDomain.BindEvents();
            this.buffDomain.BindEvents();
            this.shaderEffectDomain.BindEvents();

            this.context.uiApi.directorApi.BindKeyAction(KeyCode.Mouse0, this._OnClickUnit);
        }

        public void UnbindEvents()
        {
            this.context.BindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_TIME_SCALE_CHANGE, this._OnTimeScaleChange);
            this.directorFSMDomain.UnbindEvents();
            this.roleDomain.UnbindEvents();
            this.skillDomain.UnbindEvents();
            this.transformDomain.UnbindEvents();
            this.attributeDomain.UnbindEvents();
            this.vfxDomain.UnbindEvents();
            this.actionDomain.UnbindEvents();
            this.drawDomain.UnbindEvents();
            this.projectileDomain.UnbindEvents();
            this.fieldDomain.UnbindEvents();
            this.entityCollectDomain.UnbindEvents();
            this.buffDomain.UnbindEvents();
            this.shaderEffectDomain.UnbindEvents();

            this.context.uiApi.directorApi.UnbindKeyAction(KeyCode.Mouse0, this._OnClickUnit);
        }

        private void _OnClickUnit()
        {
            var pointerPos = this.context.uiApi.directorApi.GetPointerPosition();
            var clickWorldPos = this.context.cameraEntity.GetWorldPoint(pointerPos);

            // 存在选择的单位, 再次点击将单位放置到当前点击位置
            var fightingState = this.context.director.fsmCom.fightingState;
            var chooseEntity = fightingState.chooseEntity;
            if (chooseEntity)
            {
                // 清空选择
                fightingState.chooseEntity = null;
                // 提交LC
                var ldirectorApi = this.context.logicApi.directorApi;
                ldirectorApi.SubmitEvent(GameLCCollection.LC_GAME_UNIT_POSITION_CHANGED, new GameLCArgs_UnitPositionChanged
                {
                    entityType = chooseEntity.idCom.entityType,
                    entityId = chooseEntity.idCom.entityId,
                    newPosition = clickWorldPos
                });
                return;
            }

            var clickUnit = this.context.domainApi.directorApi.GetClickEntity(clickWorldPos);
            if (!clickUnit) return;

            // 检查阵营
            var campId = clickUnit.idCom.campId;
            if (campId != GameCampCollection.PLAYER_CAMP_ID) return;

            fightingState.chooseEntity = clickUnit;
        }


        private void _OnTimeScaleChange(object args)
        {
            var timeScale = (float)args;
            this.context.director.timeScaleCom.SetTimeScale(timeScale);
        }

        private void _TickDomain(float dt)
        {
            if (this.context.fieldContext.curField == null) return;
            this.fieldDomain.Tick(dt);
            this.roleDomain.Tick(dt);
            this.skillDomain.Tick(dt);
            this.vfxDomain.Tick(dt);
            this.drawDomain.Tick(dt);
            this.projectileDomain.Tick(dt);
            this.entityCollectDomain.Tick(dt);
            this.buffDomain.Tick(dt);
            this.transformDomain.Tick(dt);
            this.shaderEffectDomain.Tick(dt);
        }

        public void Update(float dt)
        {
            var director = this.context.director;
            director.Tick(dt);
            dt *= director.timeScaleCom.timeScale;

            // 被上一帧延迟的RC事件, 需要在最开始处理
            this.context.delayRCEventService.Tick();
            // 触发本次逻辑的RC事件 ps: UI层无需再次触发 
            this.context.logicApi.directorApi.TickRCEvents();
            // 触发内部的事件
            this.context.eventService.Tick();

            // 导演状态机
            this.directorFSMDomain.Tick(this.director, dt);
            // 默认领域逻辑每帧更新
            this._TickDomain(dt);

            // 指令处理
            this.context.cmdBufferService.Tick();
        }

        public void LateUpdate(float dt)
        {
            dt *= this.context.director.timeScaleCom.timeScale;
            this.context.cameraEntity.Tick(dt);
        }

        public void SetTimeScale(float timeScale)
        {
            this.context.director.timeScaleCom.SetTimeScale(timeScale);
        }

        public Vector2 GetRoundAreaPosition()
        {
            var director = this.director;
            const int roundAreaHeight = 10;
            var pos = new Vector2(0, roundAreaHeight * (director.curRound - 1));
            return pos;
        }

        public Vector2 ScreenToWorldPos(in Vector2 screenPos)
        {
            var worldPos = this.context.cameraEntity.camera.ScreenToWorldPoint(screenPos);
            return worldPos;
        }

        public GameEntityBase GetClickEntity(in Vector2 clickWorldPos)
        {
            const float width = 2.0f;
            const float height = 2.0f;
            var entityColliderModel = new GameBoxColliderModel(
                new Vector2(0, height / 2),
                0,
                width,
                height
            );
            var clickEntity = this._GetClickEntity(this.context.roleContext.repo, entityColliderModel, clickWorldPos);
            return clickEntity;
        }

        private GameEntityBase _GetClickEntity<T>(GameEntityRepoBase<T> repo, GameBoxColliderModel colliderModel, Vector2 clickWorldPos) where T : GameEntityBase
        {
            var clickEntity = repo.Find((entity) =>
            {
                var mtv = GamePhysicsResolvingUtil.GetResolvingMTV(colliderModel, entity.transformCom.ToArgs(), clickWorldPos);
                var isHit = mtv != Vector2.zero;
                return isHit;
            });
            return clickEntity;
        }

        public GameEntityBase FindEntity(GameEntityType entityType, int entityId)
        {
            switch (entityType)
            {
                case GameEntityType.Role:
                    return this.context.domainApi.roleApi.FindByEntityId(entityId);
                default:
                    GameLogger.LogError("导演 - 获取实体失败, 未知的实体类型 " + entityType);
                    return null;
            }
        }

        public GameEntityBase CreatePreviewUnit(GameItemUnitModel unitModel)
        {
            var idArgs = new GameIdArgs();
            idArgs.entityType = unitModel.entityType;
            idArgs.typeId = unitModel.typeId;
            var transArgs = new GameTransformArgs();
            transArgs.scale = new Vector2(1, 1);
            switch (unitModel.entityType)
            {
                case GameEntityType.Role:
                    var role = this.roleDomain.CreateRole(idArgs, transArgs);
                    role.attributeBarCom.SetActive(false);
                    return role;
                default:
                    GameLogger.LogError("导演 - 创建预览单位失败, 未知的实体类型 " + unitModel.entityType);
                    return null;
            }
        }

        public void DestroyPreviewUnit(GameEntityBase entity)
        {
            if (!entity) return;
            if (entity is GameRoleEntityR role)
            {
                this.context.domainApi.roleApi.fsmApi.Enter(role, GameRoleStateType.Destroyed);
                this.context.domainApi.entityCollectApi.CollectEntity(role);
                return;
            }
            GameLogger.LogError("导演 - 销毁预览单位失败, 未知的实体类型 " + entity.idCom.entityType);
        }
    }
}
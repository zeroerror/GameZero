using System;
using System.Collections.Generic;
using Assets.HeroEditor4D.Common.Scripts.Common;
using GamePlay.Bussiness.Core;
using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorDomain : GameDirectorDomainApi
    {
        public GameDirectorEntity director => this.context.director;

        public GameDirectorFSMDomain directorFSMDomain { get; private set; }
        public GameDirectorFSMDomainApi directorFSMApi => this.directorFSMDomain;

        public GameContext context { get; private set; }
        public GameFieldDomain fieldDomain { get; private set; }
        public GameRoleInputDomain roleInputDomain { get; private set; }
        public GameRoleAIDomain roleAIDomain { get; private set; }
        public GameRoleDomain roleDomain { get; private set; }
        public GameSkillDomain skillDomain { get; private set; }
        public GameBuffDomain buffDomain { get; private set; }
        public GameProjectileDomain projectileDomain { get; private set; }
        public GameActionDomain actionDomain { get; private set; }
        public GameTransformDomain transformDomain { get; private set; }
        public GameAttributeDomain attributeDomain { get; private set; }
        public GamePhysicsDomain physicsDomain { get; private set; }
        public GameEntitySelectDomain entitySelectDomain { get; private set; }
        public GameEntityCollectDomain entityCollectDomain { get; private set; }

        public int curRound => this.director.curRound;

        public GameDirectorDomain()
        {
            this._InitDomain();
            this._InitContext();
            this._InjectContext();
        }

        private void _InitDomain()
        {
            this.directorFSMDomain = new GameDirectorFSMDomain(this);
            this.fieldDomain = new GameFieldDomain();
            this.roleInputDomain = new GameRoleInputDomain();
            this.roleAIDomain = new GameRoleAIDomain();
            this.roleDomain = new GameRoleDomain();
            this.skillDomain = new GameSkillDomain();
            this.buffDomain = new GameBuffDomain();
            this.projectileDomain = new GameProjectileDomain();
            this.actionDomain = new GameActionDomain();
            this.transformDomain = new GameTransformDomain();
            this.attributeDomain = new GameAttributeDomain();
            this.physicsDomain = new GamePhysicsDomain();
            this.entitySelectDomain = new GameEntitySelectDomain();
            this.entityCollectDomain = new GameEntityCollectDomain();
        }

        private void _InitContext()
        {
            this.context = new GameContext();
            this.context.domainApi.SetDirectorApi(this);
            this.context.domainApi.SetFieldApi(this.fieldDomain);
            this.context.domainApi.SetRoleAIApi(this.roleAIDomain);
            this.context.domainApi.SetRoleApi(this.roleDomain);
            this.context.domainApi.SetSkillApi(this.skillDomain);
            this.context.domainApi.SetBuffApi(this.buffDomain);
            this.context.domainApi.SetProjectileApi(this.projectileDomain);
            this.context.domainApi.SetActionApi(this.actionDomain);
            this.context.domainApi.SetTransformApi(this.transformDomain);
            this.context.domainApi.SetAttributeApi(this.attributeDomain);
            this.context.domainApi.SetPhysicsApi(this.physicsDomain);
            this.context.domainApi.SetEntitySelectApi(this.entitySelectDomain);
            this.context.domainApi.SetEntityCollectApi(this.entityCollectDomain);
        }

        private void _InjectContext()
        {
            this.directorFSMDomain.Inject(this.context);
            this.fieldDomain.Inject(this.context);
            this.roleInputDomain.Inject(this.context);
            this.roleAIDomain.Inject(this.context);
            this.roleDomain.Inject(this.context);
            this.skillDomain.Inject(this.context);
            this.buffDomain.Inject(this.context);
            this.projectileDomain.Inject(this.context);
            this.actionDomain.Inject(this.context);
            this.transformDomain.Inject(this.context);
            this.attributeDomain.Inject(this.context);
            this.physicsDomain.Inject(this.context);
            this.entitySelectDomain.Inject(this.context);
            this.entityCollectDomain.Inject(this.context);
            this.directorFSMDomain.TryEnter(this.director, GameDirectorStateType.Loading, 1);
        }

        public void Destroy()
        {
            this.directorFSMDomain.Destroy();
            this.fieldDomain.Destroy();
            this.roleInputDomain.Destroy();
            this.roleAIDomain.Destroy();
            this.roleDomain.Destroy();
            this.skillDomain.Destroy();
            this.buffDomain.Destroy();
            this.projectileDomain.Destroy();
            this.actionDomain.Destroy();
            this.transformDomain.Destroy();
            this.attributeDomain.Destroy();
            this.physicsDomain.Destroy();
            this.entitySelectDomain.Destroy();
            this.entityCollectDomain.Destroy();
        }

        public virtual void TickDomain(float dt)
        {
            this.fieldDomain.Tick(dt);
            if (this.context.fieldContext.curField == null) return;
            this.roleInputDomain.Tick();
            this.roleAIDomain.Tick(dt);
            this.roleDomain.Tick(dt);
            this.skillDomain.Tick(dt);
            this.buffDomain.Tick(dt);
            this.projectileDomain.Tick(dt);
            this.actionDomain.Tick(dt);
            this.transformDomain.Tick(dt);
            this.attributeDomain.Tick(dt);
            this.entitySelectDomain.Tick(dt);
            this.entityCollectDomain.Tick();
            this.physicsDomain.Tick();
        }

        public void BindEvents()
        {
            this.context.eventService.Bind(GameLCCollection.LC_GAME_UNIT_POSITION_CHANGED, this._onUnitPositionChanged);
        }

        public void UnbindEvents()
        {
            this.context.eventService.Unbind(GameLCCollection.LC_GAME_UNIT_POSITION_CHANGED, this._onUnitPositionChanged);
        }

        private void _onUnitPositionChanged(object args)
        {
            var rcArgs = (GameLCArgs_UnitPositionChanged)args;
            var entityType = rcArgs.entityType;
            var entityId = rcArgs.entityId;
            var unitEntity = this.context.domainApi.directorApi.FindUnitItemEntity(entityType, entityId);
            unitEntity.standPos = rcArgs.newPosition;
            GameLogger.DebugLog($"[{unitEntity.unitModel}]站位更新: {rcArgs.newPosition}");
        }

        public void Update(float dt)
        {
            var tickCount = this.director.Tick(dt);
            if (tickCount <= 0) return;
            var frameTime = GameTimeCollection.frameTime;
            for (var i = 0; i < tickCount; i++)
            {
                this.context.eventService.Tick();
                this.directorFSMDomain.Tick(this.director, frameTime);
                this.context.cmdBufferService.Tick();
                this._TickDirty();
            }
        }

        private void _TickDirty()
        {
            // 提交RC - 金币变更
            if (this.director.goldDirty)
            {
                this.director.goldDirty = false;
                GameDirectorRCArgs_GoldChange goldChangeArgs;
                goldChangeArgs.gold = this.director.gold;
                this.context.SubmitRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_COINS_CHANGE, goldChangeArgs);
            }
        }

        public void SetTimeScale(float timeScale)
        {
            this.director.timeScaleCom.SetTimeScale(timeScale);
        }

        public void TickRCEvents()
        {
            this.context.rcEventService.Tick();
        }

        public void BindRC(string rcName, Action<object> callback)
        {
            this.context.BindRC(rcName, callback);
        }

        public void UnbindRC(string rcName, Action<object> callback)
        {
            this.context.UnbindRC(rcName, callback);
        }

        public void SubmitEvent(string eventName, object args)
        {
            this.context.eventService.Submit(eventName, args);
        }

        public void BuyUnit(int index, in GameVec2 initPos)
        {
            var buyableUnits = this.director.buyableUnits;
            if (index < 0 || index >= buyableUnits.Count)
            {
                GameLogger.DebugLog($"购买单位失败, 超出可购买单位列表索引范围! index: {index}");
                return;
            }
            var unitModel = buyableUnits[index];
            if (unitModel == null)
            {
                GameLogger.DebugLog($"购买单位失败, 不存在的单位! index: {index}");
                return;
            }
            // 检查金币
            if (this.director.gold < buyableUnits[index].costGold)
            {
                GameLogger.DebugLog($"购买单位[{buyableUnits[index]}]失败, 金币不足! 当前持有金币: {this.director.gold}");
                return;
            }
            // 扣除金币
            this.director.gold -= buyableUnits[index].costGold;
            // 添加物件单位实体
            var unitEntity = new GameItemUnitEntity();
            unitEntity.unitModel = unitModel;
            unitEntity.standPos = initPos;
            this.director.itemUnitEntitys.Add(unitEntity);
            // 创建游戏单位
            var unit = this.CreateUnit(unitEntity);
            // 提交RC - 购买单位
            GameDirectorRCArgs_BuyUnit rcArgs;
            rcArgs.model = unitModel;
            rcArgs.costGold = unitModel.costGold;
            this.context.SubmitRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_BUY_UNIT, rcArgs);
        }

        public GameEntityBase CreateUnit(GameItemUnitEntity unitEntity)
        {
            var model = unitEntity.unitModel;
            var createPos = unitEntity.standPos;
            GameEntityBase entity = null;
            switch (model.entityType)
            {
                case GameEntityType.Role:
                    entity = this.context.domainApi.roleApi.CreatePlayerRole(model.typeId, new GameTransformArgs
                    {
                        position = createPos,
                        scale = GameVec2.one,
                        forward = GameVec2.right
                    }, true);
                    break;
                default:
                    GameLogger.LogError("导演 - 未知的单位实体类型 " + model.entityType);
                    break;
            }
            if (entity == null) return null;
            // 记录实体Id
            unitEntity.entityId = entity.idCom.entityId;
            // 设置属性
            if (unitEntity.attributeArgs.attributes?.HasData() == true)
            {
                entity.attributeCom.SetByArgs(unitEntity.attributeArgs);
            }
            if (unitEntity.baseAttributeArgs.attributes?.HasData() == true)
            {
                entity.attributeCom.SetByArgs(unitEntity.baseAttributeArgs);
            }
            return entity;
        }

        public GameItemUnitModel[] GetBuyableUnits()
        {
            return this.director.buyableUnits.ToArray();
        }

        public bool ShuffleBuyableUnits(bool isFree)
        {
            // 检查是否可以洗牌
            if (!isFree)
            {
                var gold = this.director.gold;
                const int shuffleCost = 10;
                if (gold < shuffleCost)
                {
                    GameLogger.DebugLog($"洗牌失败, 金币不足! 需要金币: {shuffleCost}, 当前持有金币: {gold}");
                    return false;
                }
                // 扣除金币
                this.director.gold -= shuffleCost;
            }

            var unitPool = this.director.unitPool;
            if (unitPool == null)
            {
                unitPool = this._InitUnitPool();
                this.director.SetUnitPool(unitPool);
            }

            // 把当前可购买单位列表放回到总单位列表
            var buyableUnits = this.director.buyableUnits;
            foreach (var unit in buyableUnits)
            {
                unitPool.Add(unit);
            }

            // 洗牌出最多5个单位
            buyableUnits.Clear();
            int shffleCount = GameMath.Min(5, unitPool.Count);
            for (var i = 0; i < shffleCount; i++)
            {
                var unit = GameRandomService.GetRandom(unitPool);
                if (unit == null) continue;
                buyableUnits.Add(unit);
                unitPool.Remove(unit);
            }
            return true;
        }

        private List<GameItemUnitModel> _InitUnitPool()
        {
            var unitPool = new List<GameItemUnitModel>();
            // 角色模板
            var roleTemplate = this.context.domainApi.roleApi.GetRoleTemplate();
            roleTemplate.GetAll()?.Foreach((role) =>
            {
                var unit = new GameItemUnitModel(
                    GameEntityType.Role,
                    role.typeId,
                    10
                );
                unitPool.Add(unit);
            });
            return unitPool;
        }

        public GameEntityBase FindUnitEntity(GameItemUnitEntity itemEntity)
        {
            var isBought = this.director.itemUnitEntitys.Contains(itemEntity);
            if (!isBought) return null;
            switch (itemEntity.unitModel.entityType)
            {
                case GameEntityType.Role:
                    return this.context.domainApi.roleApi.FindByEntityId(itemEntity.entityId);
                default:
                    GameLogger.LogError("导演 - 未知的单位类型 " + itemEntity.unitModel.entityType);
                    return null;
            }
        }

        public GameEntityBase FindUnitEntity(GameEntityType entityType, int entityId)
        {
            var boughtUnit = this.director.itemUnitEntitys.Find((item) => item.entityId == entityId && item.unitModel.entityType == entityType);
            if (boughtUnit == null) return null;
            return this.FindUnitEntity(boughtUnit);
        }

        public GameItemUnitEntity FindUnitItemEntity(GameEntityType entityType, int entityId)
        {
            var boughtUnit = this.director.itemUnitEntitys.Find((item) => item.entityId == entityId && item.unitModel.entityType == entityType);
            return boughtUnit;
        }

        public void CleanBattleField()
        {
            var curField = this.context.fieldContext.curField;
            if (!curField) return;
            curField.Clear();
            this.context.domainApi.roleApi.RemoveAllRoles();
            this.context.domainApi.projectileApi.RemoveAllProjectiles();
            // 提交RC
            this.context.SubmitRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_CLEAR_FIELD, new GameDirectorRCArgs_CleanBattleField { fieldId = curField.model.typeId });
        }

        public GameEntityBase FindEntity(in GameIdArgs idArgs)
        {
            switch (idArgs.entityType)
            {
                case GameEntityType.Role:
                    return this.context.domainApi.roleApi.FindByEntityId(idArgs.entityId);
                default:
                    GameLogger.LogError("导演 - 获取实体失败, 未知的实体类型 " + idArgs.entityType);
                    return null;
            }
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
    }
}
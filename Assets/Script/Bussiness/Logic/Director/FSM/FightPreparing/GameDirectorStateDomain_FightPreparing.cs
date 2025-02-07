using System.Collections.Generic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorStateDomain_FightPreparing : GameDirectorStateDomainBase
    {
        public GameDirectorStateDomain_FightPreparing(GameDirectorDomain directorDomain) : base(directorDomain)
        {
        }

        protected override void _BindEvents()
        {
            this._context.eventService.Bind(GameLCCollection.LC_GAME_ACTION_OPTION_SELECTED, this._onActionOptionSelected);
            this._context.eventService.Bind(GameLCCollection.LC_GAME_PREPARING_CONFIRM_FIGHT, this._onPreparingConfirmFight);
            this._context.eventService.Bind(GameLCCollection.LC_GAME_UNIT_POSITION_CHANGED, this._onUnitPositionChanged);
        }

        protected override void _UnbindEvents()
        {
            this._context.eventService.Unbind(GameLCCollection.LC_GAME_ACTION_OPTION_SELECTED, this._onActionOptionSelected);
            this._context.eventService.Unbind(GameLCCollection.LC_GAME_PREPARING_CONFIRM_FIGHT, this._onPreparingConfirmFight);
            this._context.eventService.Unbind(GameLCCollection.LC_GAME_UNIT_POSITION_CHANGED, this._onUnitPositionChanged);
        }

        private void _onActionOptionSelected(object args)
        {
            if (!this._CheckState()) return;

            var rcArgs = (GameLCArgs_ActionOptionSelected)args;
            var optionId = rcArgs.optionId;
            var fightPreparingState = this._context.director.fsmCom.fightPreparingState;
            var options = fightPreparingState.options;
            var selectedOption = options.Find((option) => option.typeId == optionId);
            if (selectedOption == null)
            {
                GameLogger.LogError("GameDirectorStateDomain_FightPreparing._onActionOptionSelected: 未找到选项 " + optionId);
                return;
            }
            fightPreparingState.selectedOption = selectedOption;
        }

        private void _onPreparingConfirmFight(object args)
        {
            if (!this._CheckState()) return;

            var rcArgs = (GameLCArgs_PreparingConfirmFight)args;
            var fightPreparingState = this._context.director.fsmCom.fightPreparingState;
            fightPreparingState.confirmFight = true;
        }

        private void _onUnitPositionChanged(object args)
        {
            if (!this._CheckState()) return;

            var rcArgs = (GameLCArgs_UnitPositionChanged)args;
            var entityType = rcArgs.entityType;
            var entityId = rcArgs.entityId;
            GameEntityBase entity;
            switch (entityType)
            {
                case GameEntityType.Role:
                    entity = this._context.domainApi.roleApi.FindByEntityId(entityId);
                    if (!entity)
                    {
                        GameLogger.LogError("GameDirectorStateDomain_FightPreparing._onUnitPositionChanged: 未找到角色 " + entityId);
                        break;
                    }
                    break;
                default:
                    GameLogger.LogError("GameDirectorStateDomain_FightPreparing._onUnitPositionChanged: 未知的实体类型 " + entityType);
                    return;
            }

            // 检查阵营
            var campId = entity.idCom.campId;
            if (campId != GameCampCollection.PLAYER_CAMP_ID)
            {
                GameLogger.LogWarning("点击单位不是玩家阵营");
                return;
            }

            entity.transformCom.position = rcArgs.newPosition;
            GameLogger.DebugLog($"单位位置改变 - 角色 {entityId} 移动到 {rcArgs.newPosition}");
        }

        private bool _CheckState()
        {
            var stateType = this._context.director.fsmCom.stateType;
            if (stateType != GameDirectorStateType.FightPreparing)
            {
                GameLogger.LogError("GameDirectorStateDomain_FightPreparing._onActionOptionSelected: 当前状态不是战斗准备状态");
                return false;
            }
            return true;
        }

        public override bool CheckEnter(GameDirectorEntity director, object args = null)
        {
            return true;
        }

        public override void Enter(GameDirectorEntity director, object args = null)
        {
            var actionOptions = this._getRandomActionOptions(3);
            var fsmCom = director.fsmCom;
            fsmCom.EnterFightPreparing(actionOptions);
            GameLogger.DebugLog("导演 - 进入战斗准备状态");
            // 洗牌可购买单位列表
            this._context.domainApi.directorApi.ShuffleBuyableUnits(true);
            // 提交RC
            GameDirectorRCArgs_StateEnterFightPreparing rcArgs;
            rcArgs.fromStateType = fsmCom.lastStateType;
            rcArgs.actionOptions = actionOptions;
            this._context.SubmitRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING, rcArgs);
        }

        protected override void _Tick(GameDirectorEntity director, float dt)
        {
            this._directorDomain.fieldDomain.Tick(dt);
            if (this._context.fieldContext.curField == null) return;
            this._directorDomain.roleDomain.Tick(dt);
            this._directorDomain.skillDomain.Tick(dt);
            this._directorDomain.buffDomain.Tick(dt);
            this._directorDomain.projectileDomain.Tick(dt);
            this._directorDomain.actionDomain.Tick(dt);
            this._directorDomain.transformDomain.Tick(dt);
            this._directorDomain.attributeDomain.Tick(dt);
            this._directorDomain.entitySelectDomain.Tick(dt);
            this._directorDomain.entityCollectDomain.Tick();
            this._directorDomain.physicsDomain.Tick();
        }

        protected override GameDirectorExitStateArgs _CheckExit(GameDirectorEntity director)
        {
            // 一直等待, 直到玩家选择了一个选项, 并且确认了开始战斗
            var fightPreparingState = director.fsmCom.fightPreparingState;
            var selectedOptionModel = fightPreparingState.selectedOption;
            if (selectedOptionModel == null || !fightPreparingState.confirmFight)
            {
                return new GameDirectorExitStateArgs(GameDirectorStateType.None);
            }

            // 执行所有选项的行为
            var playerCampId = GameCampCollection.PLAYER_CAMP_ID;
            var optionRepo = this._context.actionContext.optionRepo;
            var optionId = selectedOptionModel.typeId;
            var optionEntity = optionRepo.FindOption(playerCampId, optionId);
            if (optionEntity)
            {
                optionEntity.AddLevel();
            }
            optionRepo.ForeachEntities((option) =>
            {
                this._context.domainApi.actionApi.DoActionOption(optionId, playerCampId);
            });
            if (!optionEntity)
            {
                this._context.domainApi.actionApi.DoActionOption(optionId, playerCampId);
            }
            return new GameDirectorExitStateArgs(GameDirectorStateType.Fighting);
        }

        private List<GameActionOptionModel> _getRandomActionOptions(int count)
        {
            var actionApi = this._context.domainApi.actionApi;
            var list = actionApi.GetActionOptionModelList();
            var result = new List<GameActionOptionModel>();
            for (int i = 0; i < count; i++)
            {
                var index = GameMath.RandomRange(0, list.Count);
                var model = list[index];
                if (model == null)
                {
                    i--;
                    continue;
                }
                result.Add(model);
                list[index] = null;
            }
            return result;
        }
    }
}
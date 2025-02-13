using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using GamePlay.Bussiness.Core;
using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;

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
            GameLogger.DebugLog($"选中行为选项[{selectedOption.typeId}]: {selectedOption.desc}");
            fightPreparingState.selectedOptionList.Add(selectedOption);
        }

        private void _onPreparingConfirmFight(object args)
        {
            var fsmCom = this._context.director.fsmCom;
            if (fsmCom.stateType != GameDirectorStateType.FightPreparing)
            {
                GameLogger.LogError("GameDirectorStateDomain_FightPreparing._onPreparingConfirmFight: 当前状态不是战斗准备状态");
                return;
            }
            var fightPreparingState = fsmCom.fightPreparingState;
            var rcArgs = (GameLCArgs_PreparingConfirmFight)args;
            fightPreparingState.confirmFight = true;
        }

        private void _onUnitPositionChanged(object args)
        {
            var rcArgs = (GameLCArgs_UnitPositionChanged)args;
            var entityType = rcArgs.entityType;
            var entityId = rcArgs.entityId;
            var unitEntity = this._context.domainApi.directorApi.FindUnitItemEntity(entityType, entityId);
            unitEntity.standPos = rcArgs.newPosition;
            // 设置所有单位是否就位为false, 使得单位重新移动到指定位置
            this._context.director.fsmCom.fightPreparingState.isAllUnitPositioned = false;
            GameLogger.DebugLog($"[{unitEntity.unitModel}]站位更新: {rcArgs.newPosition}");
        }

        public override bool CheckEnter(GameDirectorEntity director, params object[] args)
        {
            var stateType = director.fsmCom.stateType;
            if (stateType == GameDirectorStateType.FightPreparing)
            {
                GameLogger.LogError("GameDirectorStateDomain_FightPreparing.CheckEnter: 当前状态已经是战斗准备状态");
                return false;
            }
            return true;
        }

        public override void Enter(GameDirectorEntity director, params object[] args)
        {
            // 洗牌可购买单位列表
            this._context.domainApi.directorApi.ShuffleBuyableUnits(true);

            // 切换状态机组件状态
            var actionOptions = this._getRandomActionOptions(3);
            var fsmCom = director.fsmCom;
            fsmCom.EnterFightPreparing(actionOptions);
            GameLogger.DebugLog("导演 - 进入战斗准备状态");

            // 提交RC
            GameDirectorRCArgs_StateEnterFightPreparing rcArgs;
            rcArgs.fromStateType = fsmCom.lastStateType;
            rcArgs.actionOptions = actionOptions;
            this._context.SubmitRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING, rcArgs);
        }

        protected override void _Tick(GameDirectorEntity director, float dt)
        {
            this._TryMoveUnits();
            // 业务逻辑
            this._directorDomain.roleInputDomain.Tick();
            this._directorDomain.roleDomain.Tick(dt);
            this._directorDomain.attributeDomain.Tick(dt);
            this._directorDomain.transformDomain.Tick(dt);
            this._directorDomain.physicsDomain.Tick();
        }

        private void _TryMoveUnits()
        {
            var director = this._context.director;
            var fightPreparingState = director.fsmCom.fightPreparingState;
            if (fightPreparingState.isAllUnitPositioned) return;
            var unitEntitys = director.itemUnitEntitys;
            if (unitEntitys == null) return;

            bool isAllUnitPositioned = true;
            unitEntitys.ForEach((unitEntity) =>
            {
                var unit = this._context.domainApi.directorApi.FindUnitEntity(unitEntity);
                if (unit == null) return;
                var moveDstPos = unitEntity.standPos;
                var isPositioned = moveDstPos == unit.transformCom.position;
                if (isPositioned) return;
                isAllUnitPositioned = false;
                switch (unit.idCom.entityType)
                {
                    case GameEntityType.Role:
                        // 角色移动输入
                        var role = unit as GameRoleEntity;
                        var inputArgs = new GameRoleInputArgs();
                        inputArgs.moveDst = moveDstPos;
                        this._context.domainApi.roleApi.SetRoleInput(unit.idCom.entityId, inputArgs);
                        break;
                    default:
                        GameLogger.LogError("GameDirectorStateDomain_FightPreparing._TryMoveUnits: 未知的实体类型 " + unit.idCom.entityType);
                        break;
                }
            });
            fightPreparingState.isAllUnitPositioned = isAllUnitPositioned;
            // 提交RC
            if (isAllUnitPositioned)
            {
                this._context.SubmitRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING_POSITIONED, null);
            }
        }

        protected override GameDirectorExitStateArgs _CheckExit(GameDirectorEntity director)
        {
            // 等待玩家确认开始战斗
            var fightPreparingState = director.fsmCom.fightPreparingState;
            if (!fightPreparingState.confirmFight)
            {
                return new GameDirectorExitStateArgs(GameDirectorStateType.None);
            }

            // 执行所有选项的行为
            var playerCampId = GameCampCollection.PLAYER_CAMP_ID;
            var optionRepo = this._context.actionContext.optionRepo;
            var selectedOptionList = fightPreparingState.selectedOptionList;
            selectedOptionList?.ForEach((option) =>
            {
                var optionId = option?.typeId ?? 0;
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
            });
            return new GameDirectorExitStateArgs(GameDirectorStateType.Fighting);
        }

        private List<GameActionOptionModel> _getRandomActionOptions(int count)
        {
            var actionApi = this._context.domainApi.actionApi;
            var list = actionApi.GetActionOptionModelList();
            var result = new List<GameActionOptionModel>();
            for (int i = 0; i < count; i++)
            {
                var index = GameRandomService.GetRandom(0, list.Count);
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

        public override void ExitTo(GameDirectorEntity director, GameDirectorStateType toState)
        {
            base.ExitTo(director, toState);
        }
    }
}
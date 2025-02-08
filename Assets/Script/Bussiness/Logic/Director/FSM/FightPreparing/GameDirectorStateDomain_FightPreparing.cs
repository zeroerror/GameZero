using System.Collections.Generic;
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
            GameLogger.DebugLog($"选中行为选项[{selectedOption.typeId}]: {selectedOption.desc}");
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
                        return;
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
            var stateType = director.fsmCom.stateType;
            if (stateType == GameDirectorStateType.FightPreparing)
            {
                GameLogger.LogError("GameDirectorStateDomain_FightPreparing.CheckEnter: 当前状态已经是战斗准备状态");
                return false;
            }
            return true;
        }

        public override void Enter(GameDirectorEntity director, object args = null)
        {
            // 洗牌可购买单位列表
            this._context.domainApi.directorApi.ShuffleBuyableUnits(true);

            // 更新回合数
            director.curRound++;
            GameLogger.DebugLog("L 导演 - 进入第 " + director.curRound + " 回合");

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
            this._directorDomain.roleDomain.Tick(dt);
            this._directorDomain.attributeDomain.Tick(dt);
            this._directorDomain.transformDomain.Tick(dt);
        }

        private void _TryMoveUnits()
        {
            var director = this._context.director;
            var fightPreparingState = director.fsmCom.fightPreparingState;
            if (fightPreparingState.isAllUnitPositioned) return;
            var unitEntitys = director.unitEntitys;
            if (unitEntitys == null) return;

            bool isAllUnitPositioned = true;
            unitEntitys.ForEach((unitEntity) =>
            {
                var unit = this._context.domainApi.directorApi.FindUnit(unitEntity);
                if (unit == null) return;
                var moveDstPos = this._context.domainApi.directorApi.GetRoundAreaPosition();
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
                        role.inputCom.SetByArgs(inputArgs);
                        break;
                    default:
                        GameLogger.LogError("GameDirectorStateDomain_FightPreparing._TryMoveUnits: 未知的实体类型 " + unit.idCom.entityType);
                        break;
                }
            });
            fightPreparingState.isAllUnitPositioned = isAllUnitPositioned;
        }

        protected override GameDirectorExitStateArgs _CheckExit(GameDirectorEntity director)
        {
            // 一直等待, 直到玩家选择了一个选项, 并且确认了开始战斗
            var fightPreparingState = director.fsmCom.fightPreparingState;
            var selectedOptionModel = fightPreparingState.selectedOption;
            if (selectedOptionModel == null || !fightPreparingState.confirmFight || !fightPreparingState.isAllUnitPositioned)
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
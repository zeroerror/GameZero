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
        }

        protected override void _UnbindEvents()
        {
            this._context.eventService.Unbind(GameLCCollection.LC_GAME_ACTION_OPTION_SELECTED, this._onActionOptionSelected);
            this._context.eventService.Unbind(GameLCCollection.LC_GAME_PREPARING_CONFIRM_FIGHT, this._onPreparingConfirmFight);
        }

        private void _onActionOptionSelected(object args)
        {
            var evArgs = (GameLCArgs_ActionOptionSelected)args;
            var optionId = evArgs.optionId;
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
            var evArgs = (GameLCArgs_PreparingConfirmFight)args;
            var fightPreparingState = this._context.director.fsmCom.fightPreparingState;
            fightPreparingState.confirmFight = true;
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
            this._context.domainApi.directorApi.ShuffleBuyableUnits();
            // 提交RC
            GameDirectorRCArgs_StateEnterFightPreparing rcArgs;
            rcArgs.fromStateType = fsmCom.lastStateType;
            rcArgs.actionOptions = actionOptions;
            this._context.SubmitRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING, rcArgs);
        }

        protected override void _Tick(GameDirectorEntity director, float frameTime)
        {
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
            var playerCampId = GameRoleCollection.PLAYER_ROLE_CAMP_ID;
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
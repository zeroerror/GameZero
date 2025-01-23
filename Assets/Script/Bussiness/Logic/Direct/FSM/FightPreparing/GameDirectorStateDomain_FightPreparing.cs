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
        }

        protected override void _UnbindEvents()
        {
            this._context.eventService.Unbind(GameLCCollection.LC_GAME_ACTION_OPTION_SELECTED, this._onActionOptionSelected);
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

            // 执行所有选项的行为
            var userRole = this._context.roleContext.userRole;
            var playerCampId = userRole.idCom.campId;
            var optionRepo = this._context.actionContext.optionRepo;
            var option = optionRepo.FindOption(userRole.idCom.campId, optionId);
            if (option)
            {
                option.AddLevel();
            }
            optionRepo.ForeachEntities((option) =>
            {
                this._context.domainApi.actionApi.DoActionOption(option.model.typeId, playerCampId);
            });
            if (!option)
            {
                this._context.domainApi.actionApi.DoActionOption(optionId, playerCampId);
            }
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

            // 提交RC
            this._context.SubmitRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING, new GameDirectorRCArgs_StateEnterFightPreparing
            {
                fromStateType = fsmCom.lastStateType,
                actionOptions = actionOptions,
            });
        }

        protected override void _Tick(GameDirectorEntity director, float frameTime)
        {
        }

        protected override (GameDirectorStateType, object) _CheckExit(GameDirectorEntity director)
        {
            // 一直等待, 直到玩家选择了一个选项
            var fsmCom = director.fsmCom;
            if (fsmCom.fightPreparingState.selectedOption != null)
            {
                return (GameDirectorStateType.Fighting, 1);
            }
            return (GameDirectorStateType.None, null);
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
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_action_", menuName = "游戏玩法/配置/行为模板")]
    public class GameActionSO : GameSOBase
    {
        public GameActionType actionType;
        public string desc;
        public GameActionModel_Dmg dmgAction;
        public GameActionModel_Heal healAction;
        public GameActionEM_LaunchProjectile launchProjectileActionEM;

        public GameActionEMR actionEMR;
        public GameEntitySelectorEM selectorEM;

        public GameSkillSO[] skillSORefs;

        public GameActionModelBase GetActionModel()
        {
            GameActionModelBase actionModel;
            switch (actionType)
            {
                case GameActionType.Dmg:
                    actionModel = dmgAction;
                    break;
                case GameActionType.Heal:
                    actionModel = healAction;
                    break;
                case GameActionType.LaunchProjectile:
                    actionModel = launchProjectileActionEM.ToModel();
                    break;
                default:
                    GameLogger.LogError("GameActionSO: GetAction: invalid actionType: " + actionType);
                    return null;
            }
            this._SyncToActionModel(actionModel);
            return actionModel;
        }

        private void _SyncToActionModel(GameActionModelBase actionModel)
        {
            if (actionModel == null) return;
            actionModel.selector = this.selectorEM.ToSelector();
            actionModel.actionType = this.actionType;
            actionModel.typeId = this.typeId;
            this._CorrectModel(actionModel);
        }
        private void _CorrectModel(GameActionModelBase actionModel)
        {
            var selector = actionModel.selector;
            var selectAnchorType = actionModel.selector.selectAnchorType;
            if (selectAnchorType == GameEntitySelectAnchorType.Self && selectorEM.selColliderType == GameColliderType.None && this.selectorEM.campType != GameCampType.Ally)
            {
                GameLogger.LogWarning("选择器锚点类型为自身的单选行为，必须选择友方阵营, 否则永远无法满足选取条件");
                selector.campType = GameCampType.Ally;
            }
        }
    }
}
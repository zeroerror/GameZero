using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_action_", menuName = "游戏玩法/配置/行为模板")]
    public class GameActionSO : GameSOBase
    {
        public GameActionType actionType_edit;
        public GameEntitySelector selector_edit;
        public GameColliderType colliderType_edit;

        public bool isRangeSelect;
        public GameBoxColliderModel boxColliderModel;
        public GameCircleColliderModel circleColliderModel;
        public GameFanColliderModel fanColliderModel;

        public GameActionModel_Dmg dmgAction;
        public GameActionModel_Heal healAction;
        public GameActionModel_LaunchProjectile launchProjectileAction;
        public GameProjectileSO launchProjectileSO;
        public GameSkillSO[] skillSORefs;
        public GameActionModelR actionR;

        public GameActionModelBase GetActionModel()
        {
            GameActionModelBase actionModel;
            switch (actionType_edit)
            {
                case GameActionType.Dmg:
                    actionModel = dmgAction;
                    break;
                case GameActionType.Heal:
                    actionModel = healAction;
                    break;
                case GameActionType.LaunchProjectile:
                    actionModel = launchProjectileAction;
                    break;
                default:
                    GameLogger.LogError("GameActionSO: GetAction: invalid actionType: " + actionType_edit);
                    return null;
            }
            this.SyncToActionModel(actionModel);
            return actionModel;
        }

        public void SyncToActionModel(GameActionModelBase actionModel)
        {
            if (actionModel == null) return;
            if (!this.isRangeSelect) selector_edit.colliderModel = null;
            else selector_edit.colliderModel = _GetEditColliderModel();
            actionModel.selector = this.selector_edit;
            actionModel.actionType = this.actionType_edit;
            actionModel.typeId = this.typeId;
            this._CorrectModel(actionModel);
        }

        private void _CorrectModel(GameActionModelBase actionModel)
        {
            var selector = actionModel.selector;
            var selectAnchorType = actionModel.selector.selectAnchorType;
            if (selectAnchorType == GameEntitySelectAnchorType.Self && !this.isRangeSelect && this.selector_edit.campType != GameCampType.Ally)
            {
                GameLogger.LogWarning("选择器锚点类型为自身的单选行为，必须选择友方阵营, 否则永远无法满足选取条件");
                selector.campType = GameCampType.Ally;
            }
        }

        private GameColliderModelBase _GetEditColliderModel()
        {
            switch (colliderType_edit)
            {
                case GameColliderType.Box:
                    return boxColliderModel;
                case GameColliderType.Circle:
                    return circleColliderModel;
                case GameColliderType.Fan:
                    return fanColliderModel;
                default:
                    GameLogger.LogError("GameActionSO: GetColliderModel: invalid colliderType: " + colliderType_edit);
                    return null;
            }
        }
    }
}
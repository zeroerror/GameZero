using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using GamePlay.Core;
using Unity.VisualScripting;
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
            GameActionModelBase actionModel = null;
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
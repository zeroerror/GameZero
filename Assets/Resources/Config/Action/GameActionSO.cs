using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_action_", menuName = "游戏玩法/配置/行为模板")]
    public class GameActionSO : GameSOBase
    {
        public GameActionType actionType;
        public string desc;
        public GameActionEM_Dmg dmgActionEM;
        public GameActionEM_Heal healActionEM;
        public GameActionEM_LaunchProjectile launchProjectileActionEM;
        public GameActionEM_KnockBack knockBackActionEM;

        public GameActionEMR actionEMR;

        public GameSkillSO[] skillSORefs;

        public GameActionModelBase GetActionModel()
        {
            GameActionModelBase actionModel;
            switch (actionType)
            {
                case GameActionType.Dmg:
                    actionModel = dmgActionEM.ToModel();
                    break;
                case GameActionType.Heal:
                    actionModel = healActionEM.ToModel();
                    break;
                case GameActionType.LaunchProjectile:
                    actionModel = launchProjectileActionEM.ToModel();
                    break;
                case GameActionType.KnockBack:
                    actionModel = knockBackActionEM.ToModel();
                    break;
                default:
                    GameLogger.LogError("GameActionSO: GetAction: invalid actionType: " + actionType);
                    return null;
            }
            this._SyncToActionModel(actionModel);
            return actionModel;
        }

        /// <summary> 同步SO的参数到行为模型 </summary>
        private void _SyncToActionModel(GameActionModelBase actionModel)
        {
            if (actionModel == null) return;
            actionModel.actionType = this.actionType;
            actionModel.typeId = this.typeId;
            this._CorrectModel(actionModel);
        }
        private void _CorrectModel(GameActionModelBase actionModel)
        {
            var selector = actionModel.selector;
            var selectAnchorType = actionModel.selector.selectAnchorType;
            if (selectAnchorType == GameEntitySelectAnchorType.Self)
            {
                var selectorEM = this.GetCurSelectorEM();
                if (selectorEM?.selColliderType == GameColliderType.None && selectorEM?.campType != GameCampType.Ally && selectorEM?.campType != GameCampType.None)
                {
                    selector.campType = GameCampType.Ally;
                }
            }
        }

        public GameEntitySelectorEM GetCurSelectorEM()
        {
            switch (actionType)
            {
                case GameActionType.Dmg:
                    return this.dmgActionEM.selectorEM;
                case GameActionType.Heal:
                    return this.healActionEM.selectorEM;
                case GameActionType.LaunchProjectile:
                    return this.launchProjectileActionEM.selectorEM;
                case GameActionType.KnockBack:
                    return this.knockBackActionEM.selectorEM;
                default:
                    GameLogger.LogError("GameActionSO: GetSelectorEM: invalid actionType: " + actionType);
                    return null;
            }
        }
    }
}
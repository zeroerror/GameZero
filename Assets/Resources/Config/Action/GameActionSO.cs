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
        public GameActionEM_AttributeModify attributeModifyActionEM;
        public GameActionEM_LaunchProjectile launchProjectileActionEM;
        public GameActionEM_KnockBack knockBackActionEM;
        public GameActionEM_AttachBuff attachBuffActionEM;
        public GameActionEM_SummonRoles summonRolesActionEM;

        public GameActionEMR actionEMR;

        public GameSkillSO[] skillSORefs;

        public void OnEnable()
        {
            if (actionType == GameActionType.None) actionType = GameActionType.Dmg;
        }

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
                case GameActionType.AttributeModify:
                    actionModel = attributeModifyActionEM.ToModel();
                    break;
                case GameActionType.LaunchProjectile:
                    actionModel = launchProjectileActionEM.ToModel();
                    break;
                case GameActionType.KnockBack:
                    actionModel = knockBackActionEM.ToModel();
                    break;
                case GameActionType.AttachBuff:
                    actionModel = attachBuffActionEM.ToModel();
                    break;
                case GameActionType.SummonRoles:
                    actionModel = summonRolesActionEM.ToModel();
                    break;
                default:
                    GameLogger.LogError("GameActionSO: GetAction: invalid actionType: " + actionType);
                    return null;
            }
            this._SyncToActionModel(actionModel);
            return actionModel;
        }

        public GameEntitySelectorEM GetCurSelectorEM()
        {
            switch (actionType)
            {
                case GameActionType.Dmg:
                    return this.dmgActionEM.selectorEM;
                case GameActionType.Heal:
                    return this.healActionEM.selectorEM;
                case GameActionType.AttributeModify:
                    return this.attributeModifyActionEM.selectorEM;
                case GameActionType.LaunchProjectile:
                    return this.launchProjectileActionEM.selectorEM;
                case GameActionType.KnockBack:
                    return this.knockBackActionEM.selectorEM;
                case GameActionType.AttachBuff:
                    return this.attachBuffActionEM.selectorEM;
                case GameActionType.SummonRoles:
                    return this.summonRolesActionEM.selectorEM;
                default:
                    GameLogger.LogError("未处理的行为类型: " + actionType);
                    return null;
            }
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
            if (selectAnchorType == GameEntitySelectAnchorType.Actor)
            {
                var selectorEM = this.GetCurSelectorEM();
                if (selectorEM?.selColliderType == GameColliderType.None && selectorEM?.campType != GameCampType.Ally && selectorEM?.campType != GameCampType.None)
                {
                    selector.campType = GameCampType.Ally;
                }
            }
        }
    }
}
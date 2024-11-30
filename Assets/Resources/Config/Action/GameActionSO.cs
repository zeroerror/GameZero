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
        public GameEntitySelector selector;

        public bool isRangeSelect;
        public GameColliderType colliderType_edit;
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
            switch (actionType)
            {
                case GameActionType.Dmg:
                    return dmgAction;
                case GameActionType.Heal:
                    return healAction;
                case GameActionType.LaunchProjectile:
                    return launchProjectileAction;
                default:
                    GameLogger.LogError("GameActionSO: GetAction: invalid actionType: " + actionType);
                    return null;
            }
        }
    }
}
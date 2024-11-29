using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_action_", menuName = "游戏玩法/配置/行为模板")]
    public class GameActionSO : GameSOBase
    {
        public GameActionType actionType;
        public GameActionModel_Dmg dmgAction;
        public GameActionModel_Heal healAction;
        public GameActionModel_LaunchProjectile launchProjectileAction;
        public GameProjectileSO launchProjectileSO;
        public GameSkillSO[] skillSORefs;

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
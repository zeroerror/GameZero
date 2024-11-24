using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_action_", menuName = "游戏玩法/配置/行为模板")]
    public class GameActionSO : GameSOBase
    {
        public GameActionType actionType;
        public GameAction_Dmg dmgAction;
        public GameAction_Heal healAction;
        public GameAction_LaunchBullet launchBulletAction;
        public GameBulletSO launchBulletSO;
        public GameSkillSO[] skillSORefs;
    }
}
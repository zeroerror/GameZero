namespace GamePlay.Bussiness.Logic
{
    public static class GameActionRCCollection
    {
        /** 行为执行 - 伤害行为 */
        public static readonly string RC_GAME_ACTION_DO_DMG = "RC_GAME_ACTION_DO_DMG";
        /** 行为执行 - 治疗行为 */
        public static readonly string RC_GAME_ACTION_DO_HEAL = "RC_GAME_ACTION_DO_HEAL";
        /** 行为执行 - 发射投射物 */
        public static readonly string RC_GAME_ACTION_LAUNCH_PROJECTILE = "RC_GAME_ACTION_LAUNCH_PROJECTILE";
        /** 行为执行 - 击退 */
        public static readonly string RC_GAME_ACTION_DO_KNOCK_BACK = "RC_GAME_ACTION_DO_KNOCK_BACK";
        /** 行为执行 - 属性修改 */
        public static readonly string RC_GAME_ACTION_DO_ATTRIBUTE_MODIFY = "RC_GAME_ACTION_DO_ATTRIBUTE_MODIFY";
    }

    public struct GameActionRCArgs_DoDmg
    {
        public int actionId;
        public GameActionRecord_Dmg dmgRecord;
        public GameActionRCArgs_DoDmg(int actionId, in GameActionRecord_Dmg dmgRecord)
        {
            this.actionId = actionId;
            this.dmgRecord = dmgRecord;
        }
    }

    public struct GameActionRCArgs_DoHeal
    {
        public int actionId;
        public GameActionRecord_Heal healRecord;
        public GameActionRCArgs_DoHeal(int actionId, in GameActionRecord_Heal healRecord)
        {
            this.actionId = actionId;
            this.healRecord = healRecord;
        }
    }

    public struct GameActionRCArgs_LaunchProjectile
    {
        public int actionId;
        public GameActionRecord_LaunchProjectile record;
        public GameActionRCArgs_LaunchProjectile(int actionId, in GameActionRecord_LaunchProjectile record)
        {
            this.actionId = actionId;
            this.record = record;
        }
    }

    public struct GameActionRCArgs_DoKnockBack
    {
        public int actionId;
        public GameActionRecord_KnockBack record;
        public GameActionRCArgs_DoKnockBack(int actionId, in GameActionRecord_KnockBack record)
        {
            this.actionId = actionId;
            this.record = record;
        }
    }

    public struct GameActionRCArgs_DoAttributeModify
    {
        public int actionId;
        public GameActionRecord_AttributeModify record;
        public GameActionRCArgs_DoAttributeModify(int actionId, in GameActionRecord_AttributeModify record)
        {
            this.actionId = actionId;
            this.record = record;
        }
    }

}

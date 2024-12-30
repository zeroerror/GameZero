using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public static class GameActionRCCollection
    {
        /** 行为执行 */
        public static readonly string RC_GAME_ACTION_DO = "RC_GAME_ACTION_DO";
        /** 行为执行 - 伤害行为 */
        public static readonly string RC_GAME_ACTION_DMG = "RC_GAME_ACTION_DO_DMG";
        /** 行为执行 - 治疗行为 */
        public static readonly string RC_GAME_ACTION_HEAL = "RC_GAME_ACTION_DO_HEAL";
        /** 行为执行 - 发射投射物 */
        public static readonly string RC_GAME_ACTION_LAUNCH_PROJECTILE = "RC_GAME_ACTION_LAUNCH_PROJECTILE";
        /** 行为执行 - 击退 */
        public static readonly string RC_GAME_ACTION_KNOCK_BACK = "RC_GAME_ACTION_DO_KNOCK_BACK";
        /** 行为执行 - 属性修改 */
        public static readonly string RC_GAME_ACTION_ATTRIBUTE_MODIFY = "RC_GAME_ACTION_DO_ATTRIBUTE_MODIFY";
        /** 行为执行 - 挂载Buff */
        public static readonly string RC_GAME_ACTION_ATTACH_BUFF = "RC_GAME_ACTION_ATTACH_BUFF";
        /** 行为执行 - 召唤角色 */
        public static readonly string RC_GAME_ACTION_SUMMON_ROLE = "RC_GAME_ACTION_SUMMON_ROLE";
    }

    public struct GameActionRCArgs_Do
    {
        public int actionId;
        public GameIdArgs actorIdArgs;
        public GameVec2 actPos;
        public GameActionRCArgs_Do(int actionId, in GameIdArgs actorIdArgs, in GameVec2 actPos)
        {
            this.actionId = actionId;
            this.actorIdArgs = actorIdArgs;
            this.actPos = actPos;
        }
    }

    public struct GameActionRCArgs_Dmg
    {
        public int actionId;
        public GameActionRecord_Dmg dmgRecord;
        public GameActionRCArgs_Dmg(int actionId, in GameActionRecord_Dmg dmgRecord)
        {
            this.actionId = actionId;
            this.dmgRecord = dmgRecord;
        }
    }

    public struct GameActionRCArgs_Heal
    {
        public int actionId;
        public GameActionRecord_Heal healRecord;
        public GameActionRCArgs_Heal(int actionId, in GameActionRecord_Heal healRecord)
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

    public struct GameActionRCArgs_KnockBack
    {
        public int actionId;
        public GameActionRecord_KnockBack record;
        public GameActionRCArgs_KnockBack(int actionId, in GameActionRecord_KnockBack record)
        {
            this.actionId = actionId;
            this.record = record;
        }
    }

    public struct GameActionRCArgs_AttributeModify
    {
        public int actionId;
        public GameActionRecord_AttributeModify record;
        public GameActionRCArgs_AttributeModify(int actionId, in GameActionRecord_AttributeModify record)
        {
            this.actionId = actionId;
            this.record = record;
        }
    }

    public struct GameActionRCArgs_AttachBuff
    {
        public int actionId;
        public GameActionRecord_AttachBuff record;
        public GameActionRCArgs_AttachBuff(int actionId, in GameActionRecord_AttachBuff record)
        {
            this.actionId = actionId;
            this.record = record;
        }
    }

    public struct GameActionRCArgs_SummonRoles
    {
        public int actionId;
        public GameActionRecord_SummonRoles record;
        public GameActionRCArgs_SummonRoles(int actionId, in GameActionRecord_SummonRoles record)
        {
            this.actionId = actionId;
            this.record = record;
        }
    }
}

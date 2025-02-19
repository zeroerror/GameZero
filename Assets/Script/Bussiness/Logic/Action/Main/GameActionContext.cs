using System.Collections.Generic;
using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.Logic
{
    public class GameActionContext
    {
        public GameActionTemplate template { get; private set; }
        public GameActionOptionTemplate optionTemplate { get; private set; }
        public GameActionOptionRepo optionRepo { get; private set; }
        public GameIdService idService { get; private set; }

        public List<GameActionRecord_Dmg> dmgRecordList { get; private set; }
        public List<GameActionRecord_Heal> healRecordList { get; private set; }
        public List<GameActionRecord_LaunchProjectile> launchProjectileRecordList { get; private set; }
        public List<GameActionRecord_KnockBack> knockBackRecordList { get; private set; }
        public List<GameActionRecord_AttributeModify> attributeModifyRecordList { get; private set; }
        public List<GameActionRecord_AttachBuff> attachBuffRecordList { get; private set; }
        public List<GameActionRecord_SummonRoles> summonRolesRecordList { get; private set; }
        public List<GameActionRecord_CharacterTransform> transformRecordList { get; private set; }
        public List<GameActionRecord_Stealth> stealthRecordList { get; private set; }

        public GameActionContext()
        {
            this.template = new GameActionTemplate();
            this.optionTemplate = new GameActionOptionTemplate();
            this.optionRepo = new GameActionOptionRepo();
            this.idService = new GameIdService();

            this.dmgRecordList = new List<GameActionRecord_Dmg>();
            this.healRecordList = new List<GameActionRecord_Heal>();
            this.launchProjectileRecordList = new List<GameActionRecord_LaunchProjectile>();
            this.knockBackRecordList = new List<GameActionRecord_KnockBack>();
            this.attributeModifyRecordList = new List<GameActionRecord_AttributeModify>();
            this.attachBuffRecordList = new List<GameActionRecord_AttachBuff>();
            this.summonRolesRecordList = new List<GameActionRecord_SummonRoles>();
            this.transformRecordList = new List<GameActionRecord_CharacterTransform>();
            this.stealthRecordList = new List<GameActionRecord_Stealth>();
        }

        public void ClearRecords()
        {
            this.dmgRecordList.Clear();
            this.healRecordList.Clear();
            this.launchProjectileRecordList.Clear();
            this.knockBackRecordList.Clear();
            this.attributeModifyRecordList.Clear();
            this.attachBuffRecordList.Clear();
            this.summonRolesRecordList.Clear();
            this.transformRecordList.Clear();
            this.stealthRecordList.Clear();
        }
    }
}
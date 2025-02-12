namespace GamePlay.Bussiness.Logic
{
    [System.Serializable]
    public enum GameActionType
    {
        None = 0,
        /// <summary> 伤害 </summary>
        Dmg = 1,
        /// <summary> 治疗 </summary>
        Heal = 2,
        /// <summary> 发射投射物 </summary>
        LaunchProjectile = 3,
        /// <summary> 击退 </summary>
        KnockBack = 4,
        /// <summary> 属性修改 </summary>
        AttributeModify = 5,
        /// <summary> 附加buff </summary>
        AttachBuff = 6,
        /// <summary> 移除buff </summary>
        DetachBuff = 7,
        /// <summary> 召唤角色 </summary>
        SummonRoles = 8,
        /// <summary> 角色变身 </summary>
        CharacterTransform = 9,
        /// <summary> 隐身 </summary>
        Stealth = 10,

        /// <summary> 击杀 </summary>
        Kill = 999,
    }
}
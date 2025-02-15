namespace GamePlay.Bussiness.Logic
{
    public enum GameAttributeType
    {
        None = 0,
        /// <summary> 血量 </summary>
        HP = 1,
        /// <summary> 最大血量 </summary>
        MaxHP = 2,
        /// <summary> 魔法值 </summary>
        MP = 3,
        /// <summary> 最大魔法值 </summary>
        MaxMP = 4,
        /// <summary> 攻击力 </summary>
        Attack = 5,
        /// <summary> 攻击速度 </summary>
        AttackSpeed = 6,
        /// <summary> 移动速度 </summary>
        MoveSpeed = 7,
        /// <summary> 暴击率 </summary>
        CritRate = 8,
        /// <summary> 暴击伤害加成 </summary>
        CritDmgAddition = 9,

        /// <summary> 普通伤害抗性 </summary>
        NormalDmgResist = 101,
        /// <summary> 击退抗性 击退抗性越高，被击退距离越短</summary>
        KnockbackResist = 103,

        /// <summary> 物理护盾 </summary>
        Shield = 201,

        /// <summary> 治疗加成 </summary>
        HealAddition = 301,
        /// <summary> 伤害加成 </summary>
        DmgAddition = 302,
    }
}
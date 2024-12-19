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
        /// <summary> 击退抗性 击退抗性越高，被击退距离越短</summary>
        KnockbackResist = 8,
    }
}
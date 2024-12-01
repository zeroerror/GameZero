namespace GamePlay.Bussiness.Logic
{
    [System.Serializable]
    public enum GameActionType
    {
        None = 0,
        // 伤害
        Dmg = 1,
        // 治疗
        Heal = 2,
        // 发射投射物
        LaunchProjectile = 3,
    }
}
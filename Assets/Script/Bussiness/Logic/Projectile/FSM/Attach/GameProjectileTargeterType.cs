namespace GamePlay.Bussiness.Logic
{
    public enum GameProjectileTargeterType
    {
        None,
        /// <summary> 发射投射物的角色行为者 </summary>
        RoleActor = 1,
        /// <summary> 目标 </summary>
        Target = 2,
        /// <summary> 坐标 </summary>
        Position = 3,
    }
}
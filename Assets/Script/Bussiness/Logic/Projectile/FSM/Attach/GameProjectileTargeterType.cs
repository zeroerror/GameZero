namespace GamePlay.Bussiness.Logic
{
    public enum GameProjectileTargeterType
    {
        None,
        /// <summary> 发射投射物的行为者, 可以是角色、投射物 </summary>
        Actor = 1,
        /// <summary> 目标 </summary>
        Target = 2,
        /// <summary> 坐标 </summary>
        Position = 3,
    }
}
public enum GameRoleStateType
{
    None,
    /// <summary> 待机 </summary>
    Idle = 1,
    /// <summary> 移动 </summary>
    Move = 2,
    /// <summary> 施法 </summary>
    Cast = 3,
    /// <summary> 死亡 </summary>
    Dead = 4,
    /// <summary> 隐身 </summary>
    Stealth = 5,
    /// <summary> 已销毁 </summary>
    Destroyed = 100,
}

// extension
public static class GameRoleStateExtension
{
    public static string ToStr(this GameRoleStateType state)
    {
        switch (state)
        {
            case GameRoleStateType.Idle:
                return "Idle";
            case GameRoleStateType.Move:
                return "Move";
            case GameRoleStateType.Cast:
                return "Cast";
            case GameRoleStateType.Dead:
                return "Dead";
            case GameRoleStateType.Stealth:
                return "Stealth";
            default:
                return "None";
        }
    }
}
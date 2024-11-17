public enum GameRoleStateType
{
    None,
    Idle,
    Move,
    Cast,
    Dead
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
            default:
                return "None";
        }
    }
}
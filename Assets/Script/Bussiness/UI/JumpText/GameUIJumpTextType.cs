namespace GamePlay.Bussiness.UI
{
    public enum GameUIJumpTextType
    {
        None = 0,
        Dmg = 1,
        Heal = 2,
    }

    public static class GameUIJumpTextTypeExt
    {
        public static string ToText(this GameUIJumpTextType type)
        {
            switch (type)
            {
                case GameUIJumpTextType.Dmg:
                    return "dmg";
                case GameUIJumpTextType.Heal:
                    return "heal";
                default:
                    return "none";
            }
        }
    }
}
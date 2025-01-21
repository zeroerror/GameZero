namespace GamePlay.Bussiness.Logic
{
    /** 数值比较类型 */
    public enum GameNumCompareType
    {
        /** 无  */
        None = 0,
        /** 等于 */
        Equal,
        /** 大于 */
        Greater,
        /** 大于等于 */
        GreaterEqual,
        /** 小于 */
        Less,
        /** 小于等于 */
        LessEqual,
        /** 不等于 */
        NotEqual,
    }

    public static class GameNumCompareTypeExt
    {
        public static bool Compare(this GameNumCompareType compareType, float valueA, float valueB)
        {
            switch (compareType)
            {
                case GameNumCompareType.None:
                    return false;
                case GameNumCompareType.Equal:
                    return valueA == valueB;
                case GameNumCompareType.Greater:
                    return valueA > valueB;
                case GameNumCompareType.GreaterEqual:
                    return valueA >= valueB;
                case GameNumCompareType.Less:
                    return valueA < valueB;
                case GameNumCompareType.LessEqual:
                    return valueA <= valueB;
                case GameNumCompareType.NotEqual:
                    return valueA != valueB;
                default:
                    return false;
            }
        }

        public static string ToDesc(this GameNumCompareType compareType)
        {
            switch (compareType)
            {
                case GameNumCompareType.None:
                    return "无";
                case GameNumCompareType.Equal:
                    return "等于";
                case GameNumCompareType.Greater:
                    return "大于";
                case GameNumCompareType.GreaterEqual:
                    return "大于等于";
                case GameNumCompareType.Less:
                    return "小于";
                case GameNumCompareType.LessEqual:
                    return "小于等于";
                case GameNumCompareType.NotEqual:
                    return "不等于";
                default:
                    return "未知";
            }
        }
    }
}

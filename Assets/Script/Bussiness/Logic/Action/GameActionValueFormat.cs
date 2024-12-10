using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    /// <summary> 行为数值的格式类型 </summary>
    public enum GameActionValueFormat
    {
        None,
        /// <summary> 固定值 </summary>
        Fixed = 1,
        /// <summary> 百分比 </summary>
        Percent = 2,
    }

    public static class GameActionValueFormatUtil
    {
        public static float FormatValue(this GameActionValueFormat format, float value)
        {
            switch (format)
            {
                case GameActionValueFormat.Fixed:
                    break;
                case GameActionValueFormat.Percent:
                    value = value / 100;
                    break;
                default:
                    GameLogger.LogError("未处理的治疗数值格式" + format);
                    break;
            }
            return value;
        }
    }
}
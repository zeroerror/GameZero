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
            var formatV = 0f;
            switch (format)
            {
                case GameActionValueFormat.Fixed:
                    formatV = value;
                    break;
                case GameActionValueFormat.Percent:
                    formatV = value / 100f;
                    break;
                default:
                    GameLogger.LogError("未处理的治疗数值格式" + format);
                    break;
            }
            return formatV;
        }
    }
}
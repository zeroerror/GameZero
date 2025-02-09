namespace GamePlay.Core
{
    public static class GameNumberUtil
    {
        /// <summary>
        /// 获取格式化数字字符串, 超过1w显示k, 超过1kw显示w, 超过1kw显示kw
        /// <para>digits: 小数位数</para>
        /// </summary>
        public static string GetFormatNumStr(this float value, int digits = 0)
        {
            if (value >= 100000000) return $"{value / 100000000f:F1}kw";
            if (value >= 10000) return $"{value / 10000f:F1}w";
            if (value >= 1000) return $"{value / 1000f:F1}k";
            return GameMathF.GetFixed(value, digits).ToString();
        }
    }
}
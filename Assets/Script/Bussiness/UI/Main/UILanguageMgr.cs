namespace GamePlay.Bussiness.UI
{
    public static class UILanguageMgr
    {
        /// <summary>
        /// 在开发环境下获取文本的方式
        /// </summary>
        /// <para>content: 文本内容</para>
        /// <returns></returns>
        public static string GetDevStr(string content)
        {
            return content;
        }

        public static string ToDevStr(this string content)
        {
            return content;
        }

        public static string ToDevStr(this int content)
        {
            return content.ToString();
        }

        public static string ToDevStr(this float content)
        {
            return content.ToString();
        }

        // /// <summary>
        // /// 在发布后获取文本的方式, 用于多语言
        // /// <para>key: 文本key</para>
        // /// </summary>
        // public static string GetReleaseStr(string key)
        // {
        //     // TODO
        //     return key;
        // }

        // public static string ToReleaseStr(this string key)
        // {
        //     return GetReleaseStr(key);
        // }

        // public static string ToReleaseStr(this int key)
        // {
        //     return GetReleaseStr(key.ToString());
        // }

        // public static string ToReleaseStr(this float key)
        // {
        //     return GetReleaseStr(key.ToString());
        // }

    }
}
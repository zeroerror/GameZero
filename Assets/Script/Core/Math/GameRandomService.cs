using System.Collections.Generic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Core
{
    /// <summary>
    /// 随机数服务, 用于游戏中的随机数生成
    /// 可用于帧同步框架中的随机数生成, 因为服务所提供的随机种子是固定的
    /// </summary>
    public static class GameRandomService
    {
        /// <summary> 随机种子库 </summary>
        private static int[] _seeds;
        /// <summary> 当前种子索引 </summary>
        private static int _seedIndex = 0;

        /// <summary> 默认初始化随机种子 </summary>
        public static void DefaultInitSeeds()
        {
            // 读取随机种子tx
            const string url = "random_seeds";
            TextAsset textAsset = GameResourceManager.Load<TextAsset>(url);
            if (textAsset == null)
            {
                Debug.LogError("随机种子文件不存在 " + url);
                return;
            }

            string[] lines = textAsset.text.Split(',');
            _seeds = new int[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                int.TryParse(line, out _seeds[i]);
            }
        }

        private static int _GetSeed()
        {
            _seedIndex++;
            _seedIndex = _seedIndex % _seeds.Length;
            return _seeds[_seedIndex];
        }

        /// <summary>
        /// 获取一个随机的整数
        /// <para> min: 最小值 </para>
        /// <para> max: 最大值 </para>
        /// </summary>
        public static int GetRandom(int min, int max)
        {
            if (min == max)
            {
                return min;
            }
            int seed = _GetSeed();
            int random = min + seed % (max - min);
            return random;
        }

        /// <summary>
        /// 获取一个随机的浮点数
        /// <para> min: 最小值 </para>
        /// <para> max: 最大值 </para>
        /// </summary>
        public static float GetRandom(float min, float max)
        {
            if (min == max)
            {
                return min;
            }
            int seedi = _GetSeed();
            float decimalPart = seedi % 1000 / 1000f;
            float random = min + decimalPart * (max - min);
            return random;
        }

        /// <summary>
        /// 获取一个随机的浮点数
        /// <para> range: 最小值和最大值 </para>
        /// </summary>
        public static float GetRandom(Vector2 range)
        {
            return GetRandom(range.x, range.y);
        }

        public static T GetRandom<T>(T[] array)
        {
            int index = GetRandom(0, array.Length);
            return array[index];
        }

        public static T GetRandom<T>(List<T> list)
        {
            int index = GetRandom(0, list.Count);
            return list[index];
        }

        public static Vector2 GetRandomDir2D()
        {
            float angle = GetRandom(0f, 360f);
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
    }
}
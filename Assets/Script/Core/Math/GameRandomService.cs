using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Bussiness.Core
{
    /// <summary>
    /// 随机数服务, 用于游戏中的随机数生成
    /// 可用于帧同步框架中的随机数生成, 因为服务所提供的随机种子是固定的
    /// </summary>
    public class GameRandomService
    {
        /// <summary> 随机种子库 </summary>
        private int[] _seeds;
        /// <summary> 当前种子索引 </summary>
        private int _seedIndex = 0;

        public GameRandomService()
        {
            this._InitSeeds();
        }

        private void _InitSeeds()
        {
            // 读取随机种子tx
            const string url = "random_seeds";
            TextAsset textAsset = Resources.Load<TextAsset>(url);
            if (textAsset == null)
            {
                Debug.LogError("随机种子文件不存在 " + url);
                return;
            }

            string[] lines = textAsset.text.Split(',');
            this._seeds = new int[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                int.TryParse(line, out this._seeds[i]);
            }
        }

        private int _GetSeed()
        {
            this._seedIndex++;
            this._seedIndex = this._seedIndex % this._seeds.Length;
            return this._seeds[this._seedIndex];
        }

        /// <summary>
        /// 获取一个随机的整数
        /// <para> min: 最小值 </para>
        /// <para> max: 最大值 </para>
        /// </summary>
        public int GetRandom(int min, int max)
        {
            if (min == max)
            {
                return min;
            }
            int seed = this._GetSeed();
            int random = min + seed % (max - min);
            return random;
        }

        /// <summary>
        /// 获取一个随机的浮点数
        /// <para> min: 最小值 </para>
        /// <para> max: 最大值 </para>
        /// </summary>
        public float GetRandom(float min, float max)
        {
            if (min == max)
            {
                return min;
            }
            int seedi = this._GetSeed();
            float decimalPart = seedi % 1000 / 1000f;
            float random = min + decimalPart * (max - min);
            return random;
        }

        /// <summary>
        /// 获取一个随机的浮点数
        /// <para> range: 最小值和最大值 </para>
        /// </summary>
        public float GetRandom(Vector2 range)
        {
            return this.GetRandom(range.x, range.y);
        }

        public T GetRandom<T>(T[] array)
        {
            int index = this.GetRandom(0, array.Length);
            return array[index];
        }

        public T GetRandom<T>(List<T> list)
        {
            int index = this.GetRandom(0, list.Count);
            return list[index];
        }

        public Vector2 GetRandomDir2D()
        {
            float angle = this.GetRandom(0f, 360f);
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
    }
}
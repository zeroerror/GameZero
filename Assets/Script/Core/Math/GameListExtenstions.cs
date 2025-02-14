using System;
using System.Collections.Generic;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Core
{
    public static class GameListExtenstions
    {
        public static float Min(this List<float> list)
        {
            float min = list[0];
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i] < min)
                {
                    min = list[i];
                }
            }
            return min;
        }

        public static float Min(this Span<float> list)
        {
            float min = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                if (list[i] < min)
                {
                    min = list[i];
                }
            }
            return min;
        }


        public static GameVec2 Min(this Span<GameVec2> list, out int index)
        {
            GameVec2 min = list[0];
            index = 0;
            for (int i = 1; i < list.Length; i++)
            {
                if (list[i].sqrMagnitude < min.sqrMagnitude)
                {
                    min = list[i];
                    index = i;
                }
            }
            return min;
        }

        public static float Max(this List<float> list)
        {
            float max = list[0];
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i] > max)
                {
                    max = list[i];
                }
            }
            return max;
        }

        public static float Max(this Span<float> list)
        {
            float max = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                if (list[i] > max)
                {
                    max = list[i];
                }
            }
            return max;
        }

        public static List<T> Filter<T>(this List<T> list, Predicate<T> match)
        {
            List<T> result = new List<T>();
            for (int i = 0; i < list.Count; i++)
            {
                if (match(list[i]))
                {
                    result.Add(list[i]);
                }
            }
            return result;
        }

        public static T[] Filter<T>(this T[] list, Predicate<T> match)
        {
            List<T> result = new List<T>();
            for (int i = 0; i < list.Length; i++)
            {
                if (match(list[i]))
                {
                    result.Add(list[i]);
                }
            }
            return result.ToArray();
        }

        public static T Find<T>(this T[] list, Predicate<T> match)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (match(list[i])) return list[i];
            }
            return default;
        }

        public static int FindIndex<T>(this T[] list, Predicate<T> match)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (match(list[i])) return i;
            }
            return -1;
        }

        public static bool Contains<T>(this T[] list, Predicate<T> match)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (match(list[i])) return true;
            }
            return false;
        }

        public static void Foreach<T>(this T[] list, Action<T, int> action)
        {
            for (int i = 0; i < list.Length; i++) action(list[i], i);
        }

        public static void Foreach<T>(this T[] list, Action<T> action)
        {
            list.Foreach((item, index) => action(item));
        }

        public static void Foreach<T>(this List<T> list, Action<T, int> action)
        {
            for (int i = 0; i < list.Count; i++) action(list[i], i);
        }

        public static void Foreach<T>(this List<T> list, Action<T> action)
        {
            list.ForEach(action);
        }

        public static string ToString<T>(this T[] list)
        {
            string result = "";
            for (int i = 0; i < list.Length; i++)
            {
                result += list[i].ToString();
                if (i < list.Length - 1) result += ", ";
            }
            return result;
        }

        public static string ToString<T>(this List<T> list)
        {
            string result = "";
            for (int i = 0; i < list.Count; i++)
            {
                result += list[i].ToString();
                if (i < list.Count - 1) result += ", ";
            }
            return result;
        }

        public static List<T> ToList<T>(this List<T> list)
        {
            if (list == null) return null;
            List<T> result = new List<T>(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                result.Add(list[i]);
            }
            return result;
        }

        public static void Sort<T>(this List<T> list, Comparison<T> comparison)
        {
            list.Sort(comparison);
        }

        public static T[] Sort<T>(this T[] list, Comparison<T> comparison)
        {
            Array.Sort(list, comparison);
            return list;
        }

        public static U[] Map<T, U>(this T[] list, Func<T, U> func)
        {
            U[] result = new U[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                result[i] = func(list[i]);
            }
            return result;
        }

        public static List<U> Map<T, U>(this List<T> list, Func<T, U> func)
        {
            List<U> result = new List<U>(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                result.Add(func(list[i]));
            }
            return result;
        }

        public static bool HasData<T>(this T[] list)
        {
            return list != null && list.Length > 0;
        }

        public static int IndexOf<T>(this System.Array array, T value)
        {
            return System.Array.IndexOf(array, value);
        }

        /// <summary> 
        /// 移除数组中的元素,旧的数组不会被改变
        /// <para>match: 匹配条件</para>
        /// <returns>返回新数组</returns>
        /// </summary>
        public static T[] Remove<T>(this T[] arr, Predicate<T> match)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (match(arr[i]))
                {
                    arr[i] = arr[arr.Length - 1];
                    Array.Resize(ref arr, arr.Length - 1);
                    break;
                }
            }
            return arr;
        }

        public static T[] Contact<T>(this T[] arr, T value)
        {
            Array.Resize(ref arr, arr.Length + 1);
            arr[arr.Length - 1] = value;
            return arr;
        }
        public static T[] Contact<T>(this T[] arr, T[] value)
        {
            Array.Resize(ref arr, arr.Length + value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                arr[arr.Length - value.Length + i] = value[i];
            }
            return arr;
        }
    }
}

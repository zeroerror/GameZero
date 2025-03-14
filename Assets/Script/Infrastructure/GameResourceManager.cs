using System.Collections.Generic;
using UnityEngine;
namespace GamePlay.Infrastructure
{
    public static class GameResourceManager
    {

        /// <summary> 资源缓存 </summary>
        private static readonly Dictionary<string, Object> _resCache = new Dictionary<string, Object>();
        private static bool _TryGetCache<T>(string url, out T obj) where T : Object
        {
            var key = url + typeof(T).Name;
            if (_resCache.TryGetValue(key, out var cacheObj))
            {
                obj = cacheObj as T;
                return true;
            }
            obj = null;
            return false;
        }
        private static void AddToCache<T>(Object obj, string url)
        {
            var key = url + typeof(T).Name;
            _resCache[key] = obj;
        }

        /// <summary> 资源列表缓存 key为目录路径 </summary>
        private static readonly Dictionary<string, Dictionary<System.Type, Object[]>> _resListCache = new Dictionary<string, Dictionary<System.Type, Object[]>>();

        public static void ClearCache()
        {
            _resCache.Clear();
            _resListCache.Clear();
        }

        private static U[] Map<T, U>(this T[] list, System.Func<T, U> func)
        {
            U[] result = new U[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                result[i] = func(list[i]);
            }
            return result;
        }
        private static Object Clone(this Object obj)
        {
            var clone = Object.Instantiate(obj);
            clone.name = obj.name;
            return clone;
        }

        /// <summary>
        /// 加载特定资源
        /// <para> url: 资源路径 </para>
        /// </summary>
        public static T Load<T>(string url) where T : Object
        {
            if (url == null) return null;
            if (_TryGetCache<T>(url, out var cacheObj))
            {
                return cacheObj as T;
            }
            var originObj = Resources.Load<T>(url);
            if (originObj == null)
            {
                GameLogger.LogError($"资源加载失败: {url}");
                return null;
            }
            var objInst = typeof(T) == typeof(GameObject) ? originObj : originObj.Clone() as T;
            AddToCache<T>(objInst, url);
            return objInst;
        }

        public static bool IsExist<T>(string url, bool needUnload = true) where T : Object
        {
            if (url == null) return false;
            if (_TryGetCache<T>(url, out var cacheObj))
            {
                return cacheObj is T;
            }
            var originObj = Resources.Load<T>(url);
            if (originObj == null)
            {
                return false;
            }

            if (needUnload)
            {
                if (originObj is GameObject) Object.DestroyImmediate(originObj);
                Resources.UnloadAsset(originObj);
            }
            else
            {
                AddToCache<T>(originObj, url);
            }
            return true;
        }

        /// <summary>
        /// 异步加载特定资源
        /// <para> url: 资源路径 </para>
        /// <para> cb: 加载完成回调 </para>
        /// </summary>
        public static void LoadAsync<T>(string url, System.Action<T> cb) where T : Object
        {
            if (url == null) return;
            if (_TryGetCache<T>(url, out var cacheObj))
            {
                cb?.Invoke(cacheObj as T);
                return;
            }
            var rr = Resources.LoadAsync<T>(url);
            rr.completed += (op) =>
             {
                 if (!op.isDone)
                 {
                     return;
                 }
                 var objInst = rr.asset.Clone() as T;
                 Object.DestroyImmediate(objInst);
                 AddToCache<T>(objInst, url);
                 cb?.Invoke(objInst);
             };
        }

        /// <summary>
        /// 加载所有资源
        /// <para> dirUrl: 目录 </para>
        /// </summary>
        public static T[] LoadAll<T>(string dirUrl) where T : Object
        {
            if (dirUrl == null) return null;
            var type = typeof(T);
            if (_resListCache.TryGetValue(dirUrl, out var cacheDict))
            {
                if (cacheDict.TryGetValue(type, out var cacheResList))
                {
                    return cacheResList as T[];
                }
                cacheResList = Resources.LoadAll<T>(dirUrl);
                cacheResList = cacheResList.Map((obj) => obj.Clone());
                cacheDict[type] = cacheResList;
                return cacheResList as T[];
            }
            cacheDict = new Dictionary<System.Type, Object[]>();
            _resListCache[dirUrl] = cacheDict;

            var resList = Resources.LoadAll<T>(dirUrl);
            resList = resList.Map((obj) => obj.Clone() as T);
            cacheDict[type] = resList;
            return resList;
        }

        /// <summary>
        /// 加载所有资源
        /// <para> dirUrl: 目录 </para>
        /// </summary>
        public static Object[] LoadAll(string dirUrl, System.Type type)
        {
            if (dirUrl == null) return null;
            if (_resListCache.TryGetValue(dirUrl, out var cacheDict))
            {
                if (cacheDict.TryGetValue(type, out var cacheResList))
                {
                    return cacheResList;
                }
                cacheResList = Resources.LoadAll(dirUrl, type);
                cacheResList = cacheResList.Map((obj) => obj.Clone());
                cacheDict[type] = cacheResList;
                return cacheResList;
            }
            cacheDict = new Dictionary<System.Type, Object[]>();
            _resListCache[dirUrl] = cacheDict;

            var resList = Resources.LoadAll(dirUrl, type);
            resList = resList.Map((obj) => obj.Clone());
            cacheDict[type] = resList;
            return resList;
        }

        public static AnimationClip LoadAnimationClip(string url)
        {
            var clip = Load<AnimationClip>(url);
            clip.events = null;
            return clip;
        }
    }

}
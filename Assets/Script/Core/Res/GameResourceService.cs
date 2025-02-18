using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Core
{
    public static class GameResourceService
    {
        /// <summary> 资源缓存 </summary>
        private static readonly Dictionary<string, Object> _resCache = new Dictionary<string, Object>();
        /// <summary> 资源列表缓存 key为目录路径 </summary>
        private static readonly Dictionary<string, Dictionary<System.Type, Object[]>> _resListCache = new Dictionary<string, Dictionary<System.Type, Object[]>>();

        public static void ClearCache()
        {
            _resCache.Clear();
            _resListCache.Clear();
        }

        /// <summary>
        /// 加载特定资源
        /// <para> url: 资源路径 </para>
        /// </summary>
        public static T Load<T>(string url) where T : Object
        {
            if (_resCache.TryGetValue(url, out var cacheObj))
            {
                return cacheObj as T;
            }
            var obj = Resources.Load<T>(url);
            if (obj == null)
            {
                Debug.LogError($"资源加载失败: {url}");
                return null;
            }
            _resCache[url] = obj;
            return obj;
        }

        /// <summary>
        /// 异步加载特定资源
        /// <para> url: 资源路径 </para>
        /// <para> cb: 加载完成回调 </para>
        /// </summary>
        public static void LoadAsync<T>(string url, System.Action cb) where T : Object
        {
            if (_resCache.TryGetValue(url, out var cacheObj))
            {
                cb?.Invoke();
                return;
            }
            Resources.LoadAsync<T>(url).completed += (op) =>
            {
                if (!op.isDone)
                {

                    return;
                }
                cb?.Invoke();
            };
        }

        /// <summary>
        /// 加载所有资源
        /// <para> dirUrl: 目录 </para>
        /// </summary>
        public static T[] LoadAll<T>(string dirUrl) where T : Object
        {
            var type = typeof(T);
            if (_resListCache.TryGetValue(dirUrl, out var cacheDict))
            {
                if (cacheDict.TryGetValue(type, out var cacheResList))
                {
                    return cacheResList as T[];
                }
                cacheResList = Resources.LoadAll<T>(dirUrl);
                cacheDict[type] = cacheResList;
                return cacheResList as T[];
            }
            cacheDict = new Dictionary<System.Type, Object[]>();
            _resListCache[dirUrl] = cacheDict;

            var resList = Resources.LoadAll<T>(dirUrl);
            cacheDict[type] = resList;
            return resList;
        }

        /// <summary>
        /// 加载所有资源
        /// <para> dirUrl: 目录 </para>
        /// </summary>
        public static Object[] LoadAll(string dirUrl, System.Type type)
        {
            if (_resListCache.TryGetValue(dirUrl, out var cacheDict))
            {
                if (cacheDict.TryGetValue(type, out var cacheResList))
                {
                    return cacheResList;
                }
                cacheResList = Resources.LoadAll(dirUrl, type);
                cacheDict[type] = cacheResList;
                return cacheResList;
            }
            cacheDict = new Dictionary<System.Type, Object[]>();
            _resListCache[dirUrl] = cacheDict;

            var resList = Resources.LoadAll(dirUrl, type);
            cacheDict[type] = resList;
            return resList;
        }
    }
}
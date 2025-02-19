using System.Collections.Generic;
namespace GamePlay.Core
{
    public struct GameEventSubmit
    {
        public string name;
        public object args;
    }

    public class GameEventService
    {
        Dictionary<string, List<System.Action<object>>> _listeners = new();
        List<GameEventSubmit> _submitList = new();

        public void Submit(string name, object args)
        {
            Submit(new GameEventSubmit
            {
                name = name,
                args = args
            });
        }
        public void Submit(in GameEventSubmit submit)
        {
            _submitList.Add(submit);
        }

        public void Bind(string name, System.Action<object> callback)
        {
            if (!_listeners.ContainsKey(name))
                _listeners[name] = new List<System.Action<object>>();
            _listeners[name].Add(callback);
        }

        public void Unbind(string name, System.Action<object> callback)
        {
            if (!_listeners.ContainsKey(name)) return;
            _listeners[name].Remove(callback);
        }

        /// <summary>
        /// 处理所有的提交事件分发给监听者, 并清空本次处理的提交事件
        /// 若触发事件又提交了新的事件, 则会在下一帧处理, 避免死循环
        /// </summary>
        public void Tick()
        {
            var count = _submitList.Count;
            for (var i = 0; i < count; i++)
            {
                var submit = _submitList[i];
                if (!_listeners.ContainsKey(submit.name)) continue;
                var listeners = _listeners[submit.name];
                var listenerCount = listeners.Count;
                for (int j = 0; j < listenerCount; j++)
                {
                    var listener = listeners[j];
                    listener(submit.args);
                }
            }
            _submitList.RemoveRange(0, count);
        }
    }

}
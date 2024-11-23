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
        Dictionary<string, Queue<GameEventSubmit>> _submitQueues = new();

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
            if (!_submitQueues.ContainsKey(submit.name)) _submitQueues[submit.name] = new Queue<GameEventSubmit>();
            _submitQueues[submit.name].Enqueue(submit);
        }

        public void Regist(string name, System.Action<object> callback)
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

        public void Tick()
        {
            foreach (var kv in _submitQueues)
            {
                var name = kv.Key;
                var queue = kv.Value;
                while (queue.Count > 0)
                {
                    var submit = queue.Dequeue();
                    if (!_listeners.ContainsKey(name)) continue;
                    foreach (var listener in _listeners[name])
                    {
                        listener(submit.args);
                    }
                }
            }
            _submitQueues.Clear();
        }
    }

}
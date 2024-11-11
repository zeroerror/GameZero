using System.Collections.Generic;
namespace GamePlay.Core
{
    public struct GameEventSubmit
    {
        public string name;
        public object data;
    }
    public struct GameEventListener
    {
        public string name;
        public System.Action<object> callback;
    }

    public class GameEventService
    {
        Dictionary<string, List<System.Action<object>>> _listeners = new();
        Dictionary<string, Queue<GameEventSubmit>> _submitQueue = new();

        public void Submit(GameEventSubmit submit)
        {
            if (!_submitQueue.ContainsKey(submit.name))
                _submitQueue[submit.name] = new Queue<GameEventSubmit>();
            _submitQueue[submit.name].Enqueue(submit);
        }

        public void Register(GameEventListener listener)
        {
            if (!_listeners.ContainsKey(listener.name))
                _listeners[listener.name] = new List<System.Action<object>>();
            _listeners[listener.name].Add(listener.callback);
        }

        public void UnRegister(GameEventListener listener)
        {
            if (!_listeners.ContainsKey(listener.name)) return;
            _listeners[listener.name].Remove(listener.callback);
        }

        public void Tick()
        {
            foreach (var kv in _submitQueue)
            {
                var name = kv.Key;
                var submits = kv.Value;
                if (!_listeners.ContainsKey(name)) continue;
                var listeners = _listeners[name];
                while (submits.Count > 0)
                {
                    var submit = submits.Dequeue();
                    foreach (var listener in listeners)
                    {
                        listener(submit.data);
                    }
                }
            }
        }
    }

}
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
        Dictionary<string, List<GameEventSubmit>> _submitLists = new();

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
            if (!_submitLists.ContainsKey(submit.name)) _submitLists[submit.name] = new List<GameEventSubmit>();
            _submitLists[submit.name].Add(submit);
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

        public void Tick()
        {
            foreach (var kv in _submitLists)
            {
                var name = kv.Key;
                var list = kv.Value;
                var count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    var submit = list[i];
                    if (!_listeners.ContainsKey(name)) continue;
                    foreach (var listener in _listeners[name])
                    {
                        listener(submit.args);
                    }
                }
                list.RemoveRange(0, count);
            }
        }
    }

}
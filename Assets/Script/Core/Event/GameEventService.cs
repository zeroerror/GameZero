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

        public void Tick()
        {
            var count = _submitList.Count;
            for (var i = 0; i < count; i++)
            {
                var submit = _submitList[i];
                if (!_listeners.ContainsKey(submit.name)) continue;
                foreach (var listener in _listeners[submit.name])
                {
                    listener(submit.args);
                }
            }
            _submitList.RemoveRange(0, count);
        }
    }

}
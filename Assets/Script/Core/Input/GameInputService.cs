using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Bussiness.Core
{
    public class GameInputService
    {
        public Dictionary<KeyCode, List<Action>> keyActions = new Dictionary<KeyCode, List<Action>>();

        public void BindKeyAction(KeyCode keyCode, Action action)
        {
            if (!keyActions.ContainsKey(keyCode))
            {
                keyActions.Add(keyCode, new List<Action>());
            }
            keyActions[keyCode].Add(action);
        }

        public void UnbindKeyAction(KeyCode keyCode, Action action)
        {
            if (keyActions.ContainsKey(keyCode))
            {
                keyActions[keyCode].Remove(action);
            }
        }

        public void Tick()
        {
            foreach (var keyAction in keyActions)
            {
                if (Input.GetKeyDown(keyAction.Key))
                {
                    foreach (var action in keyAction.Value)
                    {
                        action();
                    }
                }
            }
        }
    }
}
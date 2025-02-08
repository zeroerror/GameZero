using System;
using System.Collections.Generic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Core
{
    public class GameInputService
    {
        public Dictionary<KeyCode, List<Action>> keyDownActions = new Dictionary<KeyCode, List<Action>>();
        public Dictionary<KeyCode, List<Action>> keyUpActions = new Dictionary<KeyCode, List<Action>>();
        public Dictionary<KeyCode, List<Action>> keyHoldActions = new Dictionary<KeyCode, List<Action>>();

        public GameInputService()
        {
        }

        public void BindKeyAction(KeyCode keyCode, Action action, GameInputStateType stateType)
        {
            Dictionary<KeyCode, List<Action>> dic;
            switch (stateType)
            {
                case GameInputStateType.KeyDown:
                    dic = keyDownActions;
                    break;
                case GameInputStateType.KeyUp:
                    dic = keyUpActions;
                    break;
                case GameInputStateType.KeyHold:
                    dic = keyHoldActions;
                    break;
                default:
                    GameLogger.LogError("GameInputService.BindKeyAction: Invalid stateType");
                    return;
            }
            if (!dic.ContainsKey(keyCode))
            {
                dic.Add(keyCode, new List<Action>());
            }
            dic[keyCode].Add(action);
        }

        public void UnbindKeyAction(KeyCode keyCode, Action action, GameInputStateType stateType)
        {
            Dictionary<KeyCode, List<Action>> dic;
            switch (stateType)
            {
                case GameInputStateType.KeyDown:
                    dic = keyDownActions;
                    break;
                case GameInputStateType.KeyUp:
                    dic = keyUpActions;
                    break;
                case GameInputStateType.KeyHold:
                    dic = keyHoldActions;
                    break;
                default:
                    GameLogger.LogError("GameInputService.UnbindKeyAction: Invalid stateType");
                    return;
            }
            if (dic.ContainsKey(keyCode))
            {
                dic[keyCode].Remove(action);
            }
        }

        public void Tick()
        {
            foreach (var keyAction in keyDownActions)
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
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Infrastructure
{
    public enum GameInputStateType
    {
        KeyDown,
        KeyUp,
        KeyHold
    }

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
                    // 使用临时列表，避免在遍历过程无法修改列表
                    this._tempList.Clear();
                    foreach (var action in keyAction.Value)
                    {
                        this._tempList.Add(action);
                    }
                    foreach (var action in this._tempList)
                    {
                        action();
                    }
                }
            }
        }
        private List<Action> _tempList = new List<Action>();
    }
}
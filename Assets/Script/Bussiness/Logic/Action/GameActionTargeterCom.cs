using System;
using System.Collections.Generic;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Logic
{
    public class GameActionTargeterCom : IGameActionTargeter
    {
        private List<IGameActionTargeter> _targeterList;
        public GameForeachType foreachType = GameForeachType.None;
        public GameRoleEntity targetRole { get => _getCurTargeter()?.targetRole; set { } }
        public GameVec2 targetPos { get => _getCurTargeter()?.targetPos ?? GameVec2.zero; set { } }
        public GameVec2 targetDirection { get => _targetDirectionList?[_curTargeterIndex] ?? GameVec2.zero; set { } }
        private List<GameVec2> _targetDirectionList;
        public int ExecuteCount => foreachType == GameForeachType.None ? _targeterList?.Count ?? 0 : 1;

        private int _curTargeterIndex = 0;

        public GameActionTargeterCom()
        {
            Clear();
        }

        public void Clear()
        {
            _targeterList = null;
            _targetDirectionList = null;
            _curTargeterIndex = 0;
        }

        public void UpdateTargeter()
        {
            if (_targeterList?.Count == 0) return;
            _targeterList.RemoveAll(targeter =>
            {
                var target = targeter.targetRole;
                return target == null;// todo 过滤死亡
            });

            if (foreachType == GameForeachType.None) return;
            switch (foreachType)
            {
                case GameForeachType.Sequential:
                    _curTargeterIndex = (_curTargeterIndex + 1) % _targeterList.Count;
                    break;
                case GameForeachType.Reverse:
                    _curTargeterIndex = (_curTargeterIndex - 1 + _targeterList.Count) % _targeterList.Count;
                    break;
                case GameForeachType.Random:
                    _curTargeterIndex = new Random().Next(0, _targeterList.Count);
                    break;
            }
        }

        public void SetTargeterList(GameVec2 actionPos, List<IGameActionTargeter> targeterList)
        {
            Clear();
            _targeterList = targeterList;
            var dirList = new List<GameVec2>();
            foreach (var targeter in targeterList)
            {
                var targetPos = targeter.targetRole?.transformCom.position ?? targeter.targetPos;
                var targetDir = (targetPos - actionPos).normalized;
                dirList.Add(targetDir);
            }
            _targetDirectionList = dirList;
        }

        private IGameActionTargeter _getCurTargeter()
        {
            return _targeterList?[_curTargeterIndex];
        }
    }
}

using System;
using System.Collections.Generic;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Logic
{
    public class GameActionTargeterCom
    {
        public GameForeachType foreachType = GameForeachType.None;
        public int ExecuteCount => foreachType == GameForeachType.None ? _targeterList?.Count ?? 0 : 1;

        /// <summary>
        /// 当前的目标实体。 UpdateTargeter逻辑会更新当前的目标实体
        /// </summary>
        public GameEntityBase targetEntity => _getCurTargeter().targetEntity;
        /// <summary>
        /// 当前的目标位置。 UpdateTargeter逻辑会更新当前的目标位置
        /// </summary>
        public GameVec2 targetPos => _getCurTargeter().targetPos;
        /// <summary>
        /// 当前的目标方向。 UpdateTargeter逻辑会更新当前的目标方向
        /// </summary>
        public GameVec2 targetDirection => _getCurTargeter().targetDirection;

        private List<GameActionTargeterArgs> _targeterList;
        private int _curTargeterIndex = 0;

        public GameActionTargeterCom()
        {
            Clear();
        }

        public void Clear()
        {
            _targeterList = null;
            _curTargeterIndex = 0;
        }

        public void UpdateTargeter()
        {
            if (_targeterList?.Count == 0) return;
            _targeterList.RemoveAll(targeter =>
            {
                var target = targeter.targetEntity;
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

        public void SetTargeterList(List<GameActionTargeterArgs> targeterList)
        {
            Clear();
            _targeterList = targeterList;
        }

        private GameActionTargeterArgs _getCurTargeter()
        {
            if (_targeterList == null || _targeterList.Count == 0) return default;
            return _targeterList[_curTargeterIndex];
        }
    }
}

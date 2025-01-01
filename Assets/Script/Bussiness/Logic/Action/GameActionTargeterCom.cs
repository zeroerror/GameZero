using System;
using System.Collections.Generic;
using GamePlay.Core;
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
        public GameEntityBase targetEntity => getCurTargeter().targetEntity;
        /// <summary>
        /// 当前的目标位置。 UpdateTargeter逻辑会更新当前的目标位置
        /// </summary>
        public GameVec2 targetPos => getCurTargeter().targetPosition;
        /// <summary>
        /// 当前的目标方向。 UpdateTargeter逻辑会更新当前的目标方向
        /// </summary>
        public GameVec2 targetDirection => getCurTargeter().targetDirection;

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
            // 剔除死亡的目标
            targeterList.RemoveAll(targeter => !!targeter.targetEntity && !targeter.targetEntity.IsAlive());
            if (targeterList.Count == 0) return;

            Clear();
            _targeterList = targeterList.ToList();
        }

        public void SetTargeter(in GameActionTargeterArgs targeter)
        {
            // 剔除死亡的目标
            var targetEntity = targeter.targetEntity;
            if (!!targetEntity && !targetEntity.IsAlive()) return;

            Clear();
            _targeterList = new List<GameActionTargeterArgs> { targeter };
        }

        public GameActionTargeterArgs getCurTargeter()
        {
            if (_targeterList == null || _targeterList.Count == 0) return default;
            return _targeterList[_curTargeterIndex];
        }

        public GameActionTargeterArgsRecord getCurTargeterAsRecord()
        {
            var record = new GameActionTargeterArgsRecord(
                getCurTargeter().targetEntity?.idCom.ToArgs() ?? default,
                getCurTargeter().targetPosition,
                getCurTargeter().targetDirection
            );
            return record;
        }
    }
}

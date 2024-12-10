using System.Collections.Generic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GamePhysicsCom
    {
        public GameColliderBase collider { get; private set; }
        public void SetCollider(GameColliderBase collider) => this.collider = collider;

        /// <summary>
        /// 发生碰撞过的实体Id列表
        /// </summary>
        private List<GameIdArgs> _collidedList;

        public GamePhysicsCom()
        {
            _collidedList = new List<GameIdArgs>();
        }

        /// <summary>
        /// 清空碰撞组件, 会清除绑定的碰撞器, 清空碰撞过的实体Id列表
        /// </summary>
        public void Clear()
        {
            ClearCollided();
        }

        /// <summary>
        /// 添加碰撞过的实体Id。用于部分需要避免重复碰撞的情况
        /// </summary>
        public void AddCollided(in GameIdArgs idArgs)
        {
            if (!_collidedList.Contains(idArgs))
            {
                _collidedList.Add(idArgs);
            }
        }

        /// <summary>
        /// 检查是否碰撞过
        /// </summary>
        public bool CheckCollided(GameIdArgs idArgs)
        {
            var index = _collidedList.FindIndex((args) => args.entityType == idArgs.entityType && args.entityId == idArgs.entityId);
            return index >= 0;
        }

        /// <summary>
        /// 清空碰撞过的实体Id列表
        /// </summary>
        public void ClearCollided()
        {
            _collidedList.Clear();
        }
    }
}
using System;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
using GameVec3 = UnityEngine.Vector3;
namespace GamePlay.Core
{
    public class GameCameraFollowCom : GameCameraComBase
    {
        public Camera camera { get; private set; }
        public GameVec3 cameraPos { get; private set; }

        public GameObject follow { get; private set; }
        public GameVec3 followOffset { get; private set; }
        GameEasing2DCom _easing2DComponent = new GameEasing2DCom();

        private Action _onComplete;

        public GameCameraFollowCom(Camera camera)
        {
            this.camera = camera;
            this.cameraPos = camera.transform.position;
        }

        public void Clear()
        {
            this.follow = null;
            this.followOffset = GameVec2.zero;
            this._easing2DComponent.Clear();
            this._onComplete = null;
        }

        protected override void _Tick(float dt)
        {
            if (this.follow == null) return;
            var targetPos = this.follow.transform.position + this.followOffset;
            if (this.cameraPos == targetPos)
            {
                this._onComplete?.Invoke();
                this._onComplete = null;
                return;
            }
            var currentPos = this.camera.transform.position;
            var easedPos = this._easing2DComponent.Tick(currentPos, targetPos, dt);
            var camPos = this.cameraPos;
            camPos.SetVec2(easedPos);
            this.cameraPos = camPos;
        }

        protected override void _Apply()
        {
            if (this.follow == null) return;
            this.camera.transform.position = this.camera.transform.position.GetSetVec2(this.cameraPos);
        }

        public void Set(GameObject follow, GameVec2 followOffset, float duration = 0.1f, GameEasingType easingType = GameEasingType.Linear)
        {
            this.follow = follow;
            this.followOffset = followOffset;
            this._easing2DComponent.SetEase(duration, easingType);
        }

        /// <summary>
        /// 设置完成回调, 会在相机到达目标位置时触发一次后清空
        /// </summary>
        /// <param name="onComplete"></param>
        public void SetOnComplete(Action onComplete)
        {
            this._onComplete = onComplete;
        }
    }
}
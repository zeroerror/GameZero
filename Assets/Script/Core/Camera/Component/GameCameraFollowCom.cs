using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    public class GameCameraFollowCom : GameCameraComBase
    {
        public Camera camera { get; private set; }
        public GameVec2 cameraPos { get; private set; }

        public GameObject follow { get; private set; }
        public GameVec2 followOffset { get; private set; }
        GameEasing2DCom _easing2DComponent = new GameEasing2DCom();

        public GameCameraFollowCom(Camera camera)
        {
            this.camera = camera;
        }

        protected override void _Tick(float dt)
        {
            if (this.follow == null) return;
            var targetPos = this.follow.transform.position.GetVec2() + this.followOffset;
            if (this.cameraPos == targetPos) return;
            var currentPos = this.camera.transform.position.GetVec2();
            var easedPos = this._easing2DComponent.Tick(currentPos, targetPos, dt);
            this.cameraPos = easedPos;
        }

        public override void Apply()
        {
            this.camera.transform.position = this.cameraPos;
        }

        public void Set(GameObject follow, GameVec2 followOffset, float duration = 0.1f, GameEasingType easingType = GameEasingType.Linear)
        {
            this.follow = follow;
            this.followOffset = followOffset;
            this._easing2DComponent.SetEase(duration, easingType);
        }
    }
}
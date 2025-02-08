using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    public class GameCameraEntity
    {
        public Camera camera { get; private set; }
        public Vector2 position => this.camera.transform.position;

        public GameCameraFollowCom followCom { get; private set; }
        public GameCameraConfinerCom confinerCom { get; private set; }
        public GameCameraShakeCom shakeCom { get; private set; }
        public GameCameraZoomCom zoomCom { get; private set; }

        public GameCameraEntity(Camera camera)
        {
            this.camera = camera;
            this.followCom = new GameCameraFollowCom(camera);
            this.confinerCom = new GameCameraConfinerCom(camera);
            this.shakeCom = new GameCameraShakeCom(camera);
            this.zoomCom = new GameCameraZoomCom(camera);
        }

        public void Tick(float dt)
        {
            this._TickComponent(dt);
            this._Apply();
        }

        private void _TickComponent(float dt)
        {
            this.followCom.Tick(dt);
            this.confinerCom.Tick(dt);
            this.shakeCom.Tick(dt);
            this.zoomCom.Tick(dt);
        }

        private void _Apply()
        {
            this.followCom.Apply();
            this.shakeCom.Apply();
            this.zoomCom.Apply();
            this.confinerCom.Apply();
        }

        public GameVec2 GetWorldPoint(GameVec2 screenPoint)
        {
            return this.camera.ScreenToWorldPoint(screenPoint);
        }
    }
}
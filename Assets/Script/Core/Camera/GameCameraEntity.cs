using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    public class GameCameraEntity
    {
        public Camera camera { get; private set; }
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

        void _Test()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // var randomAngle = Random.Range(0, 360);
                // var randomAmplitude = Random.Range(0.5f, 2.5f);
                // this.shakeCom.Shake(randomAngle, randomAmplitude, 10, 0.5f);

                this.zoomCom.Zoom(5, 4, 1, GameEasingType.Linear, false, () =>
                {
                    this.zoomCom.Zoom(4, 5, 0.5f, GameEasingType.Linear, false);
                });
            }
        }

        public void Tick(float dt)
        {
            this._TickComponent(dt);
            this._Apply();
            this._Test();
        }

        void _TickComponent(float dt)
        {
            this.followCom.Tick(dt);
            this.confinerCom.Tick(dt);
            this.shakeCom.Tick(dt);
            this.zoomCom.Tick(dt);
        }

        void _Apply()
        {
            this.followCom.Apply();
            this.shakeCom.Apply();
            this.zoomCom.Apply();
            this.confinerCom.Apply();
        }
    }
}
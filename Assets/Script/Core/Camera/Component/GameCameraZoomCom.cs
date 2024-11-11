using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    public class GameCameraZoomCom
    {
        public Camera camera { get; private set; }
        public float zoomRatio { get; private set; } = 1.0f;
        public GameCameraZoomCom(Camera camera)
        {
            this.camera = camera;
        }

        public void Tick(float dt)
        {
        }

        public void Apply()
        {
        }
    }
}
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    public class GameCameraShakeCom
    {
        public Camera camera { get; private set; }
        public GameVec2 shakeOffset { get; private set; }

        public GameCameraShakeCom(Camera camera)
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
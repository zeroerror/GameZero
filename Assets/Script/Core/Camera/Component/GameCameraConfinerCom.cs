using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    public class GameCameraConfinerCom
    {
        public Camera camera { get; private set; }
        public GameVec2 cameraPos { get; private set; }
        public GameVec2 min { get; private set; }
        public GameVec2 max { get; private set; }

        public GameCameraConfinerCom(Camera camera)
        {
            this.camera = camera;
        }

        public void Tick(float dt)
        {
        }

        public void SetConfiner(in GameVec2 min, in GameVec2 max)
        {
            this.min = min;
            this.max = max;
        }

        public void Apply()
        {
        }
    }
}
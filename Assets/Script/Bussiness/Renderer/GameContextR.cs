using GamePlay.Core;
using UnityEngine;
namespace GamePlay.Bussiness.Renderer
{
    public class GameContextR
    {
        public GameDirectorR director { get; private set; } = new GameDirectorR();
        public GameEventService eventService { get; private set; } = new GameEventService();
        public GameCameraEntity cameraEntity { get; private set; }
        public GameContextR()
        {
            this.cameraEntity = new GameCameraEntity(GameObject.Find("Main Camera")?.GetComponent<Camera>());
        }
    }
}
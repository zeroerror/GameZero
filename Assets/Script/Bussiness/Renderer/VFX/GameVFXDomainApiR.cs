using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public interface GameVFXDomainApiR
    {
        public GameVFXEntityR Play(in GameVFXPlayArgs args);
    }
}
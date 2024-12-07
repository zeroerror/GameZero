using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    [System.Serializable]
    public class GameActionModelR
    {
        [HideInInspector]
        public int typeId;
        public AnimationClip vfxClip;
        public GameCameraShakeModel shakeModel;

        public GameActionModelR(int typeId, AnimationClip vfxClip, GameCameraShakeModel shakeModel)
        {
            this.typeId = typeId;
            this.vfxClip = vfxClip;
            this.shakeModel = shakeModel;
        }
    }
}
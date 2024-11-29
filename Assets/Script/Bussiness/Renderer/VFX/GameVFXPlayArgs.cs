using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Renderer
{
    public struct GameVFXPlayArgs
    {
        public AnimationClip clip;
        public GameVec2 position;
        public float angle;
        public float scale;
        // 0非循环，-1无限循环，>0循环时间
        public float loopDuration;

        // 挂载节点
        public GameObject attachNode;
        public GameVec2 attachOffset;
        // 是否将attachNode作为父节点
        public bool isAttachParent;
    }
}
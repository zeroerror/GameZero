using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Render
{
    public struct GameVFXPlayArgs
    {
        public string url;

        public GameVec2 position;
        public float angle;
        public GameVec2 scale;
        /// <summary> 0非循环，-1无限循环，>0循环时间 </summary>
        public float loopDuration;

        /// <summary> 挂载节点 </summary>
        public GameObject attachNode;
        /// <summary> 挂载偏移 </summary>
        public GameVec2 attachOffset;
        /// <summary> 是否将attachNode作为父节点 </summary>
        public bool isAttachParent;

        /// <summary> 层级类型 </summary>
        public GameFieldLayerType layerType;
        /// <summary> 层级偏移 </summary>
        public int orderOffset;
    }
}
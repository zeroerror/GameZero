using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    /** 相机震屏参数模型 */
    [System.Serializable]
    public class GameCameraShakeModel
    {
        [Header("震屏角度"), Range(0, 360)]
        public float angle;
        [Header("震屏振幅")]
        public float amplitude;
        [Header("震屏频率")]
        public float frequency;
        [Header("震屏持续时间")]
        public float duration;

        public GameCameraShakeModel(float angle, float amplitude, float frequency, float duration)
        {
            this.angle = angle;
            this.amplitude = amplitude;
            this.frequency = frequency;
            this.duration = duration;
        }
    }
}

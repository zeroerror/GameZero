using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    /** 相机震屏参数模型 */
    [System.Serializable]
    public class GameCameraShakeModel
    {
        public float angle;
        public float amplitude;
        public float frequency;
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

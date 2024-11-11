using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    /** 相机震屏参数模型 */
    public class GameCameraShakeModel
    {
        public readonly float ShakeDuration;
        public readonly GameVec2 ShakeDirection;
        public readonly float ShakeAmplitude;
        public readonly float ShakeFrequency;

        public GameCameraShakeModel(
            float shakeDuration,
            in GameVec2 shakeDirection,
            float shakeAmplitude,
            float shakeFrequency)
        {
            this.ShakeDuration = shakeDuration;
            this.ShakeDirection = shakeDirection;
            this.ShakeAmplitude = shakeAmplitude;
            this.ShakeFrequency = shakeFrequency;
        }

        public string GetParsedStr()
        {
            string dirElement = $"{ShakeDirection.x},{ShakeDirection.y}";
            string str = $"{ShakeDuration}&{dirElement}&{ShakeAmplitude}&{ShakeFrequency}";
            return str;
        }

        public static GameCameraShakeModel ParseToModel(string str)
        {
            if (!IsStrValid(str)) return null;
            var cameraShakeModelStr = str.Split('&');
            var dirElement = cameraShakeModelStr[1].Split(',');

            var model = new GameCameraShakeModel(
                float.Parse(cameraShakeModelStr[0]),
                new GameVec2(float.Parse(dirElement[0]), float.Parse(dirElement[1])),
                float.Parse(cameraShakeModelStr[2]),
                float.Parse(cameraShakeModelStr[3])
            );
            return model;
        }

        /** 配置字符串是否有效 */
        private static bool IsStrValid(string str)
        {
            return !string.IsNullOrEmpty(str) && str != "undefined";
        }
    }
}

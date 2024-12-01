using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    public class GameEasing2DCom
    {
        private GameEasing1DCom _ease1DComponentX = new GameEasing1DCom();
        private GameEasing1DCom _ease1DComponentY = new GameEasing1DCom();

        public GameEasing2DCom() { }

        public void SetEase(float durationX, GameEasingType easingTypeX, float? durationY = null, GameEasingType? easingTypeY = null)
        {
            _ease1DComponentX.SetEase(durationX, easingTypeX);
            _ease1DComponentY.SetEase(durationY ?? durationX, easingTypeY ?? easingTypeX);
        }

        public GameVec2 Tick(in GameVec2 from, in GameVec2 to, float dt)
        {
            float easedX = _ease1DComponentX.Tick(from.x, to.x, dt);
            float easedY = _ease1DComponentY.Tick(from.y, to.y, dt);
            return new GameVec2(easedX, easedY);
        }

        public void SetResetTime(bool isResetTime)
        {
            _ease1DComponentX.SetIsResetTime(isResetTime);
            _ease1DComponentY.SetIsResetTime(isResetTime);
        }

        public void Clear()
        {
            _ease1DComponentX.Clear();
            _ease1DComponentY.Clear();
        }
    }

}
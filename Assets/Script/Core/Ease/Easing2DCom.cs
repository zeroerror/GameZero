using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    public class Easing2DCom
    {
        private Easing1DCom _ease1DComponentX = new Easing1DCom();
        private Easing1DCom _ease1DComponentY = new Easing1DCom();

        public Easing2DCom() { }

        public void SetEase(float durationX, EasingType easingTypeX, float? durationY = null, EasingType? easingTypeY = null)
        {
            _ease1DComponentX.SetEase(durationX, easingTypeX);
            _ease1DComponentY.SetEase(durationY ?? durationX, easingTypeY ?? easingTypeX);
        }

        public GameVec2 TickEase(in GameVec2 from, in GameVec2 to, float dt)
        {
            float easedX = _ease1DComponentX.TickEase(from.x, to.x, dt);
            float easedY = _ease1DComponentY.TickEase(from.y, to.y, dt);
            return new GameVec2(easedX, easedY);
        }

        public void SetResetTime(bool isResetTime)
        {
            _ease1DComponentX.SetIsResetTime(isResetTime);
            _ease1DComponentY.SetIsResetTime(isResetTime);
        }

        public void Reset()
        {
            _ease1DComponentX.Clear();
            _ease1DComponentY.Clear();
        }
    }

}
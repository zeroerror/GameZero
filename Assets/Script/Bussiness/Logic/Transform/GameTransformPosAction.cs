using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameTransformPosAction
    {
        public GameTransformCom transCom;
        public GameVec2 position;
        public float duration;
        public GameEasingType easingType;

        public float elapsedTime;
        public GameEasing2DCom posEasingCom;

        public GameTransformPosAction(
            GameTransformCom transCom,
            in GameVec2 position,
            float duration,
            GameEasingType easingType
        )
        {
            this.transCom = transCom;
            this.position = position;
            this.duration = duration;
            this.easingType = easingType;
            this.posEasingCom = new GameEasing2DCom();
            this.posEasingCom.SetEase(duration, easingType);
        }

        /// <summary>
        /// 驱动行为逻辑, 返回是否结束
        /// </summary>
        public bool Tick(float dt)
        {
            this.elapsedTime += dt;
            var easedPos = this.posEasingCom.Tick(this.transCom.position, this.position, this.elapsedTime);
            this.transCom.position = easedPos;
            return this.elapsedTime >= this.duration;
        }
    }
}
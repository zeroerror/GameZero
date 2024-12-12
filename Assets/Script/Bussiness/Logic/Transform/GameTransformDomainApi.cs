using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public interface GameTransformDomainApi
    {
        /// <summary>
        /// 从当前位置移动到目标位置
        /// <para> transCom 变换组件</para>
        /// <para> toArgs 目标位置</para>
        /// <para> duration 持续时间</para>
        /// <para> easingType 缓动曲线</para>
        /// </summary>
        public void ToPosition(GameTransformCom transCom, in GameVec2 toPos, float duration, GameEasingType easingType);
    }
}

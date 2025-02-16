using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public interface GameTransformDomainApiR
    {
        /// <summary>
        /// 抖动
        /// <para>transform: 对象</para>
        /// <para>angle: 角度</para>
        /// <para>amplitude: 幅度</para>
        /// <para>frequency: 频率</para>
        /// <para>duration: 持续时间</para>
        /// </summary>
        public void Shake(Transform transform, float angle, float amplitude, float frequency, float duration);
    }

}

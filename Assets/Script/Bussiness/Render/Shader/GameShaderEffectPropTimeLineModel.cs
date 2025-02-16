using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public struct GameShaderEffectPropTimeLineModel
    {
        /// <summary> 参数名称 </summary>
        public string propName;
        /// <summary> 开始时间 </summary>
        public float startTime;
        /// <summary> 结束时间 </summary>
        public float endTime;

        /// <summary> 起始值(float) </summary>
        public float fromValue_float;
        /// <summary> 结束值(float) </summary>
        public float toValue_float;
        /// <summary> 曲线(float) </summary>
        public AnimationCurve curve_float;
    }
}
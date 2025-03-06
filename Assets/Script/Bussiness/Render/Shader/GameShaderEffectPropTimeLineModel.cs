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

        /// <summary> 是否启用(float) </summary>
        public bool isEnable_float;
        /// <summary> 起始值(float) </summary>
        public float fromValue_float;
        /// <summary> 结束值(float) </summary>
        public float toValue_float;
        /// <summary> 曲线(float) </summary>
        public AnimationCurve curve_float;

        /// <summary> 是否启用(Color) </summary>    
        public bool isEnable_color;
        /// <summary> 起始值(Color) </summary>
        public Color fromValue_color;
        /// <summary> 结束值(Color) </summary>
        public Color toValue_color;
        /// <summary> 曲线(Color) </summary>
        public AnimationCurve curve_color;
    }
}
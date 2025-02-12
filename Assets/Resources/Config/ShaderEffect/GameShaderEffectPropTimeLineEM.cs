using GamePlay.Bussiness.Render;
using UnityEngine;

namespace GamePlay.Config
{
    /// <summary>
    /// Shader效果属性时间轴
    /// </summary>
    [System.Serializable]
    public struct GameShaderEffectPropTimeLineEM
    {
        /// <summary> 材质 </summary>
        public Material material;

        /// <summary> 属性名称 </summary>
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

        /// <summary> 起始值(Color) </summary>
        public Color fromValue_color;
        /// <summary> 结束值(Color) </summary>
        public Color toValue_color;

        public GameShaderEffectPropTimeLineModel ToModel()
        {
            GameShaderEffectPropTimeLineModel model;
            model.propName = propName;
            model.startTime = startTime;
            model.endTime = endTime;
            model.fromValue_float = fromValue_float;
            model.toValue_float = toValue_float;
            model.curve_float = curve_float;
            return model;
        }
    }

    public static class GameShaderEffectPropTimeLineEMExtension
    {
        public static GameShaderEffectPropTimeLineModel[] ToModels(this GameShaderEffectPropTimeLineEM[] ems)
        {
            var models = new GameShaderEffectPropTimeLineModel[ems.Length];
            for (int i = 0; i < ems.Length; i++)
            {
                models[i] = ems[i].ToModel();
            }
            return models;
        }
    }
}
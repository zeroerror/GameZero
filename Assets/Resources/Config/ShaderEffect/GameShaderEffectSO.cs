using GamePlay.Bussiness.Render;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_shaderEffect_", menuName = "游戏玩法/配置/Shader效果模板")]
    public class GameShaderEffectSO : GameSOBase
    {
        /// <summary> 描述 </summary>
        public string desc;
        /// <summary> shader </summary>
        public Shader shader;
        /// <summary> 循环次数(0表示无限循环) </summary>
        public int loopCount;
        /// <summary> 参数时间轴 </summary>
        public GameShaderEffectPropTimeLineEM[] propTimeLines;

        public GameShaderEffectModel ToModel()
        {
            var shaderUrl = shader.GetResRelativeUrl();
            var model = new GameShaderEffectModel(
                typeId,
                desc,
                shaderUrl,
                loopCount,
                propTimeLines.ToModels()
            );
            return model;
        }
    }
}

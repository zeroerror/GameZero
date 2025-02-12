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
        /// <summary> 材质 </summary>
        public Material material;
        /// <summary> 循环次数(0表示无限循环) </summary>
        public int loopCount;
        /// <summary> 参数时间轴 </summary>
        public GameShaderEffectPropTimeLineEM[] propTimeLines;

        public GameShaderEffectModel ToModel()
        {
            if (!material)
            {
                GameLogger.LogError("GameShaderEffectSO.ToModel: material is null. typeId: " + typeId);
                return null;
            }
            var materialUrl = material.GetResRelativeUrl();
            var model = new GameShaderEffectModel(
                typeId,
                desc,
                materialUrl,
                loopCount,
                propTimeLines.ToModels()
            );
            return model;
        }
    }
}

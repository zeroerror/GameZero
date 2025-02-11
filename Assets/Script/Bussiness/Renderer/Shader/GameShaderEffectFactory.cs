using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public class GameShaderEffectFactory
    {
        public GameShaderEffectTemplate template { get; private set; }

        public GameShaderEffectFactory()
        {
            template = new GameShaderEffectTemplate();
        }

        public GameShaderEffectEntity Load(int typeId)
        {
            if (!template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("GameShaderEffectFactoryR.Load: typeId not found: " + typeId);
                return null;
            }
            var shader = this.LoadShader(typeId);
            var mat = new Material(shader);
            var entity = new GameShaderEffectEntity(model, mat);
            return entity;
        }

        public Shader LoadShader(int typeId)
        {
            if (!template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("GameShaderEffectFactoryR.LoadShader: typeId not found: " + typeId);
                return null;
            }
            var url = model.shaderUrl;
            var shader = Resources.Load<Shader>(url);
            if (shader == null)
            {
                GameLogger.LogError("GameShaderEffectFactoryR.LoadShader: shader not found: " + url);
            }
            return shader;
        }
    }
}
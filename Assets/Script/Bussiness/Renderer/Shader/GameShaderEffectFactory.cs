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
            var mat = this.LoadMaterial(typeId);
            var entity = new GameShaderEffectEntity(model, mat);
            return entity;
        }

        public Material LoadMaterial(int typeId)
        {
            if (!template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("GameShaderEffectFactoryR.LoadMaterial: typeId not found: " + typeId);
                return null;
            }
            var url = model.materialUrl;
            var mat = Resources.Load<Material>(url);
            if (mat == null)
            {
                GameLogger.LogError("GameShaderEffectFactoryR.LoadMaterial: shader not found: " + url);
            }
            return mat;
        }
    }
}
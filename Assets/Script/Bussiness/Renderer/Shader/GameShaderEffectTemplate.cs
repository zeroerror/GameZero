using System.Collections.Generic;
using GamePlay.Config;
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public class GameShaderEffectTemplate
    {
        private Dictionary<int, GameShaderEffectModel> _dict;
        private Dictionary<int, GameShaderEffectSO> _soDict;

        public GameShaderEffectTemplate()
        {
            _dict = new Dictionary<int, GameShaderEffectModel>();
            var path = GameConfigCollection.SHADER_EFFECT_CONFIG_DIR_PATH;
            var resList = Resources.LoadAll(path, typeof(GameShaderEffectSO));
            _soDict = new Dictionary<int, GameShaderEffectSO>();
            foreach (var res in resList)
            {
                var so = res as GameShaderEffectSO;
                _soDict.Add(so.typeId, so);
            }
        }

        public bool TryGet(int typeId, out GameShaderEffectModel model)
        {
            if (_dict.TryGetValue(typeId, out model))
            {
                return true;
            }
            if (!_soDict.TryGetValue(typeId, out var so))
            {
                model = null;
                return false;
            }
            model = so.ToModel();
            _dict.Add(typeId, model);
            return true;
        }
    }
}
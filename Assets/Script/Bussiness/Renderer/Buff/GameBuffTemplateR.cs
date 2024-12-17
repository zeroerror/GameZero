using System.Collections.Generic;
using GamePlay.Config;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameBuffTemplateR
    {
        private Dictionary<int, GameBuffModelR> _dict;
        private Dictionary<int, GameBuffSO> _soDict;

        public GameBuffTemplateR()
        {
            _dict = new Dictionary<int, GameBuffModelR>();
            var path = GameConfigCollection.BUFF_CONFIG_DIR_PATH;
            var resList = Resources.LoadAll(path, typeof(GameBuffSO));
            _soDict = new Dictionary<int, GameBuffSO>();
            foreach (var res in resList)
            {
                var so = res as GameBuffSO;
                _soDict.Add(so.typeId, so);
            }
        }

        public bool TryGet(int typeId, out GameBuffModelR model)
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
            model = so.ToModelR();
            _dict.Add(typeId, model);
            return true;
        }
    }
}
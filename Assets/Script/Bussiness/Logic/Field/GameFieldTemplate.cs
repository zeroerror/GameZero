using System.Collections.Generic;
using GamePlay.Config;
using GamePlay.Core;
using GamePlay.Infrastructure;
using UnityEngine;

namespace GamePlay.Bussiness.Logic
{
    public class GameFieldTemplate
    {
        private Dictionary<int, GameFieldModel> _dict;
        private Dictionary<int, GameFieldSO> _soDict;

        public GameFieldTemplate()
        {
            _dict = new Dictionary<int, GameFieldModel>();
            var path = GameConfigCollection.FIELD_CONFIG_DIR_PATH;
            var resList = GameResourceManager.LoadAll(path, typeof(GameFieldSO));
            _soDict = new Dictionary<int, GameFieldSO>();
            foreach (var res in resList)
            {
                var so = res as GameFieldSO;
                _soDict.Add(so.typeId, so);
            }
        }

        public bool TryGet(int typeId, out GameFieldModel model)
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
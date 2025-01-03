using System.Collections.Generic;
using GamePlay.Config;
using UnityEngine;

namespace GamePlay.Bussiness.Logic
{
    public class GameActionTemplate
    {
        private Dictionary<int, GameActionModelBase> _dict;
        private Dictionary<int, GameActionSO> _soDict;

        public GameActionTemplate()
        {
            _dict = new Dictionary<int, GameActionModelBase>();
            var path = GameConfigCollection.ACTION_CONFIG_DIR_PATH;
            var resList = Resources.LoadAll(path, typeof(GameActionSO));
            _soDict = new Dictionary<int, GameActionSO>();
            foreach (var res in resList)
            {
                var so = res as GameActionSO;
                _soDict.Add(so.typeId, so);
            }
        }

        public bool TryGet(int typeId, out GameActionModelBase model)
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
            model = so.GetActionModel();
            _dict.Add(typeId, model);
            return true;
        }
    }
}
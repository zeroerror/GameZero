using System.Collections.Generic;
using GamePlay.Config;
using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.Logic
{
    public class GameActionOptionTemplate
    {
        private Dictionary<int, GameActionOptionModel> _dict;
        private Dictionary<int, GameActionOptionSO> _soDict;

        public GameActionOptionTemplate()
        {
            _dict = new Dictionary<int, GameActionOptionModel>();
            var path = GameConfigCollection.ACTION_OPTION_CONFIG_DIR_PATH;
            var resList = GameResourceManager.LoadAll(path, typeof(GameActionOptionSO));
            _soDict = new Dictionary<int, GameActionOptionSO>();
            foreach (var res in resList)
            {
                var so = res as GameActionOptionSO;
                if (so.disable) continue;
                _soDict.Add(so.typeId, so);
            }
        }

        public bool TryGet(int typeId, out GameActionOptionModel model)
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

        public List<GameActionOptionModel> GetActionOptionModelList()
        {
            var list = new List<GameActionOptionModel>();
            foreach (var so in _soDict.Values)
            {
                if (!TryGet(so.typeId, out var model)) continue;
                list.Add(model);
            }
            return list;
        }
    }
}
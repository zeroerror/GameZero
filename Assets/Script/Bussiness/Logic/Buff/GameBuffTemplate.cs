using System.Collections.Generic;
using GamePlay.Config;
using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameBuffTemplate
    {
        private Dictionary<int, GameBuffModel> _dict;
        private Dictionary<int, GameBuffSO> _soDict;

        public GameBuffTemplate()
        {
            _dict = new Dictionary<int, GameBuffModel>();
            var path = GameConfigCollection.BUFF_CONFIG_DIR_PATH;
            var resList = GameResourceService.LoadAll(path, typeof(GameBuffSO));
            _soDict = new Dictionary<int, GameBuffSO>();
            foreach (var res in resList)
            {
                var so = res as GameBuffSO;
                _soDict.Add(so.typeId, so);
            }
        }

        public bool TryGet(int typeId, out GameBuffModel model)
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
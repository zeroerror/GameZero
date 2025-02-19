using System.Collections.Generic;
using GamePlay.Config;
using GamePlay.Core;

namespace GamePlay.Bussiness.Render
{
    public class GameFieldTemplateR
    {
        private Dictionary<int, GameFieldModelR> _dict;
        private Dictionary<int, GameFieldSO> _soDict;

        public GameFieldTemplateR()
        {
            _dict = new Dictionary<int, GameFieldModelR>();
            var path = GameConfigCollection.FIELD_CONFIG_DIR_PATH;
            var resList = GameResourceManager.LoadAll(path, typeof(GameFieldSO));
            _soDict = new Dictionary<int, GameFieldSO>();
            foreach (var res in resList)
            {
                var so = res as GameFieldSO;
                _soDict.Add(so.typeId, so);
            }
        }

        public bool TryGet(int typeId, out GameFieldModelR model)
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
            model = new GameFieldModelR();
            model.typeId = so.typeId;
            model.fieldPrefab = so.fieldPrefab;
            _dict.Add(typeId, model);
            return true;
        }
    }
}
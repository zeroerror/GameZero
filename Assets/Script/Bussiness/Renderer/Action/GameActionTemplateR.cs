using System.Collections.Generic;
using GamePlay.Config;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameActionTemplateR
    {
        private Dictionary<int, GameActionModelR> _dict;
        private Dictionary<int, GameActionSO> _soDict;

        public GameActionTemplateR()
        {
            _dict = new Dictionary<int, GameActionModelR>();
            var path = GameConfigCollection.ACTION_CONFIG_DIR_PATH;
            var soList = Resources.LoadAll(path, typeof(GameActionSO));
            _soDict = new Dictionary<int, GameActionSO>();
            foreach (var so in soList)
            {
                var skillSo = so as GameActionSO;
                if (skillSo == null)
                {
                    Debug.LogError("GameActionTemplateR: LoadAll: invalid GameActionSO: " + so);
                    continue;
                }
                _soDict.Add(skillSo.typeId, skillSo);
            }
        }

        public bool TryGet(int typeId, out GameActionModelR model)
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
            model = so.actionR;
            _dict.Add(typeId, model);
            return true;
        }
    }
}
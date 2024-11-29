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
            var path = GameConfigCollection.SKILL_CONFIG_DIR_PATH;
            var soList = Resources.LoadAll(path, typeof(GameActionSO));
            _soDict = new Dictionary<int, GameActionSO>();
            foreach (var so in soList)
            {
                var skillSo = so as GameActionSO;
                if (skillSo == null)
                {
                    Debug.LogError("GameActionTemplate: LoadAll: invalid GameActionSO: " + so);
                    continue;
                }
                _soDict.Add(skillSo.typeId, skillSo);
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
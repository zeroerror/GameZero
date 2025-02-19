using System.Collections.Generic;
using GamePlay.Config;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Logic
{
    public class GameSkillTemplate
    {
        private Dictionary<int, GameSkillModel> _dict;
        private Dictionary<int, GameSkillSO> _soDict;

        public GameSkillTemplate()
        {
            _dict = new Dictionary<int, GameSkillModel>();
            var path = GameConfigCollection.SKILL_CONFIG_DIR_PATH;
            var soList = GameResourceManager.LoadAll(path, typeof(GameSkillSO));
            _soDict = new Dictionary<int, GameSkillSO>();
            foreach (var so in soList)
            {
                var skillSo = so as GameSkillSO;
                if (skillSo == null)
                {
                    Debug.LogError("GameSkillTemplate: LoadAll: invalid GameSkillSO: " + so);
                    continue;
                }
                _soDict.Add(skillSo.typeId, skillSo);
            }
        }

        public bool TryGet(int typeId, out GameSkillModel model)
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
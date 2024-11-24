using System.Collections.Generic;
using GamePlay.Config;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameSkillTemplateR
    {
        private Dictionary<int, GameSkillModelR> _dict;
        private Dictionary<int, GameSkillSO> _soDict;

        public GameSkillTemplateR()
        {
            _dict = new Dictionary<int, GameSkillModelR>();
            var path = GameConfigCollection.SKILL_CONFIG_DIR_PATH;
            var soList = Resources.LoadAll(path, typeof(GameSkillSO));
            _soDict = new Dictionary<int, GameSkillSO>();
            foreach (var so in soList)
            {
                var skillSo = so as GameSkillSO;
                if (skillSo == null)
                {
                    Debug.LogError("GameSkillTemplateR: LoadAll: invalid GameSkillSO: " + so);
                    continue;
                }
                _soDict.Add(skillSo.typeId, skillSo);
            }
        }

        public bool TryGet(int typeId, out GameSkillModelR model)
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
            var animName = so.animName;
            model = new GameSkillModelR(typeId, animName, so.animLength);
            _dict.Add(typeId, model);
            return true;
        }
    }
}
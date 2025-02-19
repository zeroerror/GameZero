using System.Collections.Generic;
using GamePlay.Config;
using GamePlay.Core;

namespace GamePlay.Bussiness.Render
{
    public class GameSkillTemplateR
    {
        private Dictionary<int, GameSkillModelR> _dict;
        private Dictionary<int, GameSkillSO> _soDict;

        public GameSkillTemplateR()
        {
            _dict = new Dictionary<int, GameSkillModelR>();
            var path = GameConfigCollection.SKILL_CONFIG_DIR_PATH;
            var soList = GameResourceManager.LoadAll(path, typeof(GameSkillSO));
            _soDict = new Dictionary<int, GameSkillSO>();
            foreach (var so in soList)
            {
                var skillSo = so as GameSkillSO;
                if (skillSo == null)
                {
                    GameLogger.LogError("GameSkillTemplateR: LoadAll: invalid GameSkillSO: " + so);
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
            model = so.ToModelR();
            _dict.Add(typeId, model);
            return true;
        }
    }
}
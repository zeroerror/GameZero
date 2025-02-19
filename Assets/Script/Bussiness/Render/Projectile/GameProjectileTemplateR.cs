using System.Collections.Generic;
using GamePlay.Config;
using GamePlay.Core;

namespace GamePlay.Bussiness.Render
{
    public class GameProjectileTemplateR
    {
        private Dictionary<int, GameProjectileModelR> _dict;
        private Dictionary<int, GameProjectileSO> _soDict;

        public GameProjectileTemplateR()
        {
            _dict = new Dictionary<int, GameProjectileModelR>();
            var path = GameConfigCollection.PROJECTILE_CONFIG_DIR_PATH;
            var resList = GameResourceManager.LoadAll(path, typeof(GameProjectileSO));
            _soDict = new Dictionary<int, GameProjectileSO>();
            foreach (var res in resList)
            {
                var so = res as GameProjectileSO;
                _soDict.Add(so.typeId, so);
            }
        }

        public bool TryGet(int typeId, out GameProjectileModelR model)
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
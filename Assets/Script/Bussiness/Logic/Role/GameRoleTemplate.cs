using System.Collections.Generic;
using GamePlay.Config;
using UnityEngine;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleTemplate
    {
        private Dictionary<int, GameRoleModel> _dict;
        private Dictionary<int, GameRoleSO> _soDict;

        public GameRoleTemplate()
        {
            _dict = new Dictionary<int, GameRoleModel>();
            var path = GameConfigCollection.ROLE_CONFIG_DIR_PATH;
            var resList = Resources.LoadAll(path, typeof(GameRoleSO));
            _soDict = new Dictionary<int, GameRoleSO>();
            foreach (var res in resList)
            {
                var so = res as GameRoleSO;
                _soDict.Add(so.typeId, so);
            }
        }

        public bool TryGet(int typeId, out GameRoleModel model)
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
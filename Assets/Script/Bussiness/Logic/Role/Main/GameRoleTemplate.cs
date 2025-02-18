using System.Collections.Generic;
using GamePlay.Config;
using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleTemplate
    {
        private readonly Dictionary<int, GameRoleModel> _dict;
        private readonly Dictionary<int, GameRoleSO> _soDict;

        public GameRoleTemplate()
        {
            _dict = new Dictionary<int, GameRoleModel>();
            var path = GameConfigCollection.ROLE_CONFIG_DIR_PATH;
            var resList = GameResourceService.LoadAll(path, typeof(GameRoleSO));
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

        public List<GameRoleModel> GetAll()
        {
            var list = new List<GameRoleModel>();
            foreach (var so in _soDict.Values)
            {
                list.Add(so.ToModel());
            }
            return list;
        }
    }
}
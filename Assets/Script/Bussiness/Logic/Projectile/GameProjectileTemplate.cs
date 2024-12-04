using System.Collections.Generic;
using GamePlay.Config;
using UnityEngine;

namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileTemplateR
    {
        private Dictionary<int, GameProjectileModel> _dict;
        private Dictionary<int, GameProjectileSO> _soDict;

        public GameProjectileTemplateR()
        {
            _dict = new Dictionary<int, GameProjectileModel>();
            var path = GameConfigCollection.PROJECTILE_CONFIG_DIR_PATH;
            var resList = Resources.LoadAll(path, typeof(GameProjectileSO));
            _soDict = new Dictionary<int, GameProjectileSO>();
            foreach (var res in resList)
            {
                var so = res as GameProjectileSO;
                _soDict.Add(so.typeId, so);
            }
        }

        public bool TryGet(int typeId, out GameProjectileModel model)
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
            var modelSetDict = new Dictionary<GameProjectileStateType, GameProjectileStateTriggerModelSet>();
            foreach (var triggerEM in so.stateTriggers)
            {
                modelSetDict.Add(triggerEM.stateType, triggerEM.emSet.ToModelSet());
            }
            model = new GameProjectileModel(typeId, so.animLength, so.timelineEvents.ToModels(), modelSetDict, so.lifeTime);
            _dict.Add(typeId, model);
            return true;
        }
    }
}
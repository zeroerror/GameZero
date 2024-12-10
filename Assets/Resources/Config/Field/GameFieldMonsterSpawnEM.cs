using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public struct GameFieldMonsterSpawnEM
    {
        [Header("角色模板")]
        public GameRoleSO roleSO;
        [Header("生成数量")]
        public int count;

        public GameFieldMonsterSpawnModel ToModel()
        {
            if (!roleSO) return default;
            return new GameFieldMonsterSpawnModel(roleSO.typeId, count);
        }
    }
}
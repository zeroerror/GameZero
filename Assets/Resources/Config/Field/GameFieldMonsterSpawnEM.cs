using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public struct GameFieldMonsterSpawnEM
    {
        [Header("角色类型ID")]
        public int typeId;
        [Header("生成数量")]
        public int count;

        public GameFieldMonsterSpawnModel ToModel()
        {
            return new GameFieldMonsterSpawnModel(typeId, count);
        }
    }
}
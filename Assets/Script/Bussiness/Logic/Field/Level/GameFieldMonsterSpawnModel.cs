using UnityEngine;

namespace GamePlay.Bussiness.Logic
{
    public struct GameFieldMonsterSpawnModel
    {
        public int typeId;
        public int count;

        public GameFieldMonsterSpawnModel(int typeId, int count)
        {
            this.typeId = typeId;
            this.count = count;
        }
    }
}
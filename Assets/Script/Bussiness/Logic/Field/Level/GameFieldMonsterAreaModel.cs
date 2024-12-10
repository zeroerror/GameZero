using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// 游戏场地怪物区域模型
    /// </summary>
    public struct GameFieldMonsterAreaModel
    {
        /// <summary> 生成位置 </summary>
        public GameVec2 position;
        /// <summary> 生成半径 </summary>
        public float radius;
        /// <summary> 怪物生成模型列表 </summary>
        public GameFieldMonsterSpawnModel[] monsterSpawnModels;

        /// <summary> 生成时机(开局后n秒) </summary>
        public int spawnTime;

        public GameFieldMonsterAreaModel(in GameVec2 position, float radius, in GameFieldMonsterSpawnModel[] monsterSpawnModels, int spawnTime)
        {
            this.position = position;
            this.radius = radius;
            this.monsterSpawnModels = monsterSpawnModels;
            this.spawnTime = spawnTime;
        }
    }
}
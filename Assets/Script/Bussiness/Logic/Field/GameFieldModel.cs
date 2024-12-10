using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameFieldModel
    {
        public readonly int typeId;
        public readonly GameFieldMonsterAreaModel[] monsterAreaModels;

        public GameFieldModel(int typeId, GameFieldMonsterAreaModel[] monsterAreaModels)
        {
            this.typeId = typeId;
            this.monsterAreaModels = monsterAreaModels;
            this.monsterAreaModels?.Sort((a, b) => a.spawnTime.CompareTo(b.spawnTime));
        }
    }
}
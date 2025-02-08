using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public class GameFieldEntity : GameEntityBase
    {
        public GameFieldModel model { get; private set; }
        private Dictionary<int, bool> _monsterSpawnedDict;

        public GameFieldEntity(GameFieldModel model) : base(model.typeId, GameEntityType.Field)
        {
            this.model = model;
            this._monsterSpawnedDict = new Dictionary<int, bool>();
        }

        public override void Tick(float dt)
        {
        }

        public override void Clear()
        {
            base.Clear();
            this.ResetMonsterSpawned();
        }

        /// <summary>
        /// 重置怪物生成状态
        /// </summary>
        public void ResetMonsterSpawned()
        {
            this._monsterSpawnedDict.Clear();
        }

        public override void Destroy()
        {
        }

        /// <summary>
        /// 判定区域内怪物是否已经生成
        /// </summary>
        /// <param name="areaIndex"></param>
        /// <returns></returns>
        public bool IsMonstersSpawned(int areaIndex)
        {
            return this._monsterSpawnedDict.ContainsKey(areaIndex) && this._monsterSpawnedDict[areaIndex];
        }
        public void SetMonsterSpawned(int areaIndex, bool isSpawned)
        {
            this._monsterSpawnedDict[areaIndex] = isSpawned;
        }
    }
}
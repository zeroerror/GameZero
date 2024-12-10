using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    public class GameFieldMonsterAreaCom : MonoBehaviour
    {
        [Header("生成半径")]
        public float radius;
        [Header("怪物生成模型列表")]
        public GameFieldMonsterSpawnEM[] monsterSpawnEMs;
        [Header("生成时机(开局后n秒)")]
        public int spawnTime;

        public GameFieldMonsterAreaEM ToEM()
        {
            GameFieldMonsterAreaEM model = new GameFieldMonsterAreaEM(
                this.transform.position,
                radius,
                monsterSpawnEMs,
                spawnTime
            );
            return model;
        }
    }
}
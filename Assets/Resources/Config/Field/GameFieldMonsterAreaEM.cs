using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameFieldMonsterAreaEM
    {
        [Header("坐标")]
        public Vector2 position;
        [Header("生成半径")]
        public float radius;
        [Header("怪物生成模型列表")]
        public GameFieldMonsterSpawnEM[] monsterSpawnEMs;
        [Header("生成时机(开局后n秒)")]
        public int spawnTime;

        public GameFieldMonsterAreaEM(in Vector2 position, float radius, GameFieldMonsterSpawnEM[] monsterSpawnEMs, int spawnTime)
        {
            this.position = position;
            this.radius = radius;
            this.monsterSpawnEMs = monsterSpawnEMs;
            this.spawnTime = spawnTime;
        }

        public GameFieldMonsterAreaModel ToModel()
        {
            GameFieldMonsterAreaModel model = new GameFieldMonsterAreaModel(
                position,
                radius,
                this.monsterSpawnEMs.Map((em) => em.ToModel()),
                spawnTime
            );
            return model;
        }
    }
}
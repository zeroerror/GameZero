using GamePlay.Bussiness.Logic;
using GamePlay.Core;
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

        public void OnEnable()
        {
            this.gameObject.SetActive(false);
        }

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

        // 绘制区域 2D的
        private void OnDrawGizmos()
        {
            // 绘制一个2d的圆
            var color = Color.yellow;
            Gizmos.color = Color.yellow;
            GameGizmosExtension.DrawCircle(transform.position, radius);
            Gizmos.color = color;
        }
    }
}
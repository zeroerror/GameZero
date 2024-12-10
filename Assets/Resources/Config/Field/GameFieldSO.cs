using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_field_", menuName = "游戏玩法/配置/场景模板")]
    public class GameFieldSO : GameSOBase
    {
        [Header("场景预制体")]
        public GameObject fieldPrefab;
        [Header("场景内怪物生成区域")]
        public GameFieldMonsterAreaEM[] monsterAreaEMs;

        protected override void OnValidate()
        {
            base.OnValidate();
            UpdateData();
        }

        public void UpdateData()
        {
            if (fieldPrefab)
            {
                var monsterAreaComs = fieldPrefab.GetComponentsInChildren<GameFieldMonsterAreaCom>();
                this.monsterAreaEMs = monsterAreaComs?.Map((com) => com.ToEM());
            }
        }

        public GameFieldModel ToModel()
        {
            var model = new GameFieldModel(
                typeId,
                monsterAreaEMs?.Map((em) => em.ToModel())
            );
            return model;
        }
    }
}
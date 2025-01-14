using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameSkillConditionEM
    {
        [Header("技能目标类型")]
        public GameSkillTargterType targeterType;
        [Header("冷却时间(s)")]
        public float cdTime;
        [Header("消耗MP")]
        public float mpCost;
        [Header("选择器")]
        public GameEntitySelectorEM selectorEM;

        public GameSkillConditionModel ToModel()
        {
            return new GameSkillConditionModel(
                targeterType,
                cdTime,
                mpCost,
                selectorEM.ToModel()
            );
        }
    }
}
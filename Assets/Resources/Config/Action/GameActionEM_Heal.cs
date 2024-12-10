using GamePlay.Bussiness.Logic;
using UnityEngine;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEM_Heal
    {
        [Header("治疗类型")]
        public GameActionHealType healType;

        [Header("治疗数值")]
        public int value;
        [Header("数值格式")]
        public GameActionValueFormat valueFormat;
        [Header("数值参考类型")]
        public GameActionValueRefType refType;
        [Header("数值参考属性")]
        public GameAttributeType refAttrType;

        public GameActionModel_Heal ToModel()
        {
            return new GameActionModel_Heal(
                this.healType,
                this.value,
                this.valueFormat,
                this.refType,
                this.refAttrType
            );
        }
    }
}
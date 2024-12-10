using GamePlay.Bussiness.Logic;
using UnityEngine;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEM_Dmg
    {
        [Header("伤害类型")]
        public GameActionDmgType dmgType;

        [Header("伤害数值")]
        public int value;
        [Header("数值格式")]
        public GameActionValueFormat valueFormat;
        [Header("数值参考类型")]
        public GameActionValueRefType refType;
        public GameEntitySelectorEM selectorEM;

        public GameActionModel_Dmg ToModel()
        {
            return new GameActionModel_Dmg(
                this.dmgType,
                this.value,
                this.valueFormat,
                this.refType
            );
        }
    }
}
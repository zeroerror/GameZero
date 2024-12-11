using GamePlay.Bussiness.Logic;
using UnityEngine;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEM_Heal
    {
        public int projectileId;
        public GameActionHealType healType;
        public int value;
        public GameActionValueFormat valueFormat;
        public GameActionValueRefType refType;
        public GameEntitySelectorEM selectorEM;

        public GameActionModel_Heal ToModel()
        {
            return new GameActionModel_Heal(
                this.projectileId,
                this.selectorEM?.ToSelector(),
                this.healType,
                this.value,
                this.valueFormat,
                this.refType
            );
        }
    }
}
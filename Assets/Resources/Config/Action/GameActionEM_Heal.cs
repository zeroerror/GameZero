using GamePlay.Bussiness.Logic;
using UnityEngine;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEM_Heal
    {
        public GameActionHealType healType;
        public int value;
        public GameActionValueFormat valueFormat;
        public GameActionValueRefType refType;

        public GameEntitySelectorEM selectorEM;
        public GameActionPreconditionSetEM preconditionSetEM;

        public GameActionModel_Heal ToModel()
        {
            return new GameActionModel_Heal(
                0,
                this.selectorEM?.ToSelector(),
                this.preconditionSetEM?.ToModel(),
                this.healType,
                this.value,
                this.valueFormat,
                this.refType
            );
        }
    }
}
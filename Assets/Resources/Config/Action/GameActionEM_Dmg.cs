using GamePlay.Bussiness.Logic;
using UnityEngine;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEM_Dmg
    {
        public int projectileId;
        public GameEntitySelectorEM selectorEM;

        public GameActionDmgType dmgType;
        public int value;
        public GameActionValueFormat valueFormat;
        public GameActionValueRefType refType;

        public GameActionModel_Dmg ToModel()
        {
            return new GameActionModel_Dmg(
                this.projectileId,
                this.selectorEM?.ToSelector(),
                this.dmgType,
                this.value,
                this.valueFormat,
                this.refType
            );
        }
    }
}
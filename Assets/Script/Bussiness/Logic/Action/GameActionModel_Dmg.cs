using System;

namespace GamePlay.Bussiness.Logic
{
    [System.Serializable]
    public class GameActionModel_Dmg : GameActionModelBase
    {
        public int dmg;

        public override string ToString()
        {
            return $"伤害值：{this.dmg}";
        }
    }
}
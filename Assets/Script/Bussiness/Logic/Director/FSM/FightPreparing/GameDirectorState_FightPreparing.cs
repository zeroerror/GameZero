using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorState_FightPreparing : GameDirectorStateBase
    {
        public List<GameActionOptionModel> options;
        public List<GameActionOptionModel> selectedOptionList = new List<GameActionOptionModel>();
        public bool confirmFight;
        public bool isAllUnitPositioned;
        public GameDirectorState_FightPreparing() { }

        public override void Clear()
        {
            base.Clear();
            this.options = null;
            this.selectedOptionList.Clear();
            this.confirmFight = false;
            this.isAllUnitPositioned = false;
        }
    }
}
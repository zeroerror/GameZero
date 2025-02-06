using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorState_FightPreparing : GameDirectorStateBase
    {
        public List<GameActionOptionModel> options;
        public GameActionOptionModel selectedOption;
        public bool preparingFinished;

        public GameDirectorState_FightPreparing() { }

        public override void Clear()
        {
            base.Clear();
            this.options = null;
            this.selectedOption = null;
            this.preparingFinished = false;
        }
    }
}
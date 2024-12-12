using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public class GameTransformContext
    {
        public List<GameTransfromPosAction> posActions { get; private set; }

        public GameTransformContext()
        {
            this.posActions = new List<GameTransfromPosAction>();
        }
    }
}
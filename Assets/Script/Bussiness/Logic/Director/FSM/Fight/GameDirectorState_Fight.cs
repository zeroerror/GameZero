using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorState_Fight : GameDirectorStateBase
    {
        /// <summary> 初始实体Id参数列表 </summary>
        public List<GameIdArgs> initEntityIdArgsList;

        public GameDirectorState_Fight() { }

        public override void Clear()
        {
            base.Clear();
            this.initEntityIdArgsList = null;
        }
    }
}
using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.Renderer
{
    public class GameDirectorState_FightR : GameDirectorStateBaseR
    {
        public GameEntityBase chooseEntity;
        public GameDirectorState_FightR() { }

        public override void Clear()
        {
            base.Clear();
            this.chooseEntity = null;
        }
    }
}
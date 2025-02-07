namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorState_Loading : GameDirectorStateBase
    {
        public int loadFieldId;
        public GameDirectorState_Loading() { }

        public override void Clear()
        {
            base.Clear();
            loadFieldId = 0;
        }
    }
}
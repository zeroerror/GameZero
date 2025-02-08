namespace GamePlay.Bussiness.Renderer
{
    public class GameDirectorState_LoadingR : GameDirectorStateBaseR
    {
        public int loadFieldId;
        public GameDirectorState_LoadingR()
        {
        }

        public override void Clear()
        {
            base.Clear();
            loadFieldId = 0;
        }
    }
}
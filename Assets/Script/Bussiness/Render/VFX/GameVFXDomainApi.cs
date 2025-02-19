namespace GamePlay.Bussiness.Render
{
    public interface GameVFXDomainApi
    {
        public GameVFXEntity Play(in GameVFXPlayArgs args);
        public void Stop(GameVFXEntity vfxEntity);
    }
}
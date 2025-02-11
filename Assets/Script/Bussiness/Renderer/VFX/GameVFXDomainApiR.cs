namespace GamePlay.Bussiness.Render
{
    public interface GameVFXDomainApiR
    {
        public GameVFXEntityR Play(in GameVFXPlayArgs args);
        public void Stop(GameVFXEntityR vfxEntity);
    }
}
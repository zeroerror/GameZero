namespace GamePlay.Bussiness.Logic
{
    public interface GameProjectileFSMDomainApi
    {
        public bool TryEnter(GameProjectileEntity role, GameProjectileStateType state);
    }
}
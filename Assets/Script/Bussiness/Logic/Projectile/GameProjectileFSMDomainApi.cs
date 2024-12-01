namespace GamePlay.Bussiness.Logic
{
    public interface GameProjectileFSMDomainApi
    {
        public bool TryEnter(GameProjectileEntity projectile, GameProjectileStateType state);

    }
}
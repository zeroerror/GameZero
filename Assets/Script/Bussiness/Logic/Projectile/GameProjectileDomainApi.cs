namespace GamePlay.Bussiness.Logic
{
    public interface GameProjectileDomainApi
    {
        public GameProjectileFSMDomainApi fsmApi { get; }
        public GameProjectileEntity CreateProjectile(int typeId, GameEntityBase creator, in GameTransformArgs transArgs);
    }
}
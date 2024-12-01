namespace GamePlay.Bussiness.Logic
{
    public interface GameProjectileDomainApi
    {
        public GameProjectileFSMDomainApi fsmApi { get; }
        public GameProjectileEntity CreateProjectile(int typeId, GameEntityBase creator, GameTransformArgs transArgs, in GameActionTargeterArgs targeter);
    }
}
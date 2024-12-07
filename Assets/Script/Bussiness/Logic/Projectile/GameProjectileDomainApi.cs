namespace GamePlay.Bussiness.Logic
{
    public interface GameProjectileDomainApi
    {
        /// <summary> 状态机API </summary>
        public GameProjectileFSMDomainApi fsmApi { get; }
        /// <summary> 创建投射物 </summary>
        public GameProjectileEntity CreateProjectile(int typeId, GameEntityBase creator, GameTransformArgs transArgs, in GameActionTargeterArgs targeter);
        /// <summary> 创建散射类型的投射物弹幕 </summary>
        public GameProjectileEntity[] CreateProjectileBarrage(int typeId, GameEntityBase creator, GameTransformArgs transArgs, GameActionTargeterArgs targeter, in GameProjectileBarrageModel_Spread barrageModel);
        /// <summary> 创建自定义发射偏移的投射物弹幕 </summary>
        public GameProjectileEntity[] CreateProjectileBarrage(int typeId, GameEntityBase creator, GameTransformArgs transArgs, in GameActionTargeterArgs targeter, in GameProjectileBarrageModel_CustomLaunchOffset barrageModel);
    }
}
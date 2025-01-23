namespace GamePlay.Bussiness.Logic
{
    public interface GameProjectileDomainApi
    {
        /// <summary> 状态机API </summary>
        public GameProjectileFSMDomainApi fsmApi { get; }

        /// <summary> 清空所有投射物 </summary>
        public void RemoveAllProjectiles();

        /// <summary> 创建投射物 </summary>
        public GameProjectileEntity CreateProjectile(int typeId, GameEntityBase creator, GameTransformArgs transArgs, in GameActionTargeterArgs targeter);

        /// <summary>
        /// 创建散射类型的投射物弹幕, 会根据目标方向对称散射, 不支持初始状态为锁定类型的弹体
        /// </summary>
        public GameProjectileEntity[] CreateBarrage(int typeId, GameEntityBase creator, in GameTransformArgs transArgs, GameActionTargeterArgs targeter, in GameProjectileBarrageModel_Spread barrageModel);

        /// <summary> 
        /// 创建自定义发射偏移的投射物弹幕, 会根据发射的锚点位置、目标方向, 在不同的起始位置发射弹体
        /// </summary>
        public GameProjectileEntity[] CreateBarrage(int typeId, GameEntityBase creator, GameTransformArgs transArgs, in GameActionTargeterArgs targeter, in GameProjectileBarrageModel_CustomLaunchOffset barrageModel);
    }
}
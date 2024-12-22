namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// 行为前置条件-目标身上的Buff条件
    /// </summary>
    public class GameActionPreconditionModel_Buff
    {
        public readonly int buffId;
        public readonly int layer;

        public GameActionPreconditionModel_Buff(int buffId, int layer)
        {
            this.buffId = buffId;
            this.layer = layer;
        }

        public bool CheckSatisfied(GameEntityBase target)
        {
            if (!(target is GameRoleEntity roleEntity)) return false;
            var buffCom = roleEntity.buffCom;
            if (!buffCom.TryGet(this.buffId, out var buff)) return false;
            if (buff.layer < this.layer) return false;
            return true;
        }
    }
}
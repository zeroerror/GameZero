using GamePlay.Bussiness.Logic;

namespace GamePlay.Config
{
    /// <summary>
    /// 行为前置条件-目标身上的Buff条件
    /// </summary>
    [System.Serializable]
    public class GameActionPreconditionEM_Buff
    {
        public bool enable;
        public GameBuffSO buffSO;
        public int layer;

        public GameActionPreconditionModel_Buff ToModel()
        {
            if (!enable) return null;
            if (!buffSO) return null;
            var model = new GameActionPreconditionModel_Buff(
                buffSO.typeId,
                layer
            );
            return model;
        }
    }
}
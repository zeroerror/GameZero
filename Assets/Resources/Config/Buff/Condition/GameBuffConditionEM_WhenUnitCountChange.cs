using GamePlay.Bussiness.Logic;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameBuffConditionEM_WhenUnitCountChange
    {
        public bool isEnable;
        public GameEntitySelectorEM selectorEM;
        public int countCondition;

        public GameBuffConditionModel_WhenUnitCountChange ToModel()
        {
            if (this.selectorEM == null) return null;
            return new GameBuffConditionModel_WhenUnitCountChange(
                this.selectorEM.ToModel(),
                this.countCondition
            );
        }
    }
}
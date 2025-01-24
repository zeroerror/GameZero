using GamePlay.Bussiness.Logic;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameBuffConditionSetEM
    {
        public GameBuffConditionEM_Duration durationEM;
        public GameBuffConditionEM_TimeInterval timeIntervalEM;
        public GameBuffConditionEM_WhenDoAction whenDoActionEM;
        public GameBuffConditionEM_WhenRoleStateEnter whenRoleStateEnterEM;
        public GameBuffConditionEM_WhenUnitCountChange whenUnitCountChangeEM;
        public GameBuffConditionEM_WhenAttributeChange whenAttributeChangeEM;

        public GameBuffConditionSetModel ToModel()
        {
            return new GameBuffConditionSetModel(
                this.durationEM != null && this.durationEM.isEnable ? this.durationEM.ToModel() : null,
                this.timeIntervalEM != null && this.timeIntervalEM.isEnable ? this.timeIntervalEM.ToModel() : null,
                this.whenDoActionEM != null && this.whenDoActionEM.isEnable ? this.whenDoActionEM.ToModel() : null,
                this.whenRoleStateEnterEM != null && this.whenRoleStateEnterEM.isEnable ? this.whenRoleStateEnterEM.ToModel() : null,
                this.whenUnitCountChangeEM != null && this.whenUnitCountChangeEM.isEnable ? this.whenUnitCountChangeEM.ToModel() : null,
                this.whenAttributeChangeEM != null && this.whenAttributeChangeEM.isEnable ? this.whenAttributeChangeEM.ToModel() : null
            );
        }
    }
}
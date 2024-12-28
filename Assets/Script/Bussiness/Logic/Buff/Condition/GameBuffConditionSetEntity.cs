namespace GamePlay.Bussiness.Logic
{
    public class GameBuffConditionSetEntity
    {
        public GameBuffConditionEntity_Duration durationEntity { get; private set; }
        public GameBuffConditionEntity_TimeInterval timeIntervalEntity { get; private set; }
        public GameBuffConditionEntity_WhenDoAction whenDoActionEntity { get; private set; }

        public GameBuffConditionSetEntity(GameBuffConditionSetModel model)
        {
            if (model.durationModel != null) this.durationEntity = new GameBuffConditionEntity_Duration(model.durationModel);
            if (model.timeIntervalModel != null) this.timeIntervalEntity = new GameBuffConditionEntity_TimeInterval(model.timeIntervalModel);
            if (model.whenDoActionModel != null) this.whenDoActionEntity = new GameBuffConditionEntity_WhenDoAction(model.whenDoActionModel);
        }

        public void Inject(
            GameBuffConditionEntityBase.ForEachActionRecordDelegate_Dmg forEachActionRecord_Dmg,
            GameBuffConditionEntityBase.ForEachActionRecordDelegate_Heal forEachActionRecord_Heal,
            GameBuffConditionEntityBase.ForEachActionRecordDelegate_LaunchProjectile forEachActionRecord_LaunchProjectile)
        {
            if (!!this.durationEntity) this.durationEntity.ForEachActionRecord_Dmg = forEachActionRecord_Dmg;
            if (!!this.timeIntervalEntity) this.timeIntervalEntity.ForEachActionRecord_Dmg = forEachActionRecord_Dmg;
            if (!!this.whenDoActionEntity) this.whenDoActionEntity.ForEachActionRecord_Dmg = forEachActionRecord_Dmg;

            if (!!this.durationEntity) this.durationEntity.ForEachActionRecord_Heal = forEachActionRecord_Heal;
            if (!!this.timeIntervalEntity) this.timeIntervalEntity.ForEachActionRecord_Heal = forEachActionRecord_Heal;
            if (!!this.whenDoActionEntity) this.whenDoActionEntity.ForEachActionRecord_Heal = forEachActionRecord_Heal;

            if (!!this.durationEntity) this.durationEntity.ForEachActionRecord_LaunchProjectile = forEachActionRecord_LaunchProjectile;
            if (!!this.timeIntervalEntity) this.timeIntervalEntity.ForEachActionRecord_LaunchProjectile = forEachActionRecord_LaunchProjectile;
            if (!!this.whenDoActionEntity) this.whenDoActionEntity.ForEachActionRecord_LaunchProjectile = forEachActionRecord_LaunchProjectile;
        }

        public void Clear()
        {
            if (!!this.durationEntity) this.durationEntity.Clear();
            if (!!this.timeIntervalEntity) this.timeIntervalEntity.Clear();
            if (!!this.whenDoActionEntity) this.whenDoActionEntity.Clear();
        }

        public void Tick(float dt)
        {
            if (!!this.durationEntity) this.durationEntity.Tick(dt);
            if (!!this.timeIntervalEntity) this.timeIntervalEntity.Tick(dt);
            if (!!this.whenDoActionEntity) this.whenDoActionEntity.Tick(dt);
        }

        public bool CheckSatisfied()
        {
            var isSatisfied = true;
            if (!!this.durationEntity && !this.durationEntity.isSatisfied) isSatisfied = false;
            if (!!this.timeIntervalEntity && !this.timeIntervalEntity.isSatisfied) isSatisfied = false;
            if (!!this.whenDoActionEntity && !this.whenDoActionEntity.isSatisfied) isSatisfied = false;
            return isSatisfied;
        }

        public void RefreshTime()
        {
            if (!this.durationEntity) return;
            this.durationEntity.Clear();
        }

        public void StackTime()
        {
            if (!this.durationEntity) return;
            this.durationEntity.extraStackCount += 1;
        }
    }
}
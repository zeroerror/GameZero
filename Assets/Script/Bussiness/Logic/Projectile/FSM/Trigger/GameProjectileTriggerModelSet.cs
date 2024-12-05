namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileTriggerModelSet
    {
        public readonly GameProjectileTriggerModel_Duration durationTriggerModel;
        public readonly GameProjectileTriggerModel_VolumeCollision volumeCollisionTriggerModel;
        public readonly GameProjectileTriggerModel_ImpactTarget impactTargetTriggerModel;

        public GameProjectileTriggerModelSet(GameProjectileTriggerModel_Duration durationTriggerModel, GameProjectileTriggerModel_VolumeCollision volumeCollisionTriggerModel, GameProjectileTriggerModel_ImpactTarget impactTargetTriggerModel)
        {
            this.durationTriggerModel = durationTriggerModel;
            this.volumeCollisionTriggerModel = volumeCollisionTriggerModel;
            this.impactTargetTriggerModel = impactTargetTriggerModel;
        }
    }
}
namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateTriggerModelSet
    {
        public readonly GameProjectileStateTriggerModel_Duration durationTriggerModel;
        public readonly GameProjectileStateTriggerModel_VolumeCollision volumeCollisionTriggerModel;
        public readonly GameProjectileStateTriggerModel_ImpactTarget impactTargetTriggerModel;

        public GameProjectileStateTriggerModelSet(GameProjectileStateTriggerModel_Duration durationTriggerModel, GameProjectileStateTriggerModel_VolumeCollision volumeCollisionTriggerModel, GameProjectileStateTriggerModel_ImpactTarget impactTargetTriggerModel)
        {
            this.durationTriggerModel = durationTriggerModel;
            this.volumeCollisionTriggerModel = volumeCollisionTriggerModel;
            this.impactTargetTriggerModel = impactTargetTriggerModel;
        }
    }
}
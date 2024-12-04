namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateTriggerSetEntity
    {
        public GameProjectileStateTriggerEntity_Duration durationTriggerEntity;
        public GameProjectileStateTriggerEntity_VolumeCollision volumeCollisionTriggerEntity;
        public GameProjectileStateTriggerEntity_ImpactTarget impactTargetTriggerEntity;

        public GameProjectileStateTriggerSetEntity(
            GameProjectileStateTriggerEntity_Duration durationTriggerEntity,
            GameProjectileStateTriggerEntity_VolumeCollision volumeCollisionTriggerEntity,
            GameProjectileStateTriggerEntity_ImpactTarget impactTargetTriggerEntity
        )
        {
            this.durationTriggerEntity = durationTriggerEntity;
            this.volumeCollisionTriggerEntity = volumeCollisionTriggerEntity;
            this.impactTargetTriggerEntity = impactTargetTriggerEntity;
        }

        public void Clear()
        {
            this.durationTriggerEntity?.Clear();
            this.volumeCollisionTriggerEntity?.Clear();
            this.impactTargetTriggerEntity?.Clear();
        }
    }
}
namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileTriggerSetEntity
    {
        public GameProjectileTriggerEntity_Duration durationTriggerEntity;
        public GameProjectileTriggerEntity_VolumeCollision volumeCollisionTriggerEntity;
        public GameProjectileTriggerEntity_ImpactTarget impactTargetTriggerEntity;

        public GameProjectileTriggerSetEntity(
            GameProjectileTriggerEntity_Duration durationTriggerEntity,
            GameProjectileTriggerEntity_VolumeCollision volumeCollisionTriggerEntity,
            GameProjectileTriggerEntity_ImpactTarget impactTargetTriggerEntity
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
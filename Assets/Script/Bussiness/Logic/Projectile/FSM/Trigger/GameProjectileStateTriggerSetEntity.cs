namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateTriggerSetEntity
    {
        public GameProjectileStateTriggerEntity_Duration durationTriggerEntity;
        public GameProjectileStateTriggerEntity_VolumeCollision volumeCollisionTriggerEntity;

        public GameProjectileStateTriggerSetEntity(
            GameProjectileStateTriggerEntity_Duration durationTriggerEntity,
            GameProjectileStateTriggerEntity_VolumeCollision volumeCollisionTriggerEntity
        )
        {
            this.durationTriggerEntity = durationTriggerEntity;
            this.volumeCollisionTriggerEntity = volumeCollisionTriggerEntity;
        }

        public void Clear()
        {
            this.durationTriggerEntity.Clear();
            this.volumeCollisionTriggerEntity.Clear();
        }
    }
}
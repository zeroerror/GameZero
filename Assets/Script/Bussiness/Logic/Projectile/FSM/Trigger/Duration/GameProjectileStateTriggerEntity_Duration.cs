namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateTriggerEntity_Duration
    {
        public readonly GameProjectileStateTriggerModel_Duration model;

        public float elapsedTime;

        public GameProjectileStateTriggerEntity_Duration(GameProjectileStateTriggerModel_Duration model)
        {
            this.model = model;
        }

        public void Clear()
        {
            this.elapsedTime = 0;
        }
    }
}
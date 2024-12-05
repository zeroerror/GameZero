namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileTriggerEntity_Duration
    {
        public readonly GameProjectileTriggerModel_Duration model;

        public float elapsedTime;

        public GameProjectileTriggerEntity_Duration(GameProjectileTriggerModel_Duration model)
        {
            this.model = model;
        }

        public void Clear()
        {
            this.elapsedTime = 0;
        }
    }
}
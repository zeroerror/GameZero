namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateTriggerDomain_Duration
    {
        private GameContext _context;

        public GameProjectileStateTriggerDomain_Duration()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public bool CheckSatisfied(GameProjectileEntity entity, GameProjectileStateTriggerEntity_Duration triggerEntity, float dt)
        {
            triggerEntity.elapsedTime += dt;
            return triggerEntity.elapsedTime >= triggerEntity.model.duration;
        }
    }
}
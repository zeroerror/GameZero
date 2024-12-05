namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileTriggerDomain_Duration
    {
        private GameContext _context;

        public GameProjectileTriggerDomain_Duration()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public bool CheckSatisfied(GameProjectileEntity entity, GameProjectileTriggerEntity_Duration triggerEntity, float dt)
        {
            triggerEntity.elapsedTime += dt;
            return triggerEntity.elapsedTime >= triggerEntity.model.duration;
        }
    }
}
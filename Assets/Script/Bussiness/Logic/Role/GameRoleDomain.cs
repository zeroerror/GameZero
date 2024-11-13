namespace GamePlay.Bussiness.Logic
{
    public class GameRoleDomain : GameEntityDomainBase<GameRoleEntity>
    {
        public override GameEntityContextBase context => this._context;
        GameRoleContext _context;

        public override void Collect()
        {
        }

        public override GameRoleEntity Create()
        {
            return new GameRoleEntity();
        }

        public override void Tick(float dt)
        {
        }
    }
}

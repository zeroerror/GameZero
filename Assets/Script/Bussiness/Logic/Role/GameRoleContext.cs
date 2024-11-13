namespace GamePlay.Bussiness.Logic
{
    public class GameRoleContext : GameEntityContextBase
    {
        public override GameEntityRepoBase repo => this._repo;
        GameRoleEntityRepo _repo;
    }
}
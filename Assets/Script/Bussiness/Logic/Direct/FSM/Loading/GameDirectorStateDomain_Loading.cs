namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorStateDomain_Loading : GameDirectorStateDomainBase
    {
        public GameDirectorStateDomain_Loading(GameDirectorDomain directorDomain) : base(directorDomain)
        {
        }

        public void Destory()
        {
        }

        public void Tick(float dt)
        {
        }

        public override bool CheckEnter(GameDirectorEntity director)
        {
            return true;
        }

        public override void Enter(GameDirectorEntity director)
        {
            throw new System.NotImplementedException();
        }

        protected override void _Tick(GameDirectorEntity director, float frameTime)
        {
            throw new System.NotImplementedException();
        }

        protected override GameDirectorStateType _CheckExit(GameDirectorEntity director)
        {
            throw new System.NotImplementedException();
        }
    }
}
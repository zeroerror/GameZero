namespace GamePlay.Bussiness.Logic
{
    public interface GameDirectorFSMDomainApi
    {
        public bool TryEnter(GameDirectorEntity director, GameDirectorStateType state);
    }
}
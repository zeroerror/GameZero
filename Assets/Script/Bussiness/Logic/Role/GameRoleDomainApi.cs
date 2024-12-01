namespace GamePlay.Bussiness.Logic
{
    public interface GameRoleDomainApi
    {
        public GameRoleEntity CreateRole(int typeId, int campId, in GameTransformArgs transArgs, bool isUser);
        public GameRoleEntity CreatePlayerRole(int typeId, int campId, in GameTransformArgs transArgs, bool isUser);

        public GameRoleFSMDomainApi fsmApi { get; }
    }
}

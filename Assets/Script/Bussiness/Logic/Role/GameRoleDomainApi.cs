namespace GamePlay.Bussiness.Logic
{
    public interface GameRoleDomainApi
    {
        public GameRoleEntity Create(int typeId, int campId, in GameTransformArgs transArgs, bool isUser = false);
        public GameRoleEntity CreateUserRole(int typeId, int campId, in GameTransformArgs transArgs);
    }
}

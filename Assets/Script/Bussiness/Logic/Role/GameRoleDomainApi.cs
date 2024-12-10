namespace GamePlay.Bussiness.Logic
{
    public interface GameRoleDomainApi
    {
        public GameRoleFSMDomainApi fsmApi { get; }
        public GameRoleAIDomainApi apApi { get; }

        public GameRoleEntity CreateRole(int typeId, int campId, in GameTransformArgs transArgs, bool isUser);
        public GameRoleEntity CreatePlayerRole(int typeId, in GameTransformArgs transArgs, bool isUser);
        public GameRoleEntity CreateMonsterRole(int typeId, in GameTransformArgs transArgs);
        public GameRoleEntity GetNearestEnemy(GameEntityBase entity);
    }
}

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleFactory
    {
        public GameRoleFactory() { }

        public GameRoleEntity Load(int typeId)
        {
            // todo template
            var e = new GameRoleEntity(typeId);
            e.attributeCom.SetAttribute(GameAttributeType.HP, 100);
            e.attributeCom.SetAttribute(GameAttributeType.MoveSpeed, 5);
            return e;
        }
    }
}
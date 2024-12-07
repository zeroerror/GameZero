namespace GamePlay.Bussiness.Logic
{
    public class GameRoleModel
    {
        public readonly int typeId;
        public readonly int[] skillIds;
        public readonly GameAttribute[] attributes;

        public GameRoleModel(int typeId, int[] skillIds, GameAttribute[] attributes)
        {
            this.typeId = typeId;
            this.skillIds = skillIds;
            this.attributes = attributes;
        }
    }
}
namespace GamePlay.Bussiness.Logic
{
    public class GameActionRecord
    {
        public int actionId;
        public GameIdArgs actorIdArgs;
        public GameIdArgs targetIdArgs;

        public GameAttributeArgs actorAttrEffect;
        public GameAttributeArgs targetAttrEffect;

        public GameActionRecord()
        {
        }
    }
}
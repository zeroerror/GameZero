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

        public override string ToString()
        {
            return $"行为Id:{actionId}\n执行者:{actorIdArgs}\n目标:{targetIdArgs}";
        }
    }
}
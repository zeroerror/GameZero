namespace GamePlay.Bussiness.Logic
{
    public abstract class GameBuffConditionEntityBase
    {
        public bool isSatisfied { get; private set; }

        public void Tick(float dt)
        {
            _Tick(dt);
            isSatisfied = _Check();
        }

        protected abstract void _Tick(float dt);
        protected abstract bool _Check();
        public abstract void Clear();

        public static bool operator !(GameBuffConditionEntityBase condition)
        {
            return condition == null;
        }

        public static bool operator true(GameBuffConditionEntityBase condition)
        {
            return condition != null;
        }

        public static bool operator false(GameBuffConditionEntityBase condition)
        {
            return condition == null;
        }
    }
}
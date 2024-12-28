using System;

namespace GamePlay.Bussiness.Logic
{
    public abstract class GameBuffConditionEntityBase
    {
        public bool isSatisfied { get; private set; }

        public delegate void ForEachActionRecordDelegate_Dmg(in Action<GameActionRecord_Dmg> actionRecord);
        public ForEachActionRecordDelegate_Dmg ForEachActionRecord_Dmg;

        public delegate void ForEachActionRecordDelegate_Heal(in Action<GameActionRecord_Heal> actionRecord);
        public ForEachActionRecordDelegate_Heal ForEachActionRecord_Heal;

        public delegate void ForEachActionRecordDelegate_LaunchProjectile(in Action<GameActionRecord_LaunchProjectile> actionRecord);
        public ForEachActionRecordDelegate_LaunchProjectile ForEachActionRecord_LaunchProjectile;

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
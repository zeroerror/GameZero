namespace GamePlay.Core
{
    public abstract class GameStateBase
    {
        public string stateName;

        public GameStateBase(string name)
        {
            stateName = name;
        }

        public override string ToString()
        {
            return stateName;
        }

        public abstract void Enter();
        public abstract void Tick(float dt);
        public abstract void Exit();
    }
}

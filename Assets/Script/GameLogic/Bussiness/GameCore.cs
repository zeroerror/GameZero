public class GameCore
{
    public GameDirectDomain directDomain { get; private set; } = new GameDirectDomain();

    public void Update(float dt)
    {
        this.directDomain.Update(dt);
    }

    public void LateUpdate(float dt)
    {
        this.directDomain.LateUpdate(dt);
    }
}
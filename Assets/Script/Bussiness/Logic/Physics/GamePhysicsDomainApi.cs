using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public interface GamePhysicsDomainApi
    {
        public void CreatePhysics(GameEntityBase entity, GameColliderModelBase colliderModel);
        public void RemovePhysics(GameEntityBase entity);
    }
}

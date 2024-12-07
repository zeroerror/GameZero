using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.Logic
{
    public class GameFieldEntity : GameEntityBase
    {
        public GameFieldModel model { get; private set; }

        public GameFieldEntity(GameFieldModel model) : base(model.typeId, GameEntityType.Field)
        {
            this.model = model;
        }

        public override void Tick(float dt)
        {
        }

        public override void Destroy()
        {
            throw new System.NotImplementedException();
        }
    }
}
namespace GamePlay.Bussiness.Logic
{
    public class GameActionOptionEntity : GameEntityBase
    {
        public GameActionOptionModel model { get; private set; }
        public int lv { get; private set; }

        public GameBuffCom buffCom { get; private set; }

        public GameActionOptionEntity(GameActionOptionModel model) : base(model.typeId, GameEntityType.None)
        {
            this.model = model;
            this.buffCom = new GameBuffCom();
        }

        public override void Tick(float dt)
        {
        }

        public override void Destroy()
        {
        }
    }
}
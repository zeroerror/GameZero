using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameFieldEntityR : GameEntityBase
    {
        public GameFieldModelR model { get; private set; }

        public GameObject go { get; private set; }
        public GameObject entityLayer { get; private set; }
        public GameObject environmentLayer { get; private set; }
        public GameObject groundLayer { get; private set; }

        public GameFieldEntityR(GameFieldModelR model, GameObject rootGO, GameObject entityLayer, GameObject environmentLayer, GameObject groundLayer) : base(model.typeId, GameEntityType.Field)
        {
            this.model = model;
            this.go = rootGO;
            this.entityLayer = entityLayer;
            this.environmentLayer = environmentLayer;
            this.groundLayer = groundLayer;
        }

        public override void Tick(float dt)
        {
        }

        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
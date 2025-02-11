using System.Collections.Generic;
using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public class GameFieldEntityR : GameEntityBase
    {
        public GameFieldModelR model { get; private set; }

        public GameObject go { get; private set; }
        public Dictionary<GameFieldLayerType, GameObject> layers { get; private set; }

        public GameFieldEntityR(GameFieldModelR model, GameObject go, Dictionary<GameFieldLayerType, GameObject> layers)
        : base(model.typeId, GameEntityType.Field)
        {
            this.model = model;
            this.go = go;
            this.layers = layers;
        }

        public override void Tick(float dt)
        {
        }

        public override void Destroy()
        {
        }

        public GameObject GetLayer(GameFieldLayerType layerType)
        {
            return this.layers[layerType];
        }
    }
}
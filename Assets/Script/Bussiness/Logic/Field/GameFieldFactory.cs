using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Logic
{
    public class GameFieldFactory
    {
        public GameFieldTemplate template { get; private set; }
        public GameFieldFactory()
        {
            template = new GameFieldTemplate();
        }
        public GameFieldEntity Load(int typeId)
        {
            if (!template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("GameFieldFactory.Load: typeId not found: " + typeId);
                return null;
            }
            var entity = new GameFieldEntity(model);
            return entity;
        }
    }
}
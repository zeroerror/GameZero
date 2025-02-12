using GamePlay.Bussiness.Logic;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEM_Stealth
    {
        public GameEntitySelectorEM selectorEM;
        public GameActionPreconditionSetEM preconditionSetEM;
        public GameVec2 randomValueOffset;

        public float duration;

        public GameActionModel_Stealth ToModel()
        {
            var model = new GameActionModel_Stealth(
                0,
                this.selectorEM.ToModel(),
                this.preconditionSetEM?.ToModel(),
                this.randomValueOffset,
                this.duration
            );
            return model;
        }
    }
}
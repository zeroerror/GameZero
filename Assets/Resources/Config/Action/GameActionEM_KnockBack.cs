using GamePlay.Bussiness.Logic;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEM_KnockBack
    {
        public GameActionKnockBackDirType knockBackDirType;
        public float distance;
        public float duration;
        public GameEasingType easingType;

        public GameEntitySelectorEM selectorEM;
        public GameActionPreconditionSetEM preconditionSetEM;
        public GameVec2 randomValueOffset;

        public GameActionModel_KnockBack ToModel()
        {
            var model = new GameActionModel_KnockBack(
                0,
                this.selectorEM.ToModel(),
                this.preconditionSetEM?.ToModel(),
                this.randomValueOffset,
                this.knockBackDirType,
                this.distance,
                this.duration,
                this.easingType
            );
            return model;
        }
    }
}
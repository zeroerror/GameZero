using GamePlay.Bussiness.Logic;
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

        public GameActionModel_KnockBack ToModel()
        {
            var model = new GameActionModel_KnockBack(
                0,
                selectorEM.ToSelector(),
                knockBackDirType,
                distance,
                duration,
                easingType
            );
            return model;
        }
    }
}
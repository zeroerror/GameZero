using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEM_CharacterTransform
    {
        public GameEntitySelectorEM selectorEM;
        public GameActionPreconditionSetEM preconditionSetEM;
        public GameVec2 randomValueOffset;

        public GameRoleSO roleSO;
        public GameActionModel_CharacterTransformAttribute[] attributeList;

        public GameActionModel_CharacterTransform ToModel()
        {
            var model = new GameActionModel_CharacterTransform(
                0,
                this.selectorEM.ToSelector(),
                this.preconditionSetEM?.ToModel(),
                this.randomValueOffset,
                this.roleSO?.typeId ?? 0,
                this.attributeList
            );
            return model;
        }
    }
}
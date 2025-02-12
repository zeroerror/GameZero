using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameActionModel_Stealth : GameActionModelBase
    {
        /// <summary> 持续时间 </summary>
        public float duraiton;

        public GameActionModel_Stealth(
            int typeId,
            GameEntitySelector selector,
            GameActionPreconditionSetModel preconditionSet,
            in GameVec2 randomValueOffset,
            float duraiton
        ) : base(GameActionType.Stealth, typeId, selector, preconditionSet, randomValueOffset)
        {
            this.duraiton = duraiton;
        }

        public override string ToString()
        {
            return $"隐身持续时间:{duraiton}";
        }

        public override GameActionModelBase GetCustomModel(float customParam)
        {
            var duraiton = customParam * this.duraiton;
            return new GameActionModel_Stealth(
                typeId,
                selector,
                preconditionSet,
                randomValueOffset,
                duraiton
            );
        }
    }
}
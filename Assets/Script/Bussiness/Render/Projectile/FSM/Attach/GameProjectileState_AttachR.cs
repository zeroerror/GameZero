using GamePlay.Bussiness.Logic;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Render
{
    public class GameProjectileState_Attach : GameProjectileStateBase
    {
        public GameEntityBase attachEntity;

        public GameProjectileState_Attach() { }

        public override void Clear()
        {
            base.Clear();
            this.attachEntity = null;
        }
    }
}
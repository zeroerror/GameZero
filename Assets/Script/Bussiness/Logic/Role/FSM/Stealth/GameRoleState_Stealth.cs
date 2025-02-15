using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleState_Stealth : GameRoleStateBase
    {
        public float duration;
        public bool isMoving
        {
            get => this._isMoving;
            set
            {
                if (this._isMoving == value) return;
                this._isMoving = value;
                this.isMovingDirty = true;
            }
        }
        private bool _isMoving;
        public bool isMovingDirty;

        public GameRoleState_Stealth() { }

        public override void Clear()
        {
            base.Clear();
            this.duration = 0;
            this.isMoving = false;
            this.isMovingDirty = false;
        }
    }
}
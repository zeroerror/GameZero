namespace GamePlay.Bussiness.Logic
{
    public class GameRoleAIState_Follow : GameRoleAIStateBase
    {
        public readonly GameRoleEntity role;

        public GameEntityBase followEntity;

        /// <summary> 远离距离 </summary>
        public float farWayDis
        {
            get => _farWayDis;
            set
            {
                _farWayDis = value;
                _farWayDis_Sqr = value * value;
            }
        }
        private float _farWayDis;
        private float _farWayDis_Sqr;

        /// <summary> 靠近距离 </summary>
        public float nearByDis
        {
            get => _nearByDis;
            set
            {
                _nearByDis = value;
                _nearByDis_Sqr = value * value;
            }
        }
        private float _nearByDis;
        private float _nearByDis_Sqr;


        public GameRoleAIState_Follow(GameRoleEntity role)
        {
            this.role = role;

            this.farWayDis = 10f;
            this.nearByDis = 4f;
        }

        public override void Clear()
        {
            base.Clear();
            this.farAwayDirty = false;
        }

        /// <summary> 是否远离了跟随目标 </summary>
        public bool isFarAway()
        {
            if (this.followEntity == null)
            {
                return false;
            }
            var offset = this.role.logicBottomPos - this.followEntity.logicBottomPos;
            var curSqrDis = offset.sqrMagnitude;
            return curSqrDis > this._farWayDis_Sqr;
        }
        public bool farAwayDirty;

        public bool isNearBy()
        {
            if (this.followEntity == null) return true;
            var offset = this.role.logicBottomPos - this.followEntity.logicBottomPos;
            var curSqrDis = offset.sqrMagnitude;
            return curSqrDis < this._nearByDis_Sqr;
        }
    }
}
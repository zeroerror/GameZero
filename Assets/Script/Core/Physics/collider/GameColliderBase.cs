using GameVec2 = UnityEngine.Vector2;
using GamePlay.Bussiness.Logic;

namespace GamePlay.Core
{
    public abstract class GameColliderBase
    {
        public int id;
        public GameEntityBase binder;
        public bool isTrigger = true;
        public GameVec2 worldPos;
        public GameVec2 worldCenterPos;
        public GameVec2 originCenterPos;
        public GameVec2 originCenterPos_rotated;
        public GameVec2 colliderOffset;
        public float angle;
        public float scale { get; protected set; } = 1;
        public GameVec2 localAxisX = GameVec2.right;
        public GameVec2 localAxisY = GameVec2.up;
        public bool lockPosition;
        public bool lockRotation;
        public bool lockScale;

        public GameColliderBase(GameEntityBase binder, GameColliderModelBase param, int id)
        {
            this.binder = binder;
            this.id = id;
            SetByModel(param);
        }

        public void SetByModel(GameColliderModelBase colliderModel)
        {
            _SetByModel(colliderModel);
            _SetWorldAngle(colliderModel.angle);
            _SetWorldScale(this.scale);
        }

        protected abstract void _SetByModel(GameColliderModelBase colliderModel);

        public void UpdateTRS(GameTransformCom trans)
        {
            if (!trans.position.Equals(worldPos))
                SetWorldPosition(trans.position);
            if (trans.angle != angle)
                SetWorldAngle(trans.angle);
            if (trans.scale != scale)
                SetWorldScale(trans.scale);
        }

        public void UpdateTRS()
        {
            var trans = this.binder.transformCom;
            if (!trans.position.Equals(worldPos))
                SetWorldPosition(trans.position);
            if (trans.angle != angle)
                SetWorldAngle(trans.angle);
            if (trans.scale != scale)
                SetWorldScale(trans.scale);
        }

        public void SetWorldPosition(GameVec2 pos)
        {
            if (pos == null) return;
            if (lockPosition) return;
            if (worldPos.Equals(pos)) return;
            _SetWorldPosition(pos);
        }

        protected abstract void _SetWorldPosition(GameVec2 pos);

        public void SetWorldAngle(float angle)
        {
            if (lockRotation) return;
            if (this.angle == angle) return;
            angle = angle == 0 ? 0 : angle;
            _SetWorldAngle(angle);
        }

        protected abstract void _SetWorldAngle(float angle);

        public void SetWorldScale(float scale)
        {
            if (lockScale) return;
            if (this.scale == scale) return;
            scale = scale == 0 ? 0 : scale;
            _SetWorldScale(scale);
        }

        protected abstract void _SetWorldScale(float scale);
        public abstract GameVec2 GetResolvingMTV(GameColliderBase colliderB, bool onlyDetectPenetration = true);
        public abstract GameVec2 GetResolvingMTV(in GameVec2 point, bool onlyDetectPenetration = true);
        public abstract GameVec2 GetProjectionOnAxis(in GameVec2 origin, in GameVec2 axis);
        public abstract bool CheckOverlap(in GameVec2 point);
    }
}

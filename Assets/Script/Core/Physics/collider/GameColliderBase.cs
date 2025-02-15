using GameVec2 = UnityEngine.Vector2;
using GamePlay.Bussiness.Logic;

namespace GamePlay.Core
{
    public abstract class GameColliderBase
    {
        public bool isEnable;
        public int id;
        public bool isTrigger = true;
        public GameVec2 worldPos;
        public GameVec2 worldCenterPos;
        public GameVec2 originCenterPos;
        public GameVec2 originCenterPos_rotated;
        public GameVec2 colliderOffset;
        public float angle;
        public GameVec2 scale { get; protected set; } = GameVec2.one;
        public GameVec2 localAxisX = GameVec2.right;
        public GameVec2 localAxisY = GameVec2.up;
        public bool lockPosition;
        public bool lockRotation;
        public bool lockScale;

        public GameEntityBase binder;
        public GameColliderTag tag;

        public GameColliderModelBase colliderModel { get; private set; }

        public GameColliderBase(GameEntityBase binder, GameColliderModelBase param, int id)
        {
            this.binder = binder;
            this.id = id;
            SetByModel(param);
        }

        public void SetByModel(GameColliderModelBase colliderModel)
        {
            if (colliderModel == null) return;
            _SetByModel(colliderModel);
            _SetWorldAngle(colliderModel.getangle);
            _SetWorldScale(this.scale);
            this.colliderModel = colliderModel;
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

        public void UpdateTRS(in GameTransformArgs transArgs)
        {
            if (!transArgs.position.Equals(worldPos))
                SetWorldPosition(transArgs.position);
            if (transArgs.angle != angle)
                SetWorldAngle(transArgs.angle);
            if (transArgs.scale != scale)
                SetWorldScale(transArgs.scale);
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

        public void SetWorldScale(in GameVec2 scale)
        {
            if (lockScale) return;
            if (this.scale == scale) return;
            _SetWorldScale(scale);
        }

        protected abstract void _SetWorldScale(in GameVec2 scale);
        public abstract GameVec2 GetResolvingMTV(GameColliderBase colliderB, bool onlyDetectPenetration = true);
        public abstract GameVec2 GetResolvingMTV(in GameVec2 point, bool onlyDetectPenetration = true);
        public abstract GameVec2 GetProjectionOnAxis(in GameVec2 origin, in GameVec2 axis);
        public abstract bool CheckOverlap(in GameVec2 point);
    }
}

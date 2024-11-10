using System;
using GameVec2 = UnityEngine.Vector2;
using Game.Bussiness;

namespace Game.Core
{
    public abstract class GameCollider
    {
        public int id;
        public GameEntity binder;
        public GameTransformComponent transform;
        public bool isTrigger = true;
        public GameVec2 worldPos;
        public GameVec2 worldCenterPos;
        public GameVec2 originCenterPos;
        public GameVec2 originCenterPos_rotated;
        public GameVec2 colliderOffset;
        public float angle;
        public float scale { get; protected set; }
        public GameVec2 localAxisX = GameVec2.right;
        public GameVec2 localAxisY = GameVec2.up;
        public bool lockPosition;
        public bool lockRotation;
        public bool lockScale;

        // Simplified constructor
        public GameCollider(GameEntity binder, object param, int id, float scale = 1)
        {
            this.binder = binder;
            this.id = id;
            SetByModel(param, scale);
        }

        public void SetByModel(object colliderModel, float scale = 1)
        {
            _SetByModel(colliderModel, scale);
            _SetWorldAngle(colliderModel?.GetType().GetProperty("angle")?.GetValue(colliderModel) as float? ?? 0);
            _SetWorldScale(scale);
        }

        protected abstract void _SetByModel(object colliderModel, float scale);

        public void UpdateTRS(GameTransformComponent trans)
        {
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

        public abstract GameVec2 GetRestoreMTV(GameCollider colliderB, bool onlyIntersect = true);

        public abstract GameVec2 GetContactMTV_ByPoint(GameVec2 point);

        public abstract GameVec2 GetContactMTV(GameCollider collider);

        public abstract bool IsIntersectWithPoint(GameVec2 point);

        public abstract GameVec2 GetProjectionOnAxis(GameVec2 origin, GameVec2 axis);
    }
}

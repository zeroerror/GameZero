using System;
using GamePlay.Logic.Bussiness;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Logic.Core
{
    public class GameCircleCollider : GameCollider
    {

        public float originRadius => _originRadius;
        float _originRadius;

        public float worldRadius => _originRadius * Math.Abs(this.scale);

        public GameCircleCollider(
            GameEntity binder,
            GameColliderCircleModel colliderModel,
            int id,
            float scale = 1
        ) : base(binder, colliderModel, id, scale)
        {
        }

        protected override void _SetByModel(object model, float scale)
        {
            var colliderModel = model as GameColliderCircleModel;
            this.colliderOffset = colliderModel.offset;
            this._originRadius = colliderModel.radius;
            this.worldPos = GameVec2.zero;
            this.originCenterPos = colliderModel.offset;
            this.originCenterPos_rotated = colliderModel.offset;
        }

        protected override void _SetWorldPosition(GameVec2 pos)
        {
            var offset = pos - this.worldPos;
            this.worldPos += offset;
            this.worldCenterPos += offset;
        }

        protected override void _SetWorldAngle(float angle)
        {
            this.angle = angle;
            var pos = this.worldPos;
            var scale = this.scale;
            this.originCenterPos_rotated = GameVectorUtil.RotateOnAxisZ(this.originCenterPos, angle);
            this.worldCenterPos = this.originCenterPos_rotated * scale + pos;
            this.localAxisX = GameVectorUtil.RotateOnAxisZ(GameVec2.right, angle);
            this.localAxisY = GameVectorUtil.RotateOnAxisZ(GameVec2.up, angle);
        }

        protected override void _SetWorldScale(float scale)
        {
            this.scale = scale;
            var pos = this.worldPos;
            this.worldCenterPos = this.originCenterPos_rotated * scale + pos;
        }

        public override GameVec2 GetProjectionOnAxis(GameVec2 origin, GameVec2 axis)
        {
            var originToCenter = this.worldCenterPos - origin;
            var projection = GameVec2.Dot(originToCenter, axis);
            return new GameVec2(
                projection - this.worldRadius,
                projection + this.worldRadius
            );
        }

        public override GameVec2 GetRestoreMTV(GameCollider colliderB, bool onlyIntersect = true)
        {
            var collider = colliderB as GameCircleCollider;
            var line = this.worldCenterPos - collider.worldCenterPos;
            var dis = line.magnitude;
            var radiusSum = this.worldRadius + collider.worldRadius;
            if (onlyIntersect && dis >= radiusSum) return GameVec2.zero;
            var dir = line.normalized;
            var mtv = dir * (radiusSum - dis);
            return mtv;
        }

        public override bool IsIntersectWithPoint(GameVec2 point)
        {
            var distance = (point - this.worldCenterPos).sqrMagnitude;
            return distance <= this.worldRadius * this.worldRadius;
        }

        public override GameVec2 GetContactMTV(GameCollider collider)
        {
            return GetRestoreMTV(collider, false);
        }

        public override GameVec2 GetContactMTV_ByPoint(GameVec2 point)
        {
            var offset = point - this.worldCenterPos;
            var dis = offset.magnitude;
            if (dis <= this.worldRadius) return GameVec2.zero;
            var dir = offset.normalized;
            var mtv = dir * (this.worldRadius - dis);
            return mtv;
        }

        public override string ToString()
        {
            var posInfo = $"Center: {this.worldCenterPos}";
            var info = $"Position: {this.worldPos}, Angle: {this.angle}, Scale: {this.scale}, Radius: {this.worldRadius}";
            return $"{posInfo}\n{info}";
        }
    }
}
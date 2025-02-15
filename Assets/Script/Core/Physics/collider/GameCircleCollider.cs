using System;
using GamePlay.Bussiness.Logic;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Core
{
    public class GameCircleCollider : GameColliderBase
    {
        public static readonly GameCircleCollider Default = new GameCircleCollider(null, null, -1);

        public float originRadius => _originRadius;
        float _originRadius;

        public float worldRadius => _originRadius * Math.Abs(this.scale.x);

        public GameCircleCollider(
            GameEntityBase binder,
            GameCircleColliderModel colliderModel,
            int id
        ) : base(binder, colliderModel, id)
        {
        }

        public override string ToString()
        {
            var posInfo = $"Center: {this.worldCenterPos}";
            var info = $"Position: {this.worldPos}, Angle: {this.angle}, Scale: {this.scale}, Radius: {this.worldRadius}";
            return $"{posInfo}\n{info}";
        }

        protected override void _SetByModel(GameColliderModelBase model)
        {
            var colliderModel = model as GameCircleColliderModel;
            this.colliderOffset = colliderModel.getoffset;
            this._originRadius = colliderModel.radius;
            this.worldPos = GameVec2.zero;
            this.originCenterPos = colliderModel.getoffset;
            this.originCenterPos_rotated = colliderModel.getoffset;
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

        protected override void _SetWorldScale(in GameVec2 scale)
        {
            this.scale = scale;
            var pos = this.worldPos;
            this.worldCenterPos = this.originCenterPos_rotated * scale + pos;
        }

        public override GameVec2 GetProjectionOnAxis(in GameVec2 origin, in GameVec2 axis)
        {
            var originToCenter = this.worldCenterPos - origin;
            var projection = GameVec2.Dot(originToCenter, axis);
            return new GameVec2(
                projection - this.worldRadius,
                projection + this.worldRadius
            );
        }

        public override GameVec2 GetResolvingMTV(GameColliderBase colliderB, bool onlyDetectPenetration = true)
        {
            var collider = colliderB as GameCircleCollider;
            var line = this.worldCenterPos - collider.worldCenterPos;
            var dis = line.magnitude;
            var radiusSum = this.worldRadius + collider.worldRadius;
            if (onlyDetectPenetration)
            {
                if (dis >= radiusSum) return GameVec2.zero;
            }
            var dir = line.normalized;
            if (dir == GameVec2.zero) dir = GameVec2.right;
            var mtv = dir * (radiusSum - dis);
            return mtv;
        }

        public override GameVec2 GetResolvingMTV(in GameVec2 point, bool onlyDetectPenetration = true)
        {
            var offset = point - this.worldCenterPos;
            var dis = offset.magnitude;
            if (onlyDetectPenetration)
            {
                if (dis >= this.worldRadius) return GameVec2.zero;
            }
            var dir = offset.normalized;
            var mtv = dir * (this.worldRadius - dis);
            return mtv;
        }

        public override bool CheckOverlap(in GameVec2 point)
        {
            var distance = (point - this.worldCenterPos).sqrMagnitude;
            return distance <= this.worldRadius * this.worldRadius;
        }
    }
}
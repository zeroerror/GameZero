using System;
using System.Collections.Generic;
using GamePlay.Bussiness.Logic;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    public class GameFanCollider : GameCollider
    {
        GameVec2 _originP1;
        GameVec2 _originP2;
        GameVec2 _originP1_rotated;
        GameVec2 _originP2_rotated;

        public GameVec2 worldP1 { get; private set; }
        public GameVec2 worldP2 { get; private set; }

        public float worldRadius => originRadius * Math.Abs(this.scale);
        public float originRadius { get; private set; }

        public float fanAngle { get; private set; }

        public GameVec2 axis1 { get; private set; }
        public GameVec2 axis2 { get; private set; }
        public GameVec2 normal1 { get; private set; }
        public GameVec2 normal2 { get; private set; }

        public GameVec2 projection1 => GetProjectionOnAxis(this.worldCenterPos, this.normal1);
        public GameVec2 projection2 => GetProjectionOnAxis(this.worldCenterPos, this.normal2);

        public GameFanCollider(GameEntity binder, GameColliderFanModel colliderModel, int id, float scale = 1)
            : base(binder, colliderModel, id, scale)
        {
        }

        protected override void _SetByModel(object model, float scale)
        {
            var colliderModel = model as GameColliderFanModel;
            var fanOffset = colliderModel.offset;
            var radius = colliderModel.radius;
            var fanAngle = colliderModel.fanAngle;
            // 扇形角度限制 (0, 180]
            if (fanAngle <= 0) fanAngle = 0.1f;
            if (fanAngle > 180) fanAngle = 180;

            this.colliderOffset = fanOffset;
            this.originRadius = radius;
            this.fanAngle = fanAngle;

            // 坐标设置 T
            this.worldPos = GameVec2.zero;

            var halfFanAngle = fanAngle / 2;
            this._originP1 = GameVectorUtil.RotateOnAxisZ(new GameVec2(radius, 0), halfFanAngle) + fanOffset;
            this._originP2 = GameVectorUtil.RotateOnAxisZ(new GameVec2(radius, 0), -halfFanAngle) + fanOffset;
            this.originCenterPos = fanOffset;
            this.originCenterPos_rotated = this.originCenterPos;

            this._originP1_rotated = this._originP1;
            this._originP2_rotated = this._originP2;
            this.worldP1 = this._originP1;
            this.worldP2 = this._originP2;
        }

        protected override void _SetWorldPosition(GameVec2 pos)
        {
            var offset = pos - this.worldPos;
            this.worldPos += offset;
            this.worldCenterPos += offset;
            this.worldP1 += offset;
            this.worldP2 += offset;
        }

        protected override void _SetWorldAngle(float angle)
        {
            this.angle = angle;
            var pos = this.worldPos;
            var scale = this.scale;

            this._originP1_rotated = GameVectorUtil.RotateOnAxisZ(_originP1, angle);
            this._originP2_rotated = GameVectorUtil.RotateOnAxisZ(_originP2, angle);
            this.originCenterPos_rotated = GameVectorUtil.RotateOnAxisZ(this.originCenterPos, angle);

            this.worldP1 = this._originP1_rotated * (scale) + (pos);
            this.worldP2 = this._originP2_rotated * (scale) + (pos);
            this.worldCenterPos = this.originCenterPos_rotated * (scale) + (pos);

            this.localAxisX = GameVectorUtil.RotateOnAxisZ(GameVec2.right, angle);
            this.localAxisY = GameVectorUtil.RotateOnAxisZ(GameVec2.up, angle);

            this.axis1 = (this.worldP1 - (this.worldCenterPos)).normalized;
            this.axis2 = (this.worldP2 - (this.worldCenterPos)).normalized;
            this.normal1 = GameVectorUtil.RotateOnAxisZ(this.axis1, 90);
            this.normal2 = GameVectorUtil.RotateOnAxisZ(this.axis2, -90);
        }

        protected override void _SetWorldScale(float scale)
        {
            this.scale = scale;
            var pos = this.worldPos;
            this.worldP1 = this._originP1_rotated * (scale) + (pos);
            this.worldP2 = this._originP2_rotated * (scale) + (pos);
            this.worldCenterPos = this.originCenterPos_rotated * (scale) + (pos);
        }

        public override GameVec2 GetProjectionOnAxis(GameVec2 origin, GameVec2 axis)
        {
            axis.Normalize();
            var cross1 = axis.Cross(this.axis1);
            var cross2 = axis.Cross(this.axis2);
            bool isInsideAngle = Math.Abs(cross1) != 1 && Math.Abs(cross2) != 1 && cross1 * cross2 < 0;

            if (isInsideAngle && this.fanAngle < 90)
            {
                var center = this.worldCenterPos;
                float centerDot = axis.Dot(center - origin);
                var offset = axis.Mul(worldRadius);
                if (axis.Dot(this.localAxisX) < 0) offset.NegSelf();

                var p = center.Add(offset);
                float pDot = axis.Dot(p.Sub(origin));
                return new GameVec2(Math.Min(centerDot, pDot), Math.Max(centerDot, pDot));
            }

            var dotList = new List<float>();
            dotList.Add(axis.Dot(this.worldCenterPos.Sub(origin)));
            dotList.Add(axis.Dot(this.worldP1.Sub(origin)));
            dotList.Add(axis.Dot(this.worldP2.Sub(origin)));

            if (isInsideAngle)
            {
                var p3 = this.worldCenterPos.Add(axis.Mul(worldRadius));
                dotList.Add(axis.Dot(p3.Sub(origin)));
            }

            return new GameVec2(dotList.Min(), dotList.Max());
        }

        public override GameVec2 GetRestoreMTV(GameCollider colliderB, bool onlyIntersect = true)
        {
            GameLogger.Error("GameFanCollider.GetRestoreMTV not implemented");
            return GameVec2.zero;
        }

        public override GameVec2 GetContactMTV(GameCollider collider)
        {
            return GetRestoreMTV(collider, false);
        }

        public override bool IsIntersectWithPoint(GameVec2 point)
        {
            var l = point.Sub(this.worldCenterPos);
            if (l.magnitude > worldRadius) return false;
            var n = l.normalized;
            bool isSameSide = n.Cross(this.axis1) * n.Cross(this.axis2) > 0;
            return !isSameSide;
        }

        public override GameVec2 GetContactMTV_ByPoint(GameVec2 point)
        {
            var n_point = (point - (this.worldCenterPos)).normalized;
            var n1 = (this.worldP1 - (this.worldCenterPos)).normalized;
            var n2 = (this.worldP2 - (this.worldCenterPos)).normalized;

            if (n_point.Cross(n1) < 0) return this.projection1.Sub(n_point.Project(n1));
            if (n_point.Cross(n2) > 0) return this.projection2.Sub(n_point.Project(n2));

            return n_point.Mul(point.Sub(this.worldCenterPos).magnitude - this.worldRadius);
        }

        public override string ToString()
        {
            return $"Center: {this.worldCenterPos}, Origin1: {this.worldP1}, Origin2: {this.worldP2}\n" +
                   $"Position: {this.worldPos}, Angle: {this.angle}, Scale: {this.scale}, Radius: {worldRadius}, Fan Angle: {this.fanAngle}";
        }
    }
}
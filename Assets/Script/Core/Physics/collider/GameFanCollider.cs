using System;
using System.Collections.Generic;
using GamePlay.Bussiness.Logic;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    public class GameFanCollider : GameColliderBase
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

        public GameFanCollider(GameEntityBase binder, GameFanColliderModel colliderModel, int id)
            : base(binder, colliderModel, id)
        {
        }


        public override string ToString()
        {
            return $"Center: {this.worldCenterPos}, Origin1: {this.worldP1}, Origin2: {this.worldP2}\n" +
                   $"Position: {this.worldPos}, Angle: {this.angle}, Scale: {this.scale}, Radius: {worldRadius}, Fan Angle: {this.fanAngle}";
        }

        protected override void _SetByModel(GameColliderModelBase model)
        {
            var colliderModel = model as GameFanColliderModel;
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

            this.axis1 = (this.worldP1 - this.worldCenterPos).normalized;
            this.axis2 = (this.worldP2 - this.worldCenterPos).normalized;
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

        public override GameVec2 GetProjectionOnAxis(in GameVec2 origin, in GameVec2 axis)
        {
            // 参数初始化
            axis.Normalize();
            var cross1 = axis.Cross(this.axis1);
            var cross2 = axis.Cross(this.axis2);
            var isInsideAngle = cross1 * cross2 < 0;
            var isNegSituation = cross1 < 0;
            var p1 = this.worldP1;
            var p2 = this.worldP2;
            var p3 = this.worldCenterPos;
            var p4 = p3 + axis * this.worldRadius * (isNegSituation ? -1 : 1);
            // 情况1 - axis所在直线会经过扇形范围
            //- 1.1 正向/反向射线经过
            //- 1.2 投影4点：扇形3点+直线圆弧交点
            if (isInsideAngle)
            {
                Span<GameVec2> points = stackalloc GameVec2[] { p1, p2, p3, p4 };
                Span<float> dots = stackalloc float[4];
                for (int i = 0; i < 4; i++)
                {
                    dots[i] = axis.Dot(points[i] - origin);
                }
                var pj1 = new GameVec2(dots.Min(), dots.Max());
                return pj1;
            }
            // 情况2 - axis所在直线不经过扇形范围
            //- 2.1 投影2点：扇形2端点
            var dot1 = axis.Dot(p1 - origin);
            var dot2 = axis.Dot(p2 - origin);
            var pj2 = new GameVec2(Math.Min(dot1, dot2), Math.Max(dot1, dot2));
            return pj2;
        }

        public override GameVec2 GetResolvingMTV(GameColliderBase colliderB, bool onlyDetectPenetration = true)
        {
            GameLogger.Error("GameFanCollider.GetResolvingMTV not implemented");
            return GameVec2.zero;
        }

        public override bool CheckOverlap(in GameVec2 point)
        {
            var l = point.Sub(this.worldCenterPos);
            if (l.magnitude > worldRadius) return false;
            var n = l.normalized;
            bool isSameSide = n.Cross(this.axis1) * n.Cross(this.axis2) > 0;
            return !isSameSide;
        }

        public override GameVec2 GetResolvingMTV(in GameVec2 point, bool onlyDetectPenetration = true)
        {
            /**
点移动到扇形表面所需MTV求解
分为以下情况：
1. 在扇形左边界左侧 或 在扇形右边界右侧
MTV = 点在边界法向量上的投影向量 + 点在边界上的投影向量
2. 在扇形角度内 且 距离大于半径
MTV = 点到扇形原点向量 - 半径模长向量
3. 在扇形角度内 且 距离小于半径
MTV = min( 点到圆弧向量, 点到左边界向量, 点到右边界向量)
            */
            // if (onlyDetectPenetration)
            // {
            //     var offset = point - this.worldCenterPos;
            //     var dis = offset.magnitude;
            //     if (dis >= this.worldRadius) return GameVec2.zero;
            //     var isleft = offset.Cross(this.axis1) > 0;
            //     if (isleft) return GameVec2.zero;
            //     var isright = offset.Cross(this.axis2) > 0;
            //     if (isright) return GameVec2.zero;
            // }
            // var isleft = this.axis1.Cross(point.Sub(this.worldCenterPos)) > 0;
            // if (isleft)
            // {
            //     var v1 = this.GetProjectionOnAxis(this.worldCenterPos, this.normal1);
            //     var v2 = this.GetProjectionOnAxis(point, this.normal1);
            // }
            return GameVec2.zero;
        }
    }
}
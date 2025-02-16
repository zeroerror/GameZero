using System;
using GamePlay.Bussiness.Logic;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Core
{
    public class GameFanCollider : GameColliderBase
    {
        public static readonly GameFanCollider Default = new GameFanCollider(null, null, -1);
        GameVec2 _originP1;
        GameVec2 _originP2;
        GameVec2 _originP1_rotated;
        GameVec2 _originP2_rotated;

        public GameVec2 worldP1 { get; private set; }
        public GameVec2 worldP2 { get; private set; }

        public float worldRadius => originRadius * Math.Abs(this.scale.x);
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
            var fanOffset = colliderModel.getoffset;
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

        protected override void _SetWorldScale(in GameVec2 scale)
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
            // 扇形间交叉检测
            // 1 距离大于半径和, 必不相交
            // 2 SAT投影轴
            //-AB扇形各自的两边
            //-A扇形分别到B扇形3点三线, B扇形分别到A扇形3点三线
            GameLogger.LogError("GameFanCollider.GetResolvingMTV not implemented");
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
            var cross_1 = point.Sub(this.worldCenterPos).Cross(this.axis1);
            var cross_2 = point.Sub(this.worldCenterPos).Cross(this.axis2);
            var toPOffset = point.Sub(this.worldCenterPos);
            var toPDir = toPOffset.normalized;
            // 点在扇形正向范围内
            if (cross_1 > 0 && cross_2 < 0)
            {
                //-距离大于半径 MTV = 扇形原点到点向量 - 半径模长
                var isFar = toPOffset.sqrMagnitude >= this.worldRadius * this.worldRadius;
                if (isFar)
                {
                    if (onlyDetectPenetration) return GameVec2.zero;
                    var mtv1 = toPDir * (toPOffset.magnitude - this.worldRadius);
                    return mtv1;
                }
                //-距离小于半径 MTV = min(左边界到点, 右边界到点, 圆弧到点)
                Span<GameVec2> mtvs = stackalloc GameVec2[3];
                mtvs[0] = toPOffset.Project(this.normal1);
                mtvs[1] = toPOffset.Project(this.normal2);
                var dis = toPOffset.magnitude;
                mtvs[2] = toPOffset.normalized.Mul(this.worldRadius - dis);
                var mtv2 = mtvs.Min(out var index);
                if (index == 2) mtv2.NegSelf();
                return mtv2;
            }
            if (onlyDetectPenetration) return GameVec2.zero;

            // 点在扇形边界外90°范围内
            //-距离小于半径  MTV = 点在边界法向量投影
            //-距离大于半径  MTV = 点在边界法向量投影 + (扇形原点到点向量 - 半径模长)
            GameVec2 n = default;
            GameVec2 a = default;
            var isOutside90 = false;
            if (toPOffset.Dot(this.axis1) > 0)
            {
                isOutside90 = true;
                n = this.normal1;
                a = this.axis1;
            }
            else if (toPOffset.Dot(this.axis2) > 0)
            {
                isOutside90 = true;
                n = this.normal2;
                a = this.axis2;
            }
            if (isOutside90)
            {
                var mtv3 = toPOffset.Project(n);
                var pj = toPOffset.Project(a);
                var pjExtraOffset = pj.magnitude - this.worldRadius;
                if (pjExtraOffset > 0) mtv3.AddSelf(pj.normalized.Mul(pjExtraOffset));
                return mtv3;
            }

            // 点在扇形边界外90°范围外 MTV = 扇形原点到点向量
            return toPOffset;
        }
    }
}
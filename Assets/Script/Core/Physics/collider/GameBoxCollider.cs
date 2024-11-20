using System.Collections.Generic;
using GamePlay.Bussiness.Logic;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    public class GameBoxCollider : GameColliderBase
    {
        public GameVec2 originP1;
        public GameVec2 originP2;
        public GameVec2 originP3;
        public GameVec2 originP4;

        public GameVec2 originP1_rotated;
        public GameVec2 originP2_rotated;
        public GameVec2 originP3_rotated;
        public GameVec2 originP4_rotated;

        public GameVec2 worldP1;
        public GameVec2 worldP2;
        public GameVec2 worldP3;
        public GameVec2 worldP4;

        /** 原始宽度 */
        public float originWidth
        {
            get { return originP2.x - originP1.x; }
        }

        /** 世界宽度 */
        public float worldWidth
        {
            get { return originWidth * GameMathF.Abs(scale); }
        }

        /** 原始高度 */
        public float originHeight
        {
            get { return originP2.y - originP3.y; }
        }

        /** 世界高度 */
        public float worldHeight
        {
            get { return originHeight * GameMathF.Abs(scale); }
        }


        private GameVec2 _worldMinPos
        {
            get { return scale > 0 ? worldP3 : worldP2; }
        }

        private GameVec2 _worldMaxPos
        {
            get { return scale > 0 ? worldP2 : worldP3; }
        }

        public GameBoxCollider(
            GameEntityBase binder,
            GameBoxColliderModel colliderModel,
            int id,
            float scale = 1
        ) : base(binder, colliderModel, id, scale)
        {
            this.scale = scale;
        }

        protected override void _SetByModel(GameColliderModelBase model, float scale)
        {
            var colliderModel = model as GameBoxColliderModel;
            // 坐标设置 T
            worldPos = GameVec2.zero;
            float boxWidth = colliderModel.width;
            float boxLength = colliderModel.height;
            float halfWidth = boxWidth / 2;
            float halfLength = boxLength / 2;
            GameVec2 boxOffset = colliderModel.offset;
            colliderOffset = boxOffset;

            originP1 = new GameVec2(-halfWidth, halfLength) + boxOffset;
            originP2 = new GameVec2(halfWidth, halfLength) + boxOffset;
            originP3 = new GameVec2(-halfWidth, -halfLength) + boxOffset;
            originP4 = new GameVec2(halfWidth, -halfLength) + boxOffset;

            originCenterPos = boxOffset;

            originP1_rotated = originP1;
            originP2_rotated = originP2;
            originP3_rotated = originP3;
            originP4_rotated = originP4;
            originCenterPos_rotated = originCenterPos;

            worldP1 = originP1;
            worldP2 = originP2;
            worldP3 = originP3;
            worldP4 = originP4;
        }

        /** 获取包围盒世界坐标系下的所有顶点 */
        public GameVec2[] GetWorldPoints()
        {
            return new GameVec2[] { worldP1, worldP2, worldP3, worldP4 };
        }

        protected override void _SetWorldPosition(GameVec2 pos)
        {
            GameVec2 offset = pos - worldPos;
            if (offset.Equals(GameVec2.zero)) return;

            worldPos += offset;
            worldCenterPos += offset;
            worldP1 += offset;
            worldP2 += offset;
            worldP3 += offset;
            worldP4 += offset;
        }

        protected override void _SetWorldAngle(float angleZ)
        {
            this.angle = angleZ;

            GameVec2 pos = worldPos;
            float scale = this.scale;

            originP1_rotated = GameVectorUtil.RotateOnAxisZ(originP1, angleZ);
            originP2_rotated = GameVectorUtil.RotateOnAxisZ(originP2, angleZ);
            originP3_rotated = GameVectorUtil.RotateOnAxisZ(originP3, angleZ);
            originP4_rotated = GameVectorUtil.RotateOnAxisZ(originP4, angleZ);
            originCenterPos_rotated = GameVectorUtil.RotateOnAxisZ(originCenterPos, angleZ);

            worldP1 = originP1_rotated * (scale) + (pos);
            worldP2 = originP2_rotated * (scale) + (pos);
            worldP3 = originP3_rotated * (scale) + (pos);
            worldP4 = originP4_rotated * (scale) + (pos);
            worldCenterPos = originCenterPos_rotated * (scale) + (pos);

            localAxisX = GameVectorUtil.RotateOnAxisZ(GameVec2.right, angleZ);
            localAxisY = GameVectorUtil.RotateOnAxisZ(GameVec2.up, angleZ);
        }

        protected override void _SetWorldScale(float scale)
        {
            this.scale = scale;
            GameVec2 pos = worldPos;
            worldP1 = originP1_rotated * scale + pos;
            worldP2 = originP2_rotated * scale + pos;
            worldP3 = originP3_rotated * scale + pos;
            worldP4 = originP4_rotated * scale + pos;
            worldCenterPos = originCenterPos_rotated * scale + pos;
        }

        public override GameVec2 GetProjectionOnAxis(in GameVec2 origin, in GameVec2 axis)
        {
            GameVec2 p1 = worldP1;
            GameVec2 p2 = worldP2;
            GameVec2 p3 = worldP3;
            GameVec2 p4 = worldP4;
            float p1Dot = GameVec2.Dot(axis, p1 - origin);
            float p2Dot = GameVec2.Dot(axis, p2 - origin);
            float p3Dot = GameVec2.Dot(axis, p3 - origin);
            float p4Dot = GameVec2.Dot(axis, p4 - origin);

            float min = GameMathF.Min(p1Dot, p2Dot, p3Dot, p4Dot);
            float max = GameMathF.Max(p1Dot, p2Dot, p3Dot, p4Dot);
            return new GameVec2(min, max);
        }

        public override GameVec2 GetResolvingMTV(GameColliderBase collider, bool onlyDetectPenetration = true)
        {
            var boxCollider = collider as GameBoxCollider;
            if (boxCollider == null) return GameVec2.zero;

            float angleZ1 = this.angle;
            float angleZ2 = boxCollider.angle;

            if (angleZ1 == 0 && angleZ2 == 0)
                return GetResolvingMTV_AABB(boxCollider);

            GameVec2 obbIntersect1 = GetResolvingMTV_OBB(boxCollider);
            if (obbIntersect1.Equals(GameVec2.zero)) return GameVec2.zero;

            GameVec2 obbIntersect2 = boxCollider.GetResolvingMTV_OBB(this);
            if (obbIntersect2.Equals(GameVec2.zero)) return GameVec2.zero;

            GameVec2 mtv = obbIntersect1.sqrMagnitude < obbIntersect2.sqrMagnitude
                ? obbIntersect1
                : obbIntersect2;
            return mtv;
        }

        private GameVec2 GetResolvingMTV_AABB(GameBoxCollider box2, int judgeMode = 0)
        {
            GameBoxCollider box1 = this;
            GameVec2 minPos1 = box1._worldMinPos;
            GameVec2 maxPos1 = box1._worldMaxPos;
            GameVec2 minPos2 = box2._worldMinPos;
            GameVec2 maxPos2 = box2._worldMaxPos;

            if (judgeMode == 0)
            {
                float mtvLenX = GameResolvingUtil.GetRestoreLength(
                    minPos1.x, maxPos1.x, minPos2.x, maxPos2.x, judgeMode);
                if (mtvLenX == 0) return GameVec2.zero;

                float mtvLenY = GameResolvingUtil.GetRestoreLength(
                    minPos1.y, maxPos1.y, minPos2.y, maxPos2.y, judgeMode);
                if (mtvLenY == 0) return GameVec2.zero;

                var records = new List<IGamePhysicsSATParams>();
                var r1 = new IGamePhysicsSATParams
                {
                    mtvLength = mtvLenX,
                    axis = GameVec2.right,
                    pj1 = new GameVec2(minPos1.x, maxPos1.x),
                    pj2 = new GameVec2(minPos2.x, maxPos2.x)
                };
                records.Add(r1);
                var r2 = new IGamePhysicsSATParams
                {
                    mtvLength = mtvLenY,
                    axis = GameVec2.up,
                    pj1 = new GameVec2(minPos1.y, maxPos1.y),
                    pj2 = new GameVec2(minPos2.y, maxPos2.y)
                };
                records.Add(r2);
                return GameResolvingUtil.GetMinMTV(records);
            }
            else
            {
                float mtvLenX = GameResolvingUtil.GetRestoreLength(minPos1.x, maxPos1.x, minPos2.x, maxPos2.x, judgeMode);
                float mtvLenY = GameResolvingUtil.GetRestoreLength(
                    minPos1.y, maxPos1.y, minPos2.y, maxPos2.y, judgeMode);

                var records = new List<IGamePhysicsSATParams>()
                {
                new IGamePhysicsSATParams
                {
                    mtvLength = mtvLenX,
                    axis = GameVec2.right,
                    pj1 = new GameVec2(minPos1.x, maxPos1.x),
                    pj2 = new GameVec2(minPos2.x, maxPos2.x)
                },
                new IGamePhysicsSATParams
                {
                    mtvLength = mtvLenY,
                    axis = GameVec2.up,
                    pj1 = new GameVec2(minPos1.y, maxPos1.y),
                    pj2 = new GameVec2(minPos2.y, maxPos2.y)
                }

                };

                return GameResolvingUtil.AddUpMTV(records);
            }
        }

        private GameVec2 GetResolvingMTV_OBB(GameBoxCollider box2)
        {
            GameBoxCollider pb1 = this;
            float halfWidth1 = pb1.worldWidth / 2;
            GameVec2 pjX1_1 = new GameVec2(-halfWidth1, halfWidth1);
            GameVec2 centerPos1 = pb1.worldCenterPos;
            GameVec2 axisX1 = pb1.localAxisX;
            GameVec2 pjX2_2 = box2.GetProjectionOnAxis(centerPos1, axisX1);

            float mtvLenX = GameResolvingUtil.GetRestoreLength(pjX1_1.x, pjX1_1.y, pjX2_2.x, pjX2_2.y);
            if (mtvLenX == 0) return GameVec2.zero;

            float halfLength1 = pb1.worldHeight / 2;
            GameVec2 pjY1_1 = new GameVec2(-halfLength1, halfLength1);
            GameVec2 axisY1 = pb1.localAxisY;
            GameVec2 pjY1_2 = box2.GetProjectionOnAxis(centerPos1, axisY1);

            float mtvLenY = GameResolvingUtil.GetRestoreLength(pjY1_1.x, pjY1_1.y, pjY1_2.x, pjY1_2.y);
            if (mtvLenY == 0) return GameVec2.zero;

            GameVec2 mtv = GameMathF.Abs(mtvLenX) < GameMathF.Abs(mtvLenY)
                ? new GameVec2(mtvLenX, 0)
                : new GameVec2(0, mtvLenY);

            GameVectorUtil.RotateSelfOnAxisZ(ref mtv, pb1.angle);
            return mtv;
        }

        public override bool CheckOverlap(in GameVec2 point)
        {
            float angleZ = angle;
            if (angleZ == 0)
            {
                return point.x >= _worldMinPos.x && point.x <= _worldMaxPos.x &&
                       point.y >= _worldMinPos.y && point.y <= _worldMaxPos.y;
            }

            GameVec2 localPos = GameVectorUtil.RotateOnAxisZ(point - (worldCenterPos), -angleZ);
            float halfWidth = worldWidth / 2;
            float halfLength = worldHeight / 2;
            GameVec2 minPos = new GameVec2(-halfWidth, -halfLength);
            GameVec2 maxPos = new GameVec2(halfWidth, halfLength);
            return localPos.x >= minPos.x && localPos.x <= maxPos.x &&
                   localPos.y >= minPos.y && localPos.y <= maxPos.y;
        }

        public override GameVec2 GetResolvingMTV(in GameVec2 point, bool onlyDetectPenetration = true)
        {
            if (this.angle == 0) return _GetResolvingMTV_AABB(point, onlyDetectPenetration);
            return _GetResolvingMTV_OBB(point, onlyDetectPenetration);
        }

        private GameVec2 _GetResolvingMTV_AABB(in GameVec2 point, bool onlyDetectPenetration = true)
        {
            GameVec2 minPos = _worldMinPos;
            GameVec2 maxPos = _worldMaxPos;
            if (onlyDetectPenetration)
            {
                if (point.x < minPos.x || point.x > maxPos.x) return GameVec2.zero;
                if (point.y < minPos.y || point.y > maxPos.y) return GameVec2.zero;
            }
            float mtvx = 0;
            float mtvy = 0;
            float offsetx1 = minPos.x - point.x;
            float offsetx2 = maxPos.x - point.x;
            mtvy = GameMathF.Abs(offsetx1) < GameMathF.Abs(offsetx2) ? offsetx1 : offsetx2;
            float offsety1 = minPos.y - point.y;
            float offsety2 = maxPos.y - point.y;
            mtvx = GameMathF.Abs(offsety1) < GameMathF.Abs(offsety2) ? offsety1 : offsety2;
            var mtv = new GameVec2(mtvx, mtvy);
            return mtv;
        }

        private GameVec2 _GetResolvingMTV_OBB(in GameVec2 point, bool onlyDetectPenetration = true)
        {
            var pjx = point.Dot(localAxisX);
            var pjy = point.Dot(localAxisY);
            var halfW = worldWidth / 2;
            var halfH = worldHeight / 2;
            if (onlyDetectPenetration)
            {
                if (pjx < -halfW || pjx > halfW) return GameVec2.zero;
                if (pjy < -halfH || pjy > halfH) return GameVec2.zero;
            }
            var offsetx1 = -halfW - pjx;
            var offsetx2 = halfW - pjx;
            var mtvx = GameMathF.Abs(offsetx1) < GameMathF.Abs(offsetx2) ? offsetx1 : offsetx2;
            var offsety1 = -halfH - pjy;
            var offsety2 = halfH - pjy;
            var mtvy = GameMathF.Abs(offsety1) < GameMathF.Abs(offsety2) ? offsety1 : offsety2;
            var mtv = localAxisX * mtvx + localAxisY * mtvy;
            return mtv;
        }
    }
}
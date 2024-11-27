using System.Collections.Generic;
using GamePlay.Bussiness.Logic;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Core
{
    public class GamePhysicsResolvingUtil
    {
        public static GameVec2 GetResolvingMTV(GameColliderBase colliderA, GameColliderModelBase colliderModelB, in GameTransformArgs transformArgs)
        {
            switch (colliderModelB)
            {
                case GameBoxColliderModel boxModel:
                    var boxCollider = new GameBoxCollider(null, boxModel, -1);
                    boxCollider.UpdateTRS(transformArgs);
                    return GetResolvingMTV(colliderA, boxCollider);
                case GameCircleColliderModel circleModel:
                    var circleCollider = new GameCircleCollider(null, circleModel, -1);
                    circleCollider.UpdateTRS(transformArgs);
                    return GetResolvingMTV(colliderA, circleCollider);
                case GameFanColliderModel fanModel:
                    var fanCollider = new GameFanCollider(null, fanModel, -1);
                    fanCollider.UpdateTRS(transformArgs);
                    return GetResolvingMTV(colliderA, fanCollider);
                default:
                    GameLogger.LogError("GamePhysicsResolvingUtil.GetResolvingMTV: unknown colliderModelB");
                    return GameVec2.zero;
            }
        }

        public static GameVec2 GetResolvingMTV(GameColliderBase colliderA, GameColliderBase colliderB)
        {
            GameVec2 mtv = GameVec2.zero;
            if (colliderA is GameBoxCollider boxCollider)
                mtv = GetResolvingMTV_Box_Collider(boxCollider, colliderB);
            else if (colliderA is GameCircleCollider circleCollider)
                mtv = GetResolvingMTV_Circle_Collider(circleCollider, colliderB);
            else if (colliderA is GameFanCollider fanCollider)
                mtv = GetResolvingMTV_Fan_Collider(fanCollider, colliderB);
            else
                GameLogger.LogError("GamePhysicsRestoreUtil.GetResolvingMTV: colliderA is not a valid collider");

            return mtv;
        }

        private static GameVec2 GetResolvingMTV_Box_Collider(GameBoxCollider boxCollider, GameColliderBase colliderB)
        {
            GameVec2 mtv = GameVec2.zero;
            if (colliderB is GameBoxCollider box)
                mtv = boxCollider.GetResolvingMTV(box);
            else if (colliderB is GameCircleCollider circle)
                mtv = GetResolvingMTV_Box_Circle(boxCollider, circle);
            else if (colliderB is GameFanCollider fan)
                mtv = GetResolvingMTV_Box_Fan(boxCollider, fan);
            return mtv;
        }

        private static GameVec2 GetResolvingMTV_Circle_Collider(GameCircleCollider circleCollider, GameColliderBase colliderB)
        {
            GameVec2 mtv = GameVec2.zero;
            if (colliderB is GameCircleCollider circle)
                mtv = circleCollider.GetResolvingMTV(circle);
            else if (colliderB is GameBoxCollider box)
                mtv = GetResolvingMTV_Box_Circle(box, circleCollider).Neg();
            else if (colliderB is GameFanCollider fan)
                mtv = GetResolvingMTV_Circle_Fan(circleCollider, fan).Neg();
            return mtv;
        }

        private static GameVec2 GetResolvingMTV_Fan_Collider(GameFanCollider fanCollider, GameColliderBase colliderB)
        {
            GameVec2 mtv = GameVec2.zero;
            if (colliderB is GameFanCollider fan)
                mtv = fanCollider.GetResolvingMTV(fan);
            else if (colliderB is GameBoxCollider box)
                mtv = GetResolvingMTV_Box_Fan(box, fanCollider).Neg();
            else if (colliderB is GameCircleCollider circle)
                mtv = GetResolvingMTV_Circle_Fan(circle, fanCollider).Neg();
            return mtv;
        }

        public static float GetRestoreLength(float pjMin1, float pjMax1, float pjMin2, float pjMax2, int judgeMode = 0)
        {
            if (judgeMode == 0)
            {
                if (pjMin1 >= pjMax2 || pjMin2 >= pjMax1) return 0;
            }
            else if (judgeMode == 1)
            {
                if (pjMin1 < pjMax2 && pjMin2 < pjMax1) return 0;
            }
            float min = UnityEngine.Mathf.Max(pjMin1, pjMin2);
            float max = UnityEngine.Mathf.Min(pjMax1, pjMax2);
            if (min > max)
            {
                float temp = min;
                min = max;
                max = temp;
            }
            return max - min;
        }

        public static GameVec2 GetResolvingMTV_Box_Circle(GameBoxCollider boxCollider, GameCircleCollider circleCollider, int judgeMode = 0)
        {
            var records = new List<IGamePhysicsSATParams>();

            var localAxisX = boxCollider.localAxisX;
            var boxPj = new GameVec2(-0.5f * boxCollider.worldWidth, 0.5f * boxCollider.worldWidth);
            var circlePj = circleCollider.GetProjectionOnAxis(boxCollider.worldCenterPos, localAxisX);
            var restoreLen1 = GetRestoreLength(boxPj.x, boxPj.y, circlePj.x, circlePj.y, judgeMode);
            if (judgeMode == 0 && restoreLen1 == 0) return GameVec2.zero;
            records.Add(new IGamePhysicsSATParams { mtvLength = restoreLen1, axis = localAxisX, pj1 = boxPj, pj2 = circlePj });

            var localAxisY = boxCollider.localAxisY;
            boxPj = new GameVec2(-0.5f * boxCollider.worldHeight, 0.5f * boxCollider.worldHeight);
            circlePj = circleCollider.GetProjectionOnAxis(boxCollider.worldCenterPos, localAxisY);
            var restoreLen2 = GetRestoreLength(boxPj.x, boxPj.y, circlePj.x, circlePj.y, judgeMode);
            if (judgeMode == 0 && restoreLen2 == 0) return GameVec2.zero;
            records.Add(new IGamePhysicsSATParams { mtvLength = restoreLen2, axis = localAxisY, pj1 = boxPj, pj2 = circlePj });

            var origin = circleCollider.worldCenterPos;
            circlePj = new GameVec2(-circleCollider.worldRadius, circleCollider.worldRadius);
            var boxPoints = boxCollider.GetWorldPoints();
            foreach (var point in boxPoints)
            {
                var axis = (point - origin).normalized;
                boxPj = boxCollider.GetProjectionOnAxis(origin, axis);
                var len = GetRestoreLength(boxPj.x, boxPj.y, circlePj.x, circlePj.y, judgeMode);
                if (judgeMode == 0 && len == 0) return GameVec2.zero;
                records.Add(new IGamePhysicsSATParams { mtvLength = -len, axis = axis, pj1 = circlePj, pj2 = boxPj });
            }
            return GetMinMTV(records);
        }

        public static GameVec2 GetMinMTV(List<IGamePhysicsSATParams> satParamList)
        {
            GameVec2 minMtv = GameVec2.zero;
            float minLen = float.MaxValue;
            foreach (var record in satParamList)
            {
                var mtvLength = GameMathF.Abs(record.mtvLength);
                if (mtvLength == 0 || mtvLength > minLen) continue;
                minLen = mtvLength;
                var pj1 = record.pj1;
                var pj2 = record.pj2;
                var sub1 = pj2.y - pj1.x;
                var sub2 = pj2.x - pj1.y;
                var dis1 = GameMathF.Abs(sub1);
                var dis2 = GameMathF.Abs(sub2);
                minMtv = dis1 < dis2 ? record.axis * sub1 : record.axis * sub2;
                minMtv *= record.mtvLength > 0 ? 1 : -1;
            }
            return minMtv;
        }

        public static GameVec2 GetResolvingMTV_Box_Fan(
        GameBoxCollider boxCollider,
        GameFanCollider fanCollider,
        int judgeMode = 0)
        {
            var records = new List<IGamePhysicsSATParams>();

            // Use the box's X-axis as the separation axis
            var localAxisX = boxCollider.localAxisX;
            var boxPj = new GameVec2(
                -0.5f * boxCollider.worldWidth,
                0.5f * boxCollider.worldWidth
            );
            var fanPj = fanCollider.GetProjectionOnAxis(
                boxCollider.worldCenterPos,
                localAxisX
            );
            var restoreLen1 = GetRestoreLength(
                boxPj.x,
                boxPj.y,
                fanPj.x,
                fanPj.y,
                judgeMode
            );
            if (judgeMode == 0 && restoreLen1 == 0)
            {
                return GameVec2.zero;
            }
            records.Add(new IGamePhysicsSATParams
            {
                mtvLength = restoreLen1,
                axis = localAxisX,
                pj1 = boxPj,
                pj2 = fanPj
            });

            // Use the box's Y-axis as the separation axis
            var localAxisY = boxCollider.localAxisY;
            boxPj = new GameVec2(
                -0.5f * boxCollider.worldHeight,
                0.5f * boxCollider.worldHeight
            );
            fanPj = fanCollider.GetProjectionOnAxis(
                boxCollider.worldCenterPos,
                localAxisY
            );
            var restoreLen2 = GetRestoreLength(
                boxPj.x,
                boxPj.y,
                fanPj.x,
                fanPj.y,
                judgeMode
            );
            if (judgeMode == 0 && restoreLen2 == 0)
            {
                return GameVec2.zero;
            }
            records.Add(new IGamePhysicsSATParams
            {
                mtvLength = restoreLen2,
                axis = localAxisY,
                pj1 = boxPj,
                pj2 = fanPj
            });

            // Use fan's two edges as separation axis
            var fanAxis1 = fanCollider.normal1;
            boxPj = boxCollider.GetProjectionOnAxis(
                fanCollider.worldCenterPos,
                fanAxis1
            );
            fanPj = fanCollider.projection1;
            var restoreLen3 = GetRestoreLength(
                boxPj.x,
                boxPj.y,
                fanPj.x,
                fanPj.y,
                judgeMode
            );
            if (judgeMode == 0 && restoreLen3 == 0)
            {
                return GameVec2.zero;
            }
            records.Add(new IGamePhysicsSATParams
            {
                mtvLength = restoreLen3,
                axis = fanAxis1,
                pj1 = boxPj,
                pj2 = fanPj
            });

            var fanAxis2 = fanCollider.normal2;
            boxPj = boxCollider.GetProjectionOnAxis(
                fanCollider.worldCenterPos,
                fanAxis2
            );
            fanPj = fanCollider.projection2;
            var restoreLen4 = GetRestoreLength(
                boxPj.x,
                boxPj.y,
                fanPj.x,
                fanPj.y,
                judgeMode
            );
            if (judgeMode == 0 && restoreLen4 == 0)
            {
                return GameVec2.zero;
            }
            records.Add(new IGamePhysicsSATParams
            {
                mtvLength = restoreLen4,
                axis = fanAxis2,
                pj1 = boxPj,
                pj2 = fanPj
            });

            // Use projections of box vertices from the fan's center
            var fo = fanCollider.worldCenterPos;
            fanPj = new GameVec2(-fanCollider.worldRadius, fanCollider.worldRadius);
            var bps = boxCollider.GetWorldPoints();
            foreach (var bp in bps)
            {
                var axis = bp.Sub(fo).normalized;
                boxPj = boxCollider.GetProjectionOnAxis(fo, axis);
                var len = GetRestoreLength(
                    boxPj.x,
                    boxPj.y,
                    fanPj.x,
                    fanPj.y,
                    judgeMode
                );
                if (judgeMode == 0 && len == 0)
                {
                    return GameVec2.zero;
                }
                records.Add(new IGamePhysicsSATParams
                {
                    mtvLength = len,
                    axis = axis,
                    pj1 = boxPj,
                    pj2 = fanPj
                });
            }
            var minMTV = GetMinMTV(records);
            return minMTV;
        }

        public static GameVec2 GetContactMTV_Box_Fan(
            GameBoxCollider boxCollider,
            GameFanCollider fanCollider)
        {
            return GetResolvingMTV_Box_Fan(boxCollider, fanCollider, 1);
        }

        public static GameVec2 GetResolvingMTV_Circle_Fan(
            GameCircleCollider circleCollider,
            GameFanCollider fanCollider,
            int judgeMode = 0)
        {
            GameLogger.LogError("GamePhysicsUtil.GetResolvingMTV_Circle_Fan not implemented");
            return GameVec2.zero;
        }

        public static GameVec2 GetContactMTV_Circle_Fan(
            GameCircleCollider circleCollider,
            GameFanCollider fanCollider)
        {
            GameLogger.LogError("GamePhysicsUtil.GetContactMTV_Circle_Fan not implemented");
            return GameVec2.zero;
        }

        public static GameVec2 AddUpMTV(List<IGamePhysicsSATParams> satParamList)
        {
            var minMtv = GameVec2.zero;
            var minLen = float.MaxValue;

            foreach (var record in satParamList)
            {
                var mtvLength = GameMathF.Abs(record.mtvLength);

                // Skip if no valid MTV or not the minimum MTV
                if (mtvLength == 0) continue;

                minLen = mtvLength;
                var pj1 = record.pj1;
                var pj2 = record.pj2;
                var sub1 = pj2.y - pj1.x;
                var sub2 = pj2.x - pj1.y;
                var dis1 = GameMathF.Abs(sub1);
                var dis2 = GameMathF.Abs(sub2);
                GameVec2 curMTV;

                if (dis1 < dis2)
                    curMTV = record.axis.Multiply(sub1);
                else
                    curMTV = record.axis.Multiply(sub2);

                curMTV = curMTV.Multiply(record.mtvLength > 0 ? 1 : -1);
                minMtv.AddSelf(curMTV);
            }

            return minMtv;
        }
    }
}
















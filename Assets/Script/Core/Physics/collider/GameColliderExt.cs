using GamePlay.Bussiness.Logic;
using UnityEngine;
using GamePlay.Infrastructure;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Core
{
    public static class GameColliderExt
    {
        public static void Draw(this GameColliderModelBase model, in GameTransformArgs trans, Color color)
        {
            switch (model)
            {
                case GameBoxColliderModel box:
                    var coll = GameBoxCollider.Default;
                    coll.SetByModel(box);
                    coll.UpdateTRS(trans);
                    coll.Draw(color);
                    break;
                case GameCircleColliderModel circle:
                    var circleColl = GameCircleCollider.Default;
                    circleColl.SetByModel(circle);
                    circleColl.UpdateTRS(trans);
                    circleColl.Draw(color);
                    break;
                case GameFanColliderModel fan:
                    var fanColl = GameFanCollider.Default;
                    fanColl.SetByModel(fan);
                    fanColl.UpdateTRS(trans);
                    fanColl.Draw(color);
                    break;
                default:
                    GameLogger.LogError($"Draw: unknown collider model {model}");
                    break;
            }
        }

        public static void Draw(this GameColliderBase collider, Color color)
        {
            switch (collider)
            {
                case GameBoxCollider box:
                    box.Draw(color);
                    break;
                case GameCircleCollider circle:
                    circle.Draw(color);
                    break;
                case GameFanCollider fan:
                    fan.Draw(color);
                    break;
                default:
                    GameLogger.LogError($"Draw: unknown collider {collider}");
                    break;
            }
        }

        public static void Draw(this GameBoxCollider box, Color color)
        {
            var worldP1 = box.worldP1;
            var worldP2 = box.worldP2;
            var worldP3 = box.worldP3;
            var worldP4 = box.worldP4;
            Debug.DrawLine(worldP1, worldP2, color);
            Debug.DrawLine(worldP2, worldP4, color);
            Debug.DrawLine(worldP4, worldP3, color);
            Debug.DrawLine(worldP3, worldP1, color);
        }

        public static void Draw(this GameCircleCollider circle, Color color)
        {
            var anchorPos = circle.worldCenterPos;
            var worldRadius = circle.worldRadius;
            Draw(anchorPos, circle.angle, worldRadius, color);
        }

        public static void Draw(in GameVec2 anchorPos, float angle, float worldRadius, Color color)
        {
            var maxDrawCount = 90;
            var stepDegree = 360 / maxDrawCount;
            var p1 = anchorPos + new GameVec2(worldRadius, 0).Rotate(angle);
            for (float degree = 0; degree < 360; degree += stepDegree)
            {
                var p2 = anchorPos + new GameVec2(worldRadius, 0).Rotate(degree);
                Debug.DrawLine(p1, p2, color);
                p1 = p2;
            }
        }

        public static void Draw(this GameFanCollider fan, Color color)
        {
            var anchorPos = fan.worldCenterPos;
            var worldP1 = fan.worldP1;// 右上角
            var worldP2 = fan.worldP2;// 右下角
            Debug.DrawLine(anchorPos, worldP1, color);
            Debug.DrawLine(anchorPos, worldP2, color);
            var maxDrawCount = 90;
            var stepDegree = fan.fanAngle / maxDrawCount;
            var p1 = worldP2;
            for (float i = -fan.fanAngle / 2; i <= fan.fanAngle / 2; i += stepDegree)
            {
                var p2 = anchorPos + new GameVec2(fan.worldRadius, 0).Rotate(fan.angle + i);
                Debug.DrawLine(p1, p2, color);
                p1 = p2;
            }
        }
    }
}
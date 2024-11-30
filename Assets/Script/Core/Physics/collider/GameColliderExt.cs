using GamePlay.Bussiness.Logic;
using UnityEngine;
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
            var worldCenterPos = circle.worldCenterPos;
            var worldRadius = circle.worldRadius;
            var stepAngle = 0.1f;
            var lastPos = worldCenterPos + new GameVec2(worldRadius, 0);
            for (float i = 0; i < 360; i += stepAngle)
            {
                var radian = i * GameMathF.Deg2Rad;
                var x = worldCenterPos.x + worldRadius * GameMathF.Cos(radian);
                var y = worldCenterPos.y + worldRadius * GameMathF.Sin(radian);
                var pos = new GameVec2(x, y);
                Debug.DrawLine(lastPos, pos, color);
                lastPos = pos;
            }
        }

        public static void Draw(this GameFanCollider fan, Color color)
        {
            var worldCenterPos = fan.worldCenterPos;
            var worldP1 = fan.worldP1;// 右上角
            var worldP2 = fan.worldP2;// 右下角
            Debug.DrawLine(worldCenterPos, worldP1, color);
            Debug.DrawLine(worldCenterPos, worldP2, color);
            var stepAngle = 0.1f;
            var lastPos = worldP2;
            for (float i = -fan.fanAngle / 2; i <= fan.fanAngle / 2; i += stepAngle)
            {
                var radian = i * GameMathF.Deg2Rad;
                var x = worldCenterPos.x + fan.worldRadius * GameMathF.Cos(radian);
                var y = worldCenterPos.y + fan.worldRadius * GameMathF.Sin(radian);
                var pos = new Vector2(x, y);
                Debug.DrawLine(lastPos, pos, color);
                lastPos = pos;
            }
        }
    }
}
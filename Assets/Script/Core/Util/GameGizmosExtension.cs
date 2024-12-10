using UnityEngine;

namespace GamePlay.Core
{
    public static class GameGizmosExtension
    {
        public static void DrawCircle(in Vector3 position, float radius)
        {
            var maxDrawCount = 90;
            var stepAngle = 360 / maxDrawCount;
            var worldRadius = radius;
            var lastPos = position.Add(new Vector2(worldRadius, 0));
            for (float i = 0; i < 360; i += stepAngle)
            {
                var radian = i * GameMathF.Deg2Rad;
                var x = position.x + worldRadius * GameMathF.Cos(radian);
                var y = position.y + worldRadius * GameMathF.Sin(radian);
                var nextP = new Vector3(x, y, position.z);
                Gizmos.DrawLine(lastPos, nextP);
                lastPos = nextP;
            }
        }
    }
}
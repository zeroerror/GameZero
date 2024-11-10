using GameVec2 = UnityEngine.Vector2;
using GameMath = System.MathF;
using Random = UnityEngine.Random;

public static class GameVectorUtil
{
    private static readonly GameVec2 VECTOR_RIGHT = GameVec2.right; // 使用 Unity 的 GameVec2.right

    // 计算某一方向到目标方向之间的角度(绝对值范围0-180)
    public static float GetAngleBetweenDirs(GameVec2 dstDir, GameVec2 baseDir = default)
    {
        if (baseDir == default) baseDir = GameVec2.right;  // 默认基础方向为右向量

        if (dstDir == GameVec2.zero) return 0;

        float startAngle = RadiansToDegrees(GameMath.Atan2(baseDir.y, baseDir.x));
        float dstAngle = RadiansToDegrees(GameMath.Atan2(dstDir.y, dstDir.x));
        float betweenAngle = dstAngle - startAngle;

        // 保证角度在 -180 到 180 之间
        if (betweenAngle > 180) betweenAngle -= 360;
        else if (betweenAngle < -180) betweenAngle += 360;

        return betweenAngle;
    }

    public static float RadiansToDegrees(float radians)
    {
        return radians * 180 / GameMath.PI;
    }

    // 根据Z轴旋转一定角度
    public static GameVec2 RotateOnAxisZ(GameVec2 tar, float angleZ)
    {
        float radian = -(angleZ * GameMath.PI) / 180;
        float cos = GameMath.Cos(radian);
        float sin = GameMath.Sin(radian);
        return new GameVec2(tar.x * cos + tar.y * sin, tar.y * cos - tar.x * sin);
    }

    // 根据Z轴旋转一定角度并修改原向量
    public static void RotateSelfOnAxisZ(ref GameVec2 tar, float angleZ)
    {
        float radian = -(angleZ * GameMath.PI) / 180;
        float cos = GameMath.Cos(radian);
        float sin = GameMath.Sin(radian);
        tar = new GameVec2(tar.x * cos + tar.y * sin, tar.y * cos - tar.x * sin);
    }

    // 获取XY平面随机方向向量
    public static GameVec2 GetXYRandomDirection(float startAngle = 0, float endAngle = 360, GameVec2 baseDir = default)
    {
        if (baseDir == default) baseDir = GameVec2.right;

        float angle = startAngle + Random.Range(0f, endAngle - startAngle);
        return RotateOnAxisZ(baseDir, angle);
    }

    // 获取某一点XY平面指定半径内的随机点
    public static GameVec2 GetXYRandomPointInRadius(GameVec2 center, float radius)
    {
        float radians = Random.Range(0f, GameMath.PI * 2);
        float randomRadius = Random.Range(0f, radius);
        return new GameVec2(center.x + randomRadius * GameMath.Cos(radians), center.y + randomRadius * GameMath.Sin(radians));
    }

    // 获取某一点XY平面指定半径区间范围内的随机点
    public static GameVec2 GetXYRandomPointInRadiusRange(GameVec2 center, float minRadius, float maxRadius)
    {
        float radians = Random.Range(0f, GameMath.PI * 2);
        float randomRadius = Random.Range(minRadius, maxRadius);
        return new GameVec2(center.x + randomRadius * GameMath.Cos(radians), center.y + randomRadius * GameMath.Sin(radians));
    }

    // 判断两个向量是否相等
    public static bool ValueEquals(GameVec2 v1, GameVec2 v2)
    {
        return v1 == v2; // 使用 GameVec2 的内置相等比较
    }

    // 获取两个向量之间的距离平方值
    public static float GetDisSqr(GameVec2 v1, GameVec2 v2)
    {
        return (v1 - v2).sqrMagnitude; // 使用 GameVec2 的内置差向量和sqrMagnitude属性
    }
}

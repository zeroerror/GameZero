using System.Numerics;
using GameVec2 = UnityEngine.Vector2;
using GameVec3 = UnityEngine.Vector3;

public static class GameVectorExtensions
{
    public static float Cross(this GameVec2 v1, in GameVec2 v2)
    {
        return v1.x * v2.y - v1.y * v2.x;
    }

    public static float Dot(this GameVec2 v1, in GameVec2 v2)
    {
        return v1.x * v2.x + v1.y * v2.y;
    }

    public static GameVec2 Mul(this GameVec2 v, float f)
    {
        return new GameVec2(v.x * f, v.y * f);
    }

    public static GameVec2 Multiply(this GameVec2 v, float f)
    {
        return Mul(v, f);
    }

    public static GameVec2 Add(this GameVec2 v1, in GameVec2 v2)
    {
        return new GameVec2(v1.x + v2.x, v1.y + v2.y);
    }

    public static void AddSelf(this ref GameVec2 v1, in GameVec2 v2)
    {
        v1.x += v2.x;
        v1.y += v2.y;
    }

    public static GameVec2 Sub(this GameVec2 v1, in GameVec2 v2)
    {
        return new GameVec2(v1.x - v2.x, v1.y - v2.y);
    }

    public static GameVec2 Neg(this GameVec2 v)
    {
        return new GameVec2(-v.x, -v.y);
    }

    public static GameVec2 Negate(this GameVec2 v)
    {
        return Neg(v);
    }

    public static GameVec2 NegSelf(this GameVec2 v)
    {
        return new GameVec2(-v.x, -v.y);
    }

    public static GameVec2 Project(this GameVec2 v1, in GameVec2 v2)
    {
        return v2.Mul(v1.Dot(v2) / v2.Dot(v2));
    }

    public static GameVec2 GetVec2(this GameVec3 v)
    {
        return new GameVec2(v.x, v.y);
    }
}
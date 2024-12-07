using System.Numerics;
using GamePlay.Core;
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

    public static GameVec3 Mul(this GameVec3 v, float f)
    {
        return new GameVec3(v.x * f, v.y * f);
    }

    public static GameVec3 Multiply(this GameVec3 v, float f)
    {
        return Mul(v, f);
    }

    public static void MulSelf(this ref GameVec3 v, in GameVec3 v2)
    {
        v.x *= v2.x;
        v.y *= v2.y;
        v.z *= v2.z;
    }

    public static GameVec3 Mul(this GameVec3 v, in GameVec3 v2)
    {
        v.x *= v2.x;
        v.y *= v2.y;
        v.z *= v2.z;
        return v;
    }

    public static GameVec2 Add(this GameVec2 v1, in GameVec2 v2)
    {
        return new GameVec2(v1.x + v2.x, v1.y + v2.y);
    }

    public static GameVec3 Add(this GameVec3 v1, in GameVec2 v2)
    {
        return new GameVec3(v1.x + v2.x, v1.y + v2.y, v1.z);
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

    public static void NegSelf(ref this GameVec2 v)
    {
        v.x = -v.x;
        v.y = -v.y;
    }

    public static GameVec2 Project(this GameVec2 v1, in GameVec2 v2)
    {
        return v2.Mul(v1.Dot(v2) / v2.Dot(v2));
    }

    public static GameVec2 GetVec2(this GameVec3 v)
    {
        return new GameVec2(v.x, v.y);
    }

    /// <summary> 
    /// 旋转自身向量, 返回旧的向量
    /// <para name="angle">角度（以度为单位), 正为逆时针, 负数为顺时针</para>
    /// </summary>
    public static GameVec2 RotateSelf(this ref GameVec2 v, float angle)
    {
        float radian = -angle * GameMathF.Deg2Rad;
        float cos = GameMathF.Cos(radian);
        float sin = GameMathF.Sin(radian);
        var x = v.x * cos + v.y * sin;
        var y = v.y * cos - v.x * sin;
        v.x = x;
        v.y = y;
        return v;
    }

    /// <summary> 
    /// 旋转向量, 返回新的向量
    /// <para name="angle">角度（以度为单位), 正为逆时针, 负数为顺时针</para>
    /// </summary>
    public static GameVec2 Rotate(this GameVec2 v, float angle)
    {
        float radian = -angle * GameMathF.Deg2Rad;
        float cos = GameMathF.Cos(radian);
        float sin = GameMathF.Sin(radian);
        return new GameVec2(v.x * cos + v.y * sin, v.y * cos - v.x * sin);
    }
}
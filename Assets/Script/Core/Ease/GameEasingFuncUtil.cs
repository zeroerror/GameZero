using System;
using GameMathF = System.Math;

public static class GameEasingFuncUtil
{
    private static readonly Func<float, float>[] TYPE_2_FUNC = new Func<float, float>[]
    {
        Immediate,
        Linear,
        EaseInSine,
        EaseOutSine,
        EaseInOutSine,
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInQuart,
        EaseOutQuart,
        EaseInOutQuart,
        EaseInQuint,
        EaseOutQuint,
        EaseInOutQuint,
        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo,
        EaseInCirc,
        EaseOutCirc,
        EaseInOutCirc,
        EaseInBack,
        EaseOutBack,
        EaseInOutBack,
        EaseInElastic
    };

    public static float EaseByType(GameEasingType type, float t)
    {
        t = GameMathF.Max(0, GameMathF.Min(1, t));
        if (float.IsNaN(t)) return t;
        return TYPE_2_FUNC[(int)type](t);
    }

    private static float Immediate(float t) => 1;

    private static float Linear(float t) => t;

    private static float EaseInSine(float t) => 1 - (float)GameMathF.Cos((t * GameMathF.PI) / 2);

    private static float EaseOutSine(float t) => (float)GameMathF.Sin((t * GameMathF.PI) / 2);

    private static float EaseInOutSine(float t) => -0.5f * ((float)GameMathF.Cos(GameMathF.PI * t) - 1);

    private static float EaseInQuad(float t) => t * t;

    private static float EaseOutQuad(float t) => 1 - (1 - t) * (1 - t);

    private static float EaseInOutQuad(float t) => t < 0.5f ? 2 * t * t : 1 - (float)GameMathF.Pow(-2 * t + 2, 2) / 2;

    private static float EaseInCubic(float t) => t * t * t;

    private static float EaseOutCubic(float t) => 1 - (float)GameMathF.Pow(1 - t, 3);

    private static float EaseInOutCubic(float t) => t < 0.5f ? 4 * t * t * t : 1 - (float)GameMathF.Pow(-2 * t + 2, 3) / 2;

    private static float EaseInQuart(float t) => t * t * t * t;

    private static float EaseOutQuart(float t) => 1 - (float)GameMathF.Pow(1 - t, 4);

    private static float EaseInOutQuart(float t) => t < 0.5f ? 8 * t * t * t * t : 1 - (float)GameMathF.Pow(-2 * t + 2, 4) / 2;

    private static float EaseInQuint(float t) => t * t * t * t * t;

    private static float EaseOutQuint(float t) => 1 - (float)GameMathF.Pow(1 - t, 5);

    private static float EaseInOutQuint(float t) => t < 0.5f ? 16 * t * t * t * t * t : 1 - (float)GameMathF.Pow(-2 * t + 2, 5) / 2;

    private static float EaseInExpo(float t) => t == 0 ? 0 : (float)GameMathF.Pow(2, 10 * (t - 1));

    private static float EaseOutExpo(float t) => t == 1 ? 1 : 1 - (float)GameMathF.Pow(2, -10 * t);

    private static float EaseInOutExpo(float t) => t == 0 ? 0 : t == 1 ? 1 : t < 0.5f ? 0.5f * (float)GameMathF.Pow(2, 20 * t - 10) : 1 - 0.5f * (float)GameMathF.Pow(2, 10 - 20 * t);

    private static float EaseInCirc(float t) => 1 - (float)GameMathF.Sqrt(1 - t * t);

    private static float EaseOutCirc(float t) => (float)GameMathF.Sqrt(1 - GameMathF.Pow(t - 1, 2));

    private static float EaseInOutCirc(float t) => t < 0.5f ? 0.5f * (1 - (float)GameMathF.Sqrt(1 - 4 * t * t)) : 0.5f * ((float)GameMathF.Sqrt(1 - GameMathF.Pow(-2 * t + 2, 2)) + 1);

    private static float EaseInBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;
        return c3 * t * t * t - c1 * t * t;
    }

    private static float EaseOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;
        return 1 + c3 * (float)GameMathF.Pow(t - 1, 3) + c1 * (float)GameMathF.Pow(t - 1, 2);
    }

    private static float EaseInOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;
        return t < 0.5f ? (float)GameMathF.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2) / 2 : (float)GameMathF.Pow(2 * t - 2, 2) * ((c2 + 1) * (2 * t - 2) + c2) + 2 / 2;
    }

    private static float EaseInElastic(float t)
    {
        // Implement easeInElastic logic here
        return t;  // Placeholder implementation
    }
}


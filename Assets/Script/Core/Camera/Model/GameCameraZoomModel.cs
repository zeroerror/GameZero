using System;
/** 相机缩放参数模型 */
public class GameCameraZoomModel
{
    public readonly float ZoomDuration;
    public readonly float ZoomAmplitude;
    public readonly GameEasingType ZoomEase;
    public readonly bool ResetOnEnd;

    public GameCameraZoomModel(
        float zoomDuration,
        float zoomAmplitude,
        GameEasingType zoomEase = GameEasingType.Parabola,
        bool resetOnEnd = true)
    {
        this.ZoomDuration = zoomDuration;
        this.ZoomAmplitude = zoomAmplitude;
        this.ZoomEase = zoomEase;
        this.ResetOnEnd = resetOnEnd;
    }


    public static GameCameraZoomModel ParseToModel(string str)
    {
        if (!IsStrValid(str)) return null;
        var elements = str.Split('&');
        return new GameCameraZoomModel(
            float.Parse(elements[0]),
            float.Parse(elements[1]),
            (GameEasingType)Enum.Parse(typeof(GameEasingType), elements[2])
        );
    }

    /** 配置字符串是否有效 */
    private static bool IsStrValid(string str)
    {
        return !string.IsNullOrEmpty(str) && str != "undefined";
    }
}

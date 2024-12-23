public enum GameFieldLayerType
{
    /// <summary> 地面 </summary>
    Ground = 0,
    /// <summary> 背景 </summary>
    Background = 1,
    /// <summary> 环境 </summary>
    Environment = 2,
    /// <summary> 实体 </summary>
    Entity = 3,
    /// <summary> 特效 </summary>
    VFX = 4,
    /// <summary> 场景UI </summary>
    SceneUI = 5,
}

public static class GameFieldLayerTypeExt
{
    public static string ToLayerName(this GameFieldLayerType layerType)
    {
        switch (layerType)
        {
            case GameFieldLayerType.Background:
                return GameFieldLayerCollection.BackgroundLayer;
            case GameFieldLayerType.Ground:
                return GameFieldLayerCollection.GroundLayer;
            case GameFieldLayerType.Environment:
                return GameFieldLayerCollection.EnvironmentLayer;
            case GameFieldLayerType.Entity:
                return GameFieldLayerCollection.EntityLayer;
            case GameFieldLayerType.VFX:
                return GameFieldLayerCollection.VFXLayer;
            case GameFieldLayerType.SceneUI:
                return GameFieldLayerCollection.SceneUILayer;
            default:
                return "Default";
        }
    }
}

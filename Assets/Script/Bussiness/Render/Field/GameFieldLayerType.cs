public enum GameFieldLayerType
{
    /// <summary> 无 </summary>`
    None = 0,
    /// <summary> 地面 </summary>
    Ground = 1,
    /// <summary> 背景 </summary>
    Background,
    /// <summary> 环境 </summary>
    Environment,
    /// <summary> 实体 </summary>
    Entity,
    /// <summary> 特效 </summary>
    VFX,
    /// <summary> 场景UI </summary>
    SceneUI,
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

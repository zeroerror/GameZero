public enum GameFieldLayerType
{
    Ground = 0,
    Background = 1,
    Environment = 2,
    Entity = 3,
    VFX = 4,
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

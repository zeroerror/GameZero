using GameVec2 = UnityEngine.Vector2;
public static class GameFieldLayerCollection
{
    public static readonly float CAMERA_Z = -500;

    public static readonly string BackgroundLayer = "Background";
    public static readonly int BackgroundLayerZ = 0;

    public static readonly string GroundLayer = "Ground";
    public static readonly int GroundLayerZ = 1000;

    public static readonly string EnvironmentLayer = "Environment";
    public static readonly int EnvironmentLayerZ = 2000;

    public static readonly string EntityLayer = "Entity";
    public static readonly int EntityLayerZ = 2000;

    public static readonly string VFXLayer = "VFX";
    public static readonly int VFXLayerZ = 20000;

    public static readonly string SceneUILayer = "SceneUI";
    public static readonly int SceneUILayerZ = 30000;

    public static readonly int StepZ = 100;
    public static readonly int InnerStepZ = 1;

    public static int GetLayerOrder(GameFieldLayerType layerType, in GameVec2 pos)
    {
        var rootOrder = 0;
        switch (layerType)
        {
            case GameFieldLayerType.Background:
                rootOrder = BackgroundLayerZ;
                break;
            case GameFieldLayerType.Ground:
                rootOrder = GroundLayerZ;
                break;
            case GameFieldLayerType.Environment:
                rootOrder = EnvironmentLayerZ;
                break;
            case GameFieldLayerType.Entity:
                rootOrder = EntityLayerZ;
                break;
            case GameFieldLayerType.VFX:
                rootOrder = VFXLayerZ;
                break;
            case GameFieldLayerType.SceneUI:
                rootOrder = SceneUILayerZ;
                break;
            default:
                rootOrder = 0;
                break;
        }
        return rootOrder - (int)(pos.y * StepZ);
    }
}

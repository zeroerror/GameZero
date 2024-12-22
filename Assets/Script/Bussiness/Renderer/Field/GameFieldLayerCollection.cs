using GameVec2 = UnityEngine.Vector2;
public static class GameFieldLayerCollection
{
    public static readonly float CAMERA_Z = -500;

    public static readonly string BackgroundLayer = "Background";
    public static readonly int BackgroundLayerZ = 0;

    public static readonly string GroundLayer = "Ground";
    public static readonly int GroundLayerZ = 1000;

    public static readonly string EnvironmentLayer = "Environment";
    public static readonly int EnvironmentLayerZ = 10000;

    public static readonly string EntityLayer = "Entity";
    public static readonly int EntityLayerZ = 10000;

    public static readonly string VFXLayer = "VFX";
    public static readonly int VFXLayerZ = 20000;

    public static readonly string SceneUILayer = "SceneUI";
    public static readonly int SceneUILayerZ = 30000;

    public static readonly int InnerStepZ = 1;

    public static int GetLayerOrder(GameFieldLayerType layerType, in GameVec2 pos)
    {
        int rootOrder;
        int stepZ = 100;
        switch (layerType)
        {
            case GameFieldLayerType.Background:
                rootOrder = BackgroundLayerZ;
                stepZ = 1;
                break;
            case GameFieldLayerType.Ground:
                rootOrder = GroundLayerZ;
                stepZ = 1;
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
                stepZ = 1;
                break;
            default:
                rootOrder = 0;
                break;
        }
        return rootOrder - (int)(pos.y * stepZ);
    }
}

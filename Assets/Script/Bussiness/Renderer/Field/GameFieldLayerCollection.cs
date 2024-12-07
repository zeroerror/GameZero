public static class GameFieldLayerCollection
{
    public static readonly string BackgroundLayer = "Background";
    public static readonly float BackgroundLayerZ = 0.0f;

    public static readonly string GroundLayer = "Ground";
    public static readonly float GroundLayerZ = -10.0f;

    public static readonly string EnvironmentLayer = "Environment";
    public static readonly float EnvironmentLayerZ = -20.0f;

    public static readonly string EntityLayer = "Entity";
    public static readonly float EntityLayerZ = -20.0f;

    public static readonly string VFXLayer = "VFX";
    public static readonly float VFXLayerZ = -40.0f;

    public static readonly string SceneUILayer = "SceneUI";
    public static readonly float SceneUILayerZ = -50.0f;

    public static readonly float CAMERA_Z = -500.0f;

    public static readonly float StepZ = 0.01f;
    public static readonly float InnerStepZ = 0.0001f;
}

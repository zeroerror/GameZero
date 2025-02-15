using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public static class GameRoleCollection
    {
        /// <summary> 角色的逻辑盒大小 </summary>
        public static GameVec2 ROLE_LOGIC_SIZE = new GameVec2(1.0f, 1.5f);
        /// <summary> 角色的碰撞盒大小 </summary>
        public static float ROLE_COLLIDER_RADIUS = 0.25f;
    }
}
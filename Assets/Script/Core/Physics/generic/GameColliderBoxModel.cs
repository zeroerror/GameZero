using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    /// <summary>
    /// 盒子碰撞器模型, 用于描述XY平面上的碰撞盒
    /// </summary>
    public class GameColliderBoxModel
    {
        // 碰撞盒宽度
        public float width;

        // 碰撞盒高度
        public float height;

        // 碰撞盒旋转角度
        public float angle;

        // 初始坐标偏移
        public GameVec2 offset => _offset;

        private GameVec2 _offset;

        // 构造函数
        public GameColliderBoxModel(float width, float height, float angle, GameVec2 offset)
        {
            this.width = width;
            this.height = height;
            this.angle = angle;
            _offset = offset;
        }

        // 将碰撞盒描述为字符串
        public override string ToString()
        {
            return $"宽度: {width}, 高度: {height}, 旋转角度: {angle}, 偏移: {offset}";
        }
    }
}
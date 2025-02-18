using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public class GameRoleCollectionR
    {
        public static readonly string ROLE_ANIM_NAME_IDLE = "idle";
        public static readonly string ROLE_ANIM_NAME_MOVE = "walk";
        public static readonly string ROLE_ANIM_NAME_ATTACK = "attack";
        public static readonly string ROLE_ANIM_NAME_DEAD = "dead";
        /// <summary> 多层级动画名关键字 </summary>
        public static readonly string[] ROLE_MULTY_LAYER_ANIM_KEYS = { "idle", "move" };
        /// <summary> 基础动画名关键字 </summary>
        public static readonly string[] ROLE_BASE_ANIM_KEYS = { "idle", "move", "attack", "dead" };

        /// <summary> 血条偏移 </summary>
        public static readonly Vector2 ROLE_ATTRIBUTE_SLIDER_HP_OFFSET = new Vector2(0, 75);
        /// <summary> 血条大小 </summary>
        public static readonly Vector2 ROLE_ATTRIBUTE_SLIDER_HP_SIZE = new Vector2(75, 10);
        /// <summary> 蓝条偏移 </summary>
        public static readonly Vector2 ROLE_ATTRIBUTE_SLIDER_MP_OFFSET = new Vector2(0, 68);
        /// <summary> 蓝条大小 </summary>
        public static readonly Vector2 ROLE_ATTRIBUTE_SLIDER_MP_SIZE = new Vector2(75, 10);
        /// <summary> 护盾条偏移 </summary>
        public static readonly Vector2 ROLE_ATTRIBUTE_SLIDER_SHIELD_OFFSET = new Vector2(0, 75);
        /// <summary> 护盾条大小 </summary>
        public static readonly Vector2 ROLE_ATTRIBUTE_SLIDER_SHIELD_SIZE = new Vector2(75, 10);
        /// <summary> 分割线数值 </summary>
        public static readonly int ROLE_ATTRIBUTE_SLIDER_DIVISION = 100;
    }
}
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public class GameRoleCollectionR
    {
        public static readonly string ROLE_ANIM_NAME_IDLE = "idle";
        public static readonly string ROLE_ANIM_NAME_MOVE = "walk";
        public static readonly string ROLE_ANIM_NAME_ATTACK = "attack";
        public static readonly string ROLE_ANIM_NAME_DEAD = "dead";

        /// <summary> 血条偏移 </summary>
        public static readonly Vector2 ROLE_ATTRIBUTE_SLIDER_HP_OFFSET = new Vector2(0, 75);
        /// <summary> 血条大小 </summary>
        public static readonly Vector2 ROLE_ATTRIBUTE_SLIDER_HP_SIZE = new Vector2(75, 10);
        /// <summary> 蓝条偏移 </summary>
        public static readonly Vector2 ROLE_ATTRIBUTE_SLIDER_MP_OFFSET = new Vector2(0, 65);
        /// <summary> 蓝条大小 </summary>
        public static readonly Vector2 ROLE_ATTRIBUTE_SLIDER_MP_SIZE = new Vector2(75, 10);
        /// <summary> 护盾条偏移 </summary>
        public static readonly Vector2 ROLE_ATTRIBUTE_SLIDER_SHIELD_OFFSET = new Vector2(0, 75);
        /// <summary> 护盾条大小 </summary>
        public static readonly Vector2 ROLE_ATTRIBUTE_SLIDER_SHIELD_SIZE = new Vector2(75, 10);
    }
}
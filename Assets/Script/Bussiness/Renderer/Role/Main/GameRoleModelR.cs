using GamePlay.Config;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleModelR
    {
        /// <summary> 角色类型Id </summary>
        public readonly int typeId;
        /// <summary> 角色名称 </summary>
        public readonly string roleName;
        /// <summary> 角色描述 </summary>
        public readonly string desc;
        /// <summary> 预制体路径 </summary>
        public readonly string prefabUrl;
        /// <summary> 角色技能列表 </summary>
        public readonly GameSkillSO[] skills;
        /// <summary> 是否为多层级动画 </summary>
        public readonly bool isMultyAnimationLayer;

        public GameRoleModelR(
            int typeId,
            string roleName,
            string desc,
            string prefabUrl,
            GameSkillSO[] skills,
            bool isMultyAnimationLayer)
        {
            this.typeId = typeId;
            this.roleName = roleName;
            this.desc = desc;
            this.prefabUrl = prefabUrl;
            this.skills = skills;
            this.isMultyAnimationLayer = isMultyAnimationLayer;
        }
    }
}
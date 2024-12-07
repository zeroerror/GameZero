using GamePlay.Config;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleModelR
    {
        public readonly string roleName;
        public readonly string desc;
        public readonly GameObject prefab;
        public readonly GameSkillSO[] skills;

        public GameRoleModelR(string roleName, string desc, GameObject prefab, GameSkillSO[] skills)
        {
            this.roleName = roleName;
            this.desc = desc;
            this.prefab = prefab;
            this.skills = skills;
        }
    }
}
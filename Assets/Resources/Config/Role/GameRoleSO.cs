using System.Linq;
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_role_", menuName = "游戏玩法/配置/角色模板")]
    public class GameRoleSO : GameSOBase
    {
        [Header("名称")]
        public string roleName;
        [Header("描述")]
        public string desc;
        [Header("预制体")]
        public GameObject prefab;
        [Header("技能列表")]
        public GameSkillSO[] skills;
        [Header("属性列表")]
        public GameAttribute[] attributes;

        public GameRoleModel ToModel()
        {
            var skillIds = skills?.Select(s => s.typeId).ToArray();
            var model = new GameRoleModel(typeId, skillIds, attributes);
            return model;
        }

        public GameRoleModelR ToModelR()
        {
            var model = new GameRoleModelR(roleName, desc, prefab, skills);
            return model;
        }

    }
}

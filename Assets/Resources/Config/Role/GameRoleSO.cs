using System.Linq;
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_role_", menuName = "游戏玩法/配置/角色模板")]
    public class GameRoleSO : GameSOBase
    {
        public string roleName;
        public string desc;
        public GameObject prefab;
        public string prefabUrl;
        public GameSkillSO[] skills;
        public GameAttribute[] attributes;

        public GameRoleModel ToModel()
        {
            var skillIds = skills?.Select(s => s.typeId).ToArray();
            var model = new GameRoleModel(typeId, skillIds, attributes);
            return model;
        }

        public GameRoleModelR ToModelR()
        {
            var isMultyAnimationLayer = this.typeId < 1000;
            var model = new GameRoleModelR(this.typeId, roleName, desc, prefabUrl, skills, isMultyAnimationLayer);
            return model;
        }

    }
}
